using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using dnppv.contracts.patternrecognizer;
using dnppv.pilepatternrecognizer;
using dnppv.textfileadapter;
using dnppv.pile;

namespace test.whitebox.pilepatternrecognizer
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void testConvertSignalsToTerminalValues()
        {
            PilePatternRecognizer r = new PilePatternRecognizer();
            MemoryPile<Signal, Pair> pile = new MemoryPile<Signal, Pair>();
            using(TextFileAdapter file = new TextFileAdapter(@"..\..\test1.txt"))
            {
                SortedList<int, RelationItem> signals;
                signals = r.ConvertSignalsToTerminalValues(file, pile);

                Assert.AreEqual(3, signals.Count);
                Assert.AreEqual("a", ((Signal)signals[0].Relation).Key);
                Assert.AreEqual("b", ((Signal)signals[1].Relation).Key);
                Assert.AreEqual("c", ((Signal)signals[2].Relation).Key);

                bool isNew;
                Signal s;
                s = pile.Create("a", out isNew);
                Assert.IsFalse(isNew);
            }
        }

        [Test]
        public void testCombineRelations()
        {
            PilePatternRecognizer r = new PilePatternRecognizer();
            
            MemoryPile<Signal, Pair> pile;
            SortedList<int, RelationItem> signals;
            Dictionary<long, Pair> patternsInLayer;
            SortedList<int, RelationItem> relations;

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test1.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();

                signals = r.ConvertSignalsToTerminalValues(file, pile);
                Assert.AreEqual(3, signals.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(signals, pile, patternsInLayer, 1);
                Assert.AreEqual(0, relations.Count);
            }

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test1a.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();

                signals = r.ConvertSignalsToTerminalValues(file, pile);
                Assert.AreEqual(5, signals.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(signals, pile, patternsInLayer, 1);
                Assert.AreEqual(2, relations.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(relations, pile, patternsInLayer, 2);
                Assert.AreEqual(0, relations.Count);
            }

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test1b.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();

                signals = r.ConvertSignalsToTerminalValues(file, pile);
                Assert.AreEqual(7, signals.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(signals, pile, patternsInLayer, 1);
                Assert.AreEqual(4, relations.Count);

                Assert.AreEqual("a", ((Signal)((Pair)relations[0].Relation).NormParent).Key);
                Assert.AreEqual("b", ((Signal)((Pair)relations[0].Relation).AssocParent).Key);
                Assert.AreEqual("c", ((Signal)((Pair)relations[1].Relation).AssocParent).Key);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(relations, pile, patternsInLayer, 2);
                Assert.AreEqual(0, relations.Count);
            }

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test1c.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();

                signals = r.ConvertSignalsToTerminalValues(file, pile);
                Assert.AreEqual(9, signals.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(signals, pile, patternsInLayer, 1);
                Assert.AreEqual(6, relations.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(relations, pile, patternsInLayer, 2);
                Assert.AreEqual(4, relations.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(relations, pile, patternsInLayer, 3);
                Assert.AreEqual(0, relations.Count);
            }
        }


        [Test]
        public void testPatternsInLevel()
        {
            PilePatternRecognizer r = new PilePatternRecognizer();
            MemoryPile<Signal, Pair> pile = new MemoryPile<Signal, Pair>();
            SortedList<int, RelationItem> signals;
            SortedList<int, RelationItem> relations;
            Dictionary<long, Pair> patternsInLayer;
            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test1.txt"))
            {
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                Assert.AreEqual(3, signals.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(signals, pile, patternsInLayer, 1);
                Assert.AreEqual(0, relations.Count);
                Assert.AreEqual(0, patternsInLayer.Count);
            }

            pile = new MemoryPile<Signal, Pair>();
            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test2.txt"))
            {
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                Assert.AreEqual(11, signals.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(signals, pile, patternsInLayer, 1);
                Assert.AreEqual(5, relations.Count);
                Assert.AreEqual(2, patternsInLayer.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(relations, pile, patternsInLayer, 2);
                Assert.AreEqual(0, relations.Count);
                Assert.AreEqual(0, patternsInLayer.Count);
            }

            pile = new MemoryPile<Signal, Pair>();
            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test3.txt"))
            {
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                Assert.AreEqual(12, signals.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(signals, pile, patternsInLayer, 1);
                Assert.AreEqual(6, relations.Count);
                Assert.AreEqual(2, patternsInLayer.Count);

                patternsInLayer = new Dictionary<long, Pair>();
                relations = r.PairRelations(relations, pile, patternsInLayer, 2);
                Assert.AreEqual(0, relations.Count);
                Assert.AreEqual(1, patternsInLayer.Count);
            }
        }


        [Test]
        public void testFindPatterns()
        {
            PilePatternRecognizer r = new PilePatternRecognizer();
            MemoryPile<Signal, Pair> pile;

            SortedList<int, RelationItem> signals;
            List<Dictionary<long, Pair>> patternLayers;

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test1.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                patternLayers = r.FindPatterns(signals, pile);
                Assert.AreEqual(0, patternLayers.Count);
            }

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test2.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                patternLayers = r.FindPatterns(signals, pile);
                Assert.AreEqual(1, patternLayers.Count);
                Assert.AreEqual(2, patternLayers[0].Count);

                List<Pair> patterns = new List<Pair>(patternLayers[0].Values);
                Assert.AreEqual(3, patterns[0].Occurrences.Length);
                Assert.AreEqual(0, patterns[0].Occurrences[0]);
                Assert.AreEqual(3, patterns[0].Occurrences[1]);
                Assert.AreEqual(9, patterns[0].Occurrences[2]);

                Assert.AreEqual(2, patterns[1].Occurrences.Length);
                Assert.AreEqual(1, patterns[1].Occurrences[0]);
                Assert.AreEqual(6, patterns[1].Occurrences[1]);
            }

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test3.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                patternLayers = r.FindPatterns(signals, pile);
                Assert.AreEqual(2, patternLayers.Count);
                Assert.AreEqual(2, patternLayers[0].Count);
                Assert.AreEqual(1, patternLayers[1].Count);

                // patterns w size=2
                List<Pair> patterns = new List<Pair>(patternLayers[0].Values);
                // ab
                Assert.AreEqual(3, patterns[0].Occurrences.Length);
                Assert.AreEqual(0, patterns[0].Occurrences[0]);
                Assert.AreEqual(3, patterns[0].Occurrences[1]);
                Assert.AreEqual(9, patterns[0].Occurrences[2]);

                // bx
                Assert.AreEqual(3, patterns[1].Occurrences.Length);
                Assert.AreEqual(1, patterns[1].Occurrences[0]);
                Assert.AreEqual(6, patterns[1].Occurrences[1]);
                Assert.AreEqual(10, patterns[1].Occurrences[2]);

                // patterns w size=3
                patterns = new List<Pair>(patternLayers[1].Values);
                // abx
                Assert.AreEqual(2, patterns[0].Occurrences.Length);
                Assert.AreEqual(0, patterns[0].Occurrences[0]);
                Assert.AreEqual(9, patterns[0].Occurrences[1]);
            }
        }


        [Test]
        public void testPatternListObjectModel()
        {
            PatternList pl = new PatternList(100);
            Pattern p;

            p = pl.Add(2);
            p.Add(20);
            p.Add(27);

            p = pl.Add(3);
            p.Add(30);
            p.Add(34);

            p = pl.Add(3);
            p.Add(32);
            p.Add(36);
            p.Add(39);

            Assert.AreEqual(100, pl.SignalCount);
            Assert.AreEqual(3, pl.Count);
            
            Assert.AreEqual(2, pl[0].Size);
            Assert.AreEqual(2, pl[0].Count);
            Assert.AreEqual(20, pl[0][0].Start);
            Assert.AreEqual(21, pl[0][0].End);
            Assert.AreEqual(27, pl[0][1].Start);
            Assert.AreEqual(28, pl[0][1].End);

            Assert.AreEqual(3, pl[1].Size);
            Assert.AreEqual(2, pl[1].Count);
            Assert.AreEqual(30, pl[1][0].Start);
            Assert.AreEqual(32, pl[1][0].End);
            Assert.AreEqual(34, pl[1][1].Start);
            Assert.AreEqual(36, pl[1][1].End);

            Assert.AreEqual(3, pl[2].Size);
            Assert.AreEqual(3, pl[2].Count);
            Assert.AreEqual(32, pl[2][0].Start);
            Assert.AreEqual(34, pl[2][0].End);
            Assert.AreEqual(36, pl[2][1].Start);
            Assert.AreEqual(38, pl[2][1].End);
            Assert.AreEqual(39, pl[2][2].Start);
            Assert.AreEqual(41, pl[2][2].End);
        }


        [Test]
        public void testBuildPatternList()
        {
            PilePatternRecognizer r = new PilePatternRecognizer();
            MemoryPile<Signal, Pair> pile;

            SortedList<int, RelationItem> signals;
            List<Dictionary<long, Pair>> patternLayers;
            IPatternList pl;

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test1.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                patternLayers = r.FindPatterns(signals, pile);
                pl = r.BuildPatternList(patternLayers, signals.Count);

                Assert.AreEqual(3, pl.SignalCount);
                Assert.AreEqual(0, pl.Count);
            }

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test2.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                patternLayers = r.FindPatterns(signals, pile);
                pl = r.BuildPatternList(patternLayers, signals.Count);

                Assert.AreEqual(11, pl.SignalCount);
                Assert.AreEqual(2, pl.Count);

                Assert.AreEqual(3, pl[0].Count);
                Assert.AreEqual(2, pl[0].Size);
                Assert.AreEqual(0, pl[0][0].Start);
                Assert.AreEqual(3, pl[0][1].Start);
                Assert.AreEqual(9, pl[0][2].Start);

                Assert.AreEqual(2, pl[1].Count);
                Assert.AreEqual(2, pl[1].Size);
                Assert.AreEqual(1, pl[1][0].Start);
                Assert.AreEqual(6, pl[1][1].Start);
            }

            using (TextFileAdapter file = new TextFileAdapter(@"..\..\test3.txt"))
            {
                pile = new MemoryPile<Signal, Pair>();
                signals = r.ConvertSignalsToTerminalValues(file, pile);
                patternLayers = r.FindPatterns(signals, pile);
                pl = r.BuildPatternList(patternLayers, signals.Count);

                Assert.AreEqual(12, pl.SignalCount);
                Assert.AreEqual(3, pl.Count);

                Assert.AreEqual(3, pl[0].Count);
                Assert.AreEqual(2, pl[0].Size);
                Assert.AreEqual(0, pl[0][0].Start);
                Assert.AreEqual(3, pl[0][1].Start);
                Assert.AreEqual(9, pl[0][2].Start);

                Assert.AreEqual(3, pl[1].Count);
                Assert.AreEqual(2, pl[1].Size);
                Assert.AreEqual(1, pl[1][0].Start);
                Assert.AreEqual(6, pl[1][1].Start);
                Assert.AreEqual(10, pl[1][2].Start);

                Assert.AreEqual(2, pl[2].Count);
                Assert.AreEqual(3, pl[2].Size);
                Assert.AreEqual(0, pl[2][0].Start);
                Assert.AreEqual(9, pl[2][1].Start);
            }
        }
    
    }
}
