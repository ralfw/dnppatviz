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
    public class PilePatternRecognizer_OLD : IPatternRecognizer
    {
        private TraceSource ts = new TraceSource("PilePatternRecognizer", SourceLevels.All);
        private DateTime processingStarted;


        #region IPatternRecognizer Members

        public IPatternList DetectPatterns(IFileAdapter file)
        {
            this.ts.TraceEvent(TraceEventType.Start, 10, "PilePatternRecognizer.DetectPatterns started"); this.ts.Flush();
            this.processingStarted = DateTime.Now;

            // convert signals to tv relations
            MemoryPile<Signal, Pair> pile = new MemoryPile<Signal, Pair>();
            List<RelationBase> signals = this.ConvertSignalsToTerminalValues(file, pile);
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


        internal List<RelationBase> ConvertSignalsToTerminalValues(IFileAdapter file, MemoryPile<Signal, Pair> pile)
        {
            List<RelationBase> signals = new List<RelationBase>();
            while (file.Read())
                signals.Add(pile.Create(file.CurrentSignal));
            return signals;
        }


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


        internal List<Dictionary<long, Pair>> FindPatterns(List<RelationBase> signals, MemoryPile<Signal, Pair> pile)
        {
            this.ts.TraceEvent(TraceEventType.Start, 200, "FindPatterns started");

            List<Dictionary<long, Pair>> patternsInAllLayers = new List<Dictionary<long, Pair>>();

            List<RelationBase> pairedRelations = signals;
            Dictionary<long, Pair> patternsInLayer;
            do
            {
                this.ts.TraceEvent(TraceEventType.Information, 210, "Relational layer: {0}, # of relations: {1}", patternsInAllLayers.Count, pairedRelations.Count); this.ts.Flush();

                patternsInLayer = new Dictionary<long,Pair>();
                pairedRelations = PairRelations(pairedRelations, pile, patternsInLayer);
                this.ts.TraceEvent(TraceEventType.Information, 220, "# of patterns in layer: {0}, time elapsed: {1}", patternsInLayer.Count, DateTime.Now.Subtract(this.processingStarted)); this.ts.Flush();

                if (patternsInLayer.Count > 0) patternsInAllLayers.Add(patternsInLayer);
            } while(patternsInLayer.Count > 0);

            this.ts.TraceEvent(TraceEventType.Stop, 290, "FindPatterns finished");

            return patternsInAllLayers;
        }


        internal List<RelationBase> PairRelations(List<RelationBase> relations, MemoryPile<Signal, Pair> pile, Dictionary<long, Pair> patternsInLayer)
        {
            List<RelationBase> relationPairs = new List<RelationBase>();

            RelationBase prevRel = null;
            for(int i=0; i<relations.Count; i++)
            {
                RelationBase rel = relations[i];

                if (prevRel != null)
                {
                    bool isNew;
                    Pair newRel = pile.Create(prevRel, rel, out isNew);
                    newRel.AddOccurrence(i-1);

                    relationPairs.Add(newRel);

                    if (!isNew)
                        if (!patternsInLayer.ContainsKey(newRel.Id))
                            patternsInLayer.Add(newRel.Id, newRel);
                }
                prevRel = rel;
            }

            return relationPairs;
        }
    }
}
