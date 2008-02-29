using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using dnppv.contracts.domainmodel;
using dnppv.contracts.patternrecognizer;
using dnppv.textfileadapter;

namespace test.blackbox.pilepatternrecognizer
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void testDetectPatterns1()
        {
            IPatternRecognizer r;
            r = new dnppv.pilepatternrecognizer.PilePatternRecognizer();

            IPatternList pl;
            using (RawTextFileAdapter fa = new RawTextFileAdapter(@"..\..\test3levelsnooverlap.txt"))
            {
                pl = r.DetectPatterns(fa);
                Assert.AreEqual(10, pl.Count);

                // ab
                Assert.AreEqual(2, pl[0].Count);
                Assert.AreEqual(2, pl[0].Size);
                Assert.AreEqual(0, pl[0][0].Start);
                Assert.AreEqual(12, pl[0][1].Start);

                // cd
                Assert.AreEqual(3, pl[1].Count);
                Assert.AreEqual(2, pl[1].Size);
                Assert.AreEqual(3, pl[1][0].Start);
                Assert.AreEqual(15, pl[1][1].Start);
                Assert.AreEqual(24, pl[1][2].Start);

                // hi
                Assert.AreEqual(4, pl[5].Count);
                Assert.AreEqual(2, pl[5].Size);
                Assert.AreEqual(9, pl[5][0].Start);
                Assert.AreEqual(21, pl[5][1].Start);
                Assert.AreEqual(30, pl[5][2].Start);
                Assert.AreEqual(35, pl[5][3].Start);

                // cde
                Assert.AreEqual(3, pl[6].Count);
                Assert.AreEqual(3, pl[6].Size);
                Assert.AreEqual(3, pl[6][0].Start);
                Assert.AreEqual(15, pl[6][1].Start);
                Assert.AreEqual(24, pl[6][2].Start);

                // fgh
                Assert.AreEqual(4, pl[7].Count);
                Assert.AreEqual(3, pl[7].Size);
                Assert.AreEqual(7, pl[7][0].Start);
                Assert.AreEqual(19, pl[7][1].Start);
                Assert.AreEqual(28, pl[7][2].Start);
                Assert.AreEqual(33, pl[7][3].Start);

                // fghi
                Assert.AreEqual(4, pl[9].Count);
                Assert.AreEqual(4, pl[9].Size);
                Assert.AreEqual(7, pl[9][0].Start);
                Assert.AreEqual(19, pl[9][1].Start);
                Assert.AreEqual(28, pl[9][2].Start);
                Assert.AreEqual(33, pl[9][3].Start);
            }
        }


        [Test]
        public void testDetectPatterns2()
        {
            IPatternRecognizer r;
            r = new dnppv.pilepatternrecognizer.PilePatternRecognizer();

            IPatternList pl;
            using (RawTextFileAdapter fa = new RawTextFileAdapter(@"..\..\test3levelswithoverlap.txt"))
            {
                pl = r.DetectPatterns(fa);
                Assert.AreEqual(11, pl.Count);

                // ab
                Assert.AreEqual(3, pl[0].Count);
                Assert.AreEqual(2, pl[0].Size);
                Assert.AreEqual(11, pl[0][2].Start);

                // ef
                Assert.AreEqual(2, pl[4].Count);
                Assert.AreEqual(2, pl[4].Size);
                Assert.AreEqual(18, pl[4][0].Start);

                // cde
                Assert.AreEqual(2, pl[7].Count);
                Assert.AreEqual(3, pl[7].Size);
                Assert.AreEqual(21, pl[7][1].Start);

                // abcd
                Assert.AreEqual(2, pl[9].Count);
                Assert.AreEqual(4, pl[9].Size);
                Assert.AreEqual(11, pl[9][1].Start);

                // cdef
                Assert.AreEqual(2, pl[10].Count);
                Assert.AreEqual(4, pl[10].Size);
                Assert.AreEqual(21, pl[10][1].Start);
            }
        }
    }
}
