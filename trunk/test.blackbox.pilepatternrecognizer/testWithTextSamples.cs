using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;

using NUnit.Framework;

using dnppv.contracts.patternrecognizer;
using dnppv.textfileadapter;

namespace test.blackbox.pilepatternrecognizer
{
    [TestFixture]
    public class testsWithTextSamples
    {
        [Test]
        public void test3Levels()
        {
            TestTextFile(@"..\..\test3levelsnooverlap.txt", int.MaxValue);
        }

        [Test]
        public void testLoriot()
        {
            TestTextFile(@"..\..\Text Samples\loriot.txt", int.MaxValue);
        }


        [Test]
        public void testMaxUndMoritz()
        {
            //TestTextFile(@"..\..\Text Samples\mumVorwort.txt", 5);
            TestTextFile(@"..\..\Text Samples\mum1+2.txt", 10);
        }

        //[Test]
        public void testHamlet()
        {
            TestTextFile(@"..\..\Text Samples\hamlet.txt", 10);
        }


        private void TestTextFile(string filename, int dumpLevel)
        {
            IPatternRecognizer r;
            r = new dnppv.pilepatternrecognizer.PilePatternRecognizer();

            IPatternList pl;
            using (TextFileAdapter fa = new TextFileAdapter(filename))
            {
                pl = r.DetectPatterns(fa);
                DumpPatternStats(pl);
                DumpPatterns(pl, filename, dumpLevel);
            }
        }


        private void DumpPatterns(IPatternList patternList, string filename, int dumpLevel)
        {
            if (dumpLevel > 0)
            {
                string text;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(filename, Encoding.Default))
                {
                    text = sr.ReadToEnd();
                }

                for (int i = 0; i<dumpLevel && i<patternList.Count; i++)
                {
                    int j = patternList.Count - i - 1;

                    IPattern pattern = patternList[j];
                    string patternText = text.Substring(pattern[0].Start, pattern.Size);

                    Console.WriteLine("pattern {0} [{1}..{2}={3}] {4}*<{5}>", 
                        j+1, 
                        pattern[0].Start, pattern[0].End, pattern.Size, 
                        pattern.Count, 
                        ToNonWhitespace(patternText));
                }
            }
        }


        private string ToNonWhitespace(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
                if (char.IsControl(c))
                    sb.Append(".");
                else
                    sb.Append(c);
            return sb.ToString();
        }


        private void DumpPatternStats(IPatternList patternList)
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("number of patterns: {0}", patternList.Count);

            Dictionary<int, int> sizeDistribution = new Dictionary<int, int>();
            int minSize = int.MaxValue;
            int maxSize = int.MinValue;
            foreach (IPattern p in patternList)
            {
                if (sizeDistribution.ContainsKey(p.Size))
                    sizeDistribution[p.Size] += 1;
                else
                    sizeDistribution.Add(p.Size, 1);

                if (minSize > p.Size) minSize = p.Size;
                if (maxSize < p.Size) maxSize = p.Size;
            }
            Console.WriteLine("number of different pattern sizes: {0}; min. size: {1}, max. size: {2}", sizeDistribution.Count, minSize, maxSize);
            foreach (int size in sizeDistribution.Keys)
                Console.WriteLine("  size {0}: {1} patterns", size, sizeDistribution[size]);

            int minOccur = int.MaxValue;
            int maxOccur = int.MinValue;
            foreach(IPattern p in patternList)
            {
                if (minOccur > p.Count) minOccur = p.Count;
                if (maxOccur < p.Count) maxOccur = p.Count;
            }
            Console.WriteLine("min. occur: {0}, max. occur: {1}", minOccur, maxOccur);
        }
    }
}
