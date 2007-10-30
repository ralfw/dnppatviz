using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

using System.Diagnostics;

using dnppv.contracts.fileadapter;
using dnppv.contracts.patternrecognizer;
using dnppv.pile;

[assembly: InternalsVisibleTo("test.whitebox.pilepatternrecognizer")]

namespace dnppv.pilepatternrecognizer
{
    public class PilePatternRecognizer : IPatternRecognizer
    {
        private TraceSource ts = new TraceSource("PilePatternRecognizer", SourceLevels.All);
        private DateTime processingStarted;


        #region IPatternRecognizer Members

        public IPatternList DetectPatterns(IFileAdapter file)
        {
            this.ts.TraceEvent(TraceEventType.Start, 10, "PilePatternRecognizer.DetectPatterns started on <{0}>", System.IO.Path.GetFileName(file.Filename)); this.ts.Flush();
            this.processingStarted = DateTime.Now;

            // convert signals to tv relations
            MemoryPile<Signal, Pair> pile = new MemoryPile<Signal, Pair>();
            SortedList<int, RelationItem> signals = this.ConvertSignalsToTerminalValues(file, pile);
            this.ts.TraceEvent(TraceEventType.Information, 20, "# of signals received: {0}, time elapsed: {1}", signals.Count, DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();

            // pair relations until no more patterns are found
            List<Dictionary<long, Pair>> patternLayers;
            patternLayers = this.FindPatterns(signals, pile);
            this.ts.TraceEvent(TraceEventType.Information, 30, "# of pattern layers: {0}, time elapsed: {1}", patternLayers.Count, DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();

            // build pattern list
            IPatternList patternList = this.BuildPatternList(patternLayers, signals.Count);
            this.ts.TraceEvent(TraceEventType.Information, 40, "# of patterns found: {0}, time elapsed: {1}", patternList.Count, DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();
            this.ts.TraceEvent(TraceEventType.Information, 45, "Pile stats: # of TVs: {0}, # of inner rels: {1}", pile.CountTV, pile.CountInnerRelation);
            this.ts.TraceEvent(TraceEventType.Stop, 50, "PilePatternRecognizer.DetectPatterns finished"); this.ts.Flush();

            return patternList;
        }
        #endregion


        internal IPatternList BuildPatternList(List<Dictionary<long, Pair>> patternLayers, int signalCount)
        {
            this.ts.TraceEvent(TraceEventType.Start, 100, "BuildPatternList started"); this.ts.Flush();

            PatternList patterns = new PatternList(signalCount);

            // on all layers of patterns, i.e. for all pattern sizes
            // compile the patterns with their occurrences
            for (int layer = 0; layer < patternLayers.Count; layer++)
            {
                this.ts.TraceEvent(TraceEventType.Information, 110, "Pattern layer: {0}", layer); this.ts.Flush();

                // for all patterns of the same size...
                foreach (Pair sr in patternLayers[layer].Values)
                {
                    // create a pattern object...
                    int patternSize = layer + 2;
                    Pattern p = patterns.Add(patternSize);
                    // and add all occurrences
                    foreach (int start in sr.Occurrences)
                        p.Add(start);
                }

                this.ts.TraceEvent(TraceEventType.Information, 120, "Running total # of patterns: {0}", patterns.Count); this.ts.Flush();
            }

            this.ts.TraceEvent(TraceEventType.Stop, 190, "BuildPatternList finished"); this.ts.Flush();

            return patterns;
        }


        internal List<Dictionary<long, Pair>> FindPatterns(SortedList<int, RelationItem> signals, MemoryPile<Signal, Pair> pile)
        {
            this.ts.TraceEvent(TraceEventType.Start, 200, "FindPatterns started");

            List<Dictionary<long, Pair>> patternsInAllLayers = new List<Dictionary<long, Pair>>();

            SortedList<int, RelationItem> pairedRelations = signals;
            Dictionary<long, Pair> patternsInLayer;
            do
            {
                this.ts.TraceEvent(TraceEventType.Information, 210, "Relational layer: {0}, # of relations: {1}", patternsInAllLayers.Count, pairedRelations.Count); this.ts.Flush();

                patternsInLayer = new Dictionary<long,Pair>();
                pairedRelations = PairRelations(pairedRelations, pile, patternsInLayer, patternsInAllLayers.Count+1);
                this.ts.TraceEvent(TraceEventType.Information, 220, "# of patterns in layer: {0}, time elapsed: {1}", patternsInLayer.Count, DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();

                if (patternsInLayer.Count > 0) patternsInAllLayers.Add(patternsInLayer);
            } while(pairedRelations.Count > 0);

            this.ts.TraceEvent(TraceEventType.Stop, 290, "FindPatterns finished");

            return patternsInAllLayers;
        }


        internal SortedList<int, RelationItem> PairRelations(SortedList<int, RelationItem> relations, MemoryPile<Signal, Pair> pile, Dictionary<long, Pair> patternsInLayer, int layerIndex)
        {
            this.ts.TraceEvent(TraceEventType.Verbose, 300, "PairRelations started @ {0}", DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();

            #region Collect all patterns in layer
            RelationItem prevRelItem = null;

            foreach(int i in relations.Keys)
            {
                RelationItem relItem = relations[i];

                if (prevRelItem != null)
                {
                    // check, if the current rel is adjacent to the prev rel
                    if (prevRelItem.Index == relItem.Index - 1)
                    {
                        bool isNew;
                        Pair newRel = pile.Create(prevRelItem.Relation, relItem.Relation, out isNew);
                        newRel.AddOccurrence(i - 1);

                        if (!isNew)
                            if (!patternsInLayer.ContainsKey(newRel.Id))
                                patternsInLayer.Add(newRel.Id, newRel);
                    }
                }
                prevRelItem = relItem;
            }
            this.ts.TraceEvent(TraceEventType.Verbose, 310, "# of patterns collected: {0}, time elapsed: {1}", patternsInLayer.Count, DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();
            #endregion

            #region Buid next layer from adjacent patterns only
            SortedList<int, RelationItem> relationPairs = new SortedList<int, RelationItem>();

            // build sequence of only pattern relations in the order of their occurrences
            foreach (Pair p in patternsInLayer.Values)
                foreach (int o in p.Occurrences)
                    relationPairs.Add(o, new RelationItem(o, p));
            this.ts.TraceEvent(TraceEventType.Verbose, 320, "# of relation pairs: {0}, time elapsed: {1}", relationPairs.Count, DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();

            // remove groups of adjacent pattern relations to short for the layer in under word
            // layer 0: roots/signals (single relations) = no removals
            // layer 1: pairs of roots (relations representing 2 relations) = remove groups of <1 relations, i.e. remove no relations
            // layer 2: relations representing 3 relations = remove groups of <2 relations
            // layer n: remote groups of <n relations
            List<RelationItem> patternGroup = null;
            List<int> keys = new List<int>(relationPairs.Keys);
            foreach (int i in keys)
            {
                if (patternGroup == null)
                {
                    patternGroup = new List<RelationItem>();
                    patternGroup.Add(relationPairs[i]);
                }
                else
                {
                    RelationItem groupTailPattern = patternGroup[patternGroup.Count - 1];
                    RelationItem currentPattern = relationPairs[i];

                    if (groupTailPattern.Index == currentPattern.Index - 1)
                        patternGroup.Add(currentPattern); // add pattern to group since it´s adjacent to previous pattern
                    else
                    {
                        // check, if pattern group is too short for pattern layer...
                        if (patternGroup.Count < layerIndex)
                        {
                            // yes, group too short: delete group members from future layer
                            foreach (RelationItem patternItem in patternGroup)
                                relationPairs.Remove(patternItem.Index);
                        }

                        // start new pattern group with current pattern
                        patternGroup = new List<RelationItem>();
                        patternGroup.Add(currentPattern);
                    }
                }
            }
            // check, if pattern group is too short for pattern layer...
            if (patternGroup != null && patternGroup.Count < layerIndex)
            {
                // yes, group too short: delete group members from future layer
                foreach (RelationItem patternItem in patternGroup)
                    relationPairs.Remove(patternItem.Index);
            }
            this.ts.TraceEvent(TraceEventType.Verbose, 320, "# of relation pairs final: {0}, time elapsed: {1}", relationPairs.Count, DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();

            return relationPairs;
            #endregion
        }

        
        internal SortedList<int, RelationItem> ConvertSignalsToTerminalValues(IFileAdapter file, MemoryPile<Signal, Pair> pile)
        {
            SortedList<int, RelationItem> signals = new SortedList<int, RelationItem>();
            int i = 0;
            while (file.Read())
            {
                signals.Add(i, new RelationItem(i, pile.Create(file.CurrentSignal)));
                i++;
            }
            return signals;
        }
    }


    internal class RelationItem
    {
        private int index;
        private RelationBase relation;

        public RelationItem(int index, RelationBase relation)
        {
            this.index = index;
            this.relation = relation;
        }

        public int Index
        {
            get { return this.index; }
        }

        public RelationBase Relation
        {
            get { return this.relation; }
        }
    }
}
