using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

using System.Diagnostics;

using dnppv.contracts.domainmodel;
using dnppv.contracts.fileadapter;
using dnppv.contracts.patternrecognizer;
using dnppv.pile;

[assembly: InternalsVisibleTo("test.whitebox.pilepatternrecognizer")]

namespace dnppv.pilepatternrecognizer
{
    public class PilePatternRecognizer : IPatternRecognizer
    {
        private TraceSource ts = new TraceSource("PilePatternRecognizer", SourceLevels.Off);
        private DateTime processingStarted;


        #region IPatternRecognizer Members

        public IPatternList DetectPatterns(IFileAdapter file)
        {
            #region trace
            this.ts.TraceEvent(TraceEventType.Start, 10, "PilePatternRecognizer.DetectPatterns started on <{0}>", System.IO.Path.GetFileName(file.Filename));
            #endregion
            this.processingStarted = DateTime.Now;

            // convert signals to tv relations
            MemoryPile<Signal, Pair> pile = new MemoryPile<Signal, Pair>();
            SortedList<int, RelationItem> signals = this.ConvertSignalsToTerminalValues(file, pile);
            #region trace
            this.ts.TraceEvent(TraceEventType.Information, 20, "# of signals received: {0}, time elapsed: {1}", signals.Count, DateTime.Now.Subtract(this.processingStarted));
            #endregion
            // pair relations until no more patterns are found
            List<List<Pair>> patternLayers;
            patternLayers = this.FindPatterns(signals, pile);
            #region trace
            this.ts.TraceEvent(TraceEventType.Information, 30, "# of pattern layers: {0}, time elapsed: {1}", patternLayers.Count, DateTime.Now.Subtract(this.processingStarted)); 
            #endregion
            // build pattern list
            IPatternList patternList = this.BuildPatternList(patternLayers, signals.Count);
            #region trace
            this.ts.TraceEvent(TraceEventType.Information, 40, "# of patterns found: {0}, time elapsed: {1}", patternList.Count, DateTime.Now.Subtract(this.processingStarted)); 
            this.ts.TraceEvent(TraceEventType.Information, 45, "Pile stats: # of TVs: {0}, # of inner rels: {1}", pile.CountTV, pile.CountInnerRelation);
            this.ts.TraceEvent(TraceEventType.Stop, 50, "PilePatternRecognizer.DetectPatterns finished"); 
            #endregion

            return patternList;
        }
        #endregion


        internal IPatternList BuildPatternList(List<List<Pair>> patternLayers, int signalCount)
        {
            #region trace
            this.ts.TraceEvent(TraceEventType.Start, 100, "BuildPatternList started"); 
            #endregion
            PatternList patterns = new PatternList(signalCount);

            // on all layers of patterns, i.e. for all pattern sizes
            // compile the patterns with their occurrences
            for (int layer = 0; layer < patternLayers.Count; layer++)
            {
                #region trace
                this.ts.TraceEvent(TraceEventType.Information, 110, "Pattern layer: {0}", layer); 
                #endregion
                // for all patterns of the same size...
                foreach (Pair sr in patternLayers[layer])
                {
                    // create a pattern object...
                    int patternSize = layer + 2;
                    Pattern p = patterns.Add(patternSize);
                    // and add all occurrences
                    foreach (int start in sr.Occurrences)
                        p.Add(start);
                }
                #region trace
                this.ts.TraceEvent(TraceEventType.Information, 120, "Running total # of patterns: {0}", patterns.Count); 
                #endregion
            }
            #region trace
            this.ts.TraceEvent(TraceEventType.Stop, 190, "BuildPatternList finished"); 
            #endregion
            return patterns;
        }


        internal List<List<Pair>> FindPatterns(SortedList<int, RelationItem> signals, MemoryPile<Signal, Pair> pile)
        {
            #region trace
            this.ts.TraceEvent(TraceEventType.Start, 200, "FindPatterns started");
            #endregion
            List<List<Pair>> patternsInAllLayers = new List<List<Pair>>();

            SortedList<int, RelationItem> pairedRelations = signals;
            List<Pair> patternsInLayer;
            do
            {
                #region trace
                this.ts.TraceEvent(TraceEventType.Information, 210, "Relational layer: {0}, # of relations: {1}", patternsInAllLayers.Count, pairedRelations.Count); 
                #endregion
                patternsInLayer = new List<Pair>();
                pairedRelations = PairRelations(pairedRelations, pile, patternsInLayer, patternsInAllLayers.Count+1);
                #region trace
                this.ts.TraceEvent(TraceEventType.Information, 220, "# of patterns in layer: {0}, time elapsed: {1}", patternsInLayer.Count, DateTime.Now.Subtract(this.processingStarted)); 
                #endregion
                if (patternsInLayer.Count > 0) patternsInAllLayers.Add(patternsInLayer);
            } while(pairedRelations.Count > 0);
            #region trace
            this.ts.TraceEvent(TraceEventType.Stop, 290, "FindPatterns finished");
            #endregion
            return patternsInAllLayers;
        }


        internal SortedList<int, RelationItem> PairRelations(SortedList<int, RelationItem> relations, MemoryPile<Signal, Pair> pile, List<Pair> patternsInLayer, int layerIndex)
        {
            #region trace
            this.ts.TraceEvent(TraceEventType.Verbose, 300, "PairRelations started @ {0}", DateTime.Now.Subtract(this.processingStarted)); 
            #endregion

            #region Collect all patterns in layer
            RelationItem prevRelItem = null;

            foreach(int i in relations.Keys)
            {
                RelationItem relItem = relations[i];

                if (prevRelItem != null)
                {
                    // check, if the current rel is adjacent to the prev rel...
                    if (prevRelItem.Index == relItem.Index - 1)
                    {
                        // yes, then combine both into a new rel for the next layer
                        bool isNew;
                        Pair newRel = pile.Create(prevRelItem.Relation, relItem.Relation, out isNew);
                        newRel.AddOccurrence(i - 1);

                        if (newRel.OccurrenceCount == 2)
                            patternsInLayer.Add(newRel);
                    }
                }

                prevRelItem = relItem;
            }
            this.ts.TraceEvent(TraceEventType.Verbose, 310, "# of patterns collected: {0}, time elapsed: {1}", patternsInLayer.Count, DateTime.Now.Subtract(this.processingStarted)); 
            #endregion

            #region Buid next layer from adjacent patterns only
            SortedList<int, RelationItem> relationPairs = new SortedList<int, RelationItem>();

            // build sequence of only pattern relations in the order of their occurrences
            foreach (Pair p in patternsInLayer)
                foreach (int o in p.Occurrences)
                    relationPairs.Add(o, new RelationItem(o, p));
            this.ts.TraceEvent(TraceEventType.Verbose, 320, "# of relation pairs: {0}, time elapsed: {1}", relationPairs.Count, DateTime.Now.Subtract(this.processingStarted)); 

            // remove single relations, not adjacent to others
            bool wasAdjacent = true;
            List<int> keys = new List<int>(relationPairs.Keys);
            for(int i=0; i<keys.Count; i++)
            {
                int k = keys[i];
                bool isAdjacent = i > 0 && keys[i - 1] == k - 1;
                if (!wasAdjacent && !isAdjacent)
                        relationPairs.Remove(keys[i - 1]);
                wasAdjacent = isAdjacent;
            }
            if (!wasAdjacent)
                relationPairs.Remove(keys[keys.Count - 1]);
            this.ts.TraceEvent(TraceEventType.Verbose, 320, "# of relation pairs final: {0}, time elapsed: {1}", relationPairs.Count, DateTime.Now.Subtract(this.processingStarted)); 

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
