using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

using dnppv.contracts.fileadapter;
using dnppv.contracts.patternrecognizer;
using dnppv.pile;

[assembly: InternalsVisibleTo("test.whitebox.pilepatternrecognizer")]

namespace dnppv.pilepatternrecognizer
{
    public class PilePatternRecognizer : IPatternRecognizer
    {
        #region IPatternRecognizer Members

        public IPatternList DetectPatterns(IFileAdapter file)
        {
            return null;
        }

        #endregion


        internal List<RelationBase> ConvertSignalsToTerminalValues(IFileAdapter file, MemoryPile<Signal, SignalRelation> pile)
        {
            List<RelationBase> signals = new List<RelationBase>();
            while (file.Read())
                signals.Add(pile.Create(file.CurrentSignal));
            return signals;
        }


        internal List<RelationBase> CombineRelations(List<RelationBase> relations, MemoryPile<Signal, SignalRelation> pile, Dictionary<long, SignalRelation> patternsInLayer)
        {
            List<RelationBase> combinedRelations = new List<RelationBase>();

            RelationBase prevRel = null;
            foreach (RelationBase rel in relations)
            {
                if (prevRel != null)
                {
                    bool isNew;
                    SignalRelation newRel = pile.Create(prevRel, rel, out isNew);
                    combinedRelations.Add(newRel);

                    if (!isNew)
                        if (!patternsInLayer.ContainsKey(newRel.Id))
                            patternsInLayer.Add(newRel.Id, newRel);
                }
                prevRel = rel;
            }

            return combinedRelations;
        }
    }
}
