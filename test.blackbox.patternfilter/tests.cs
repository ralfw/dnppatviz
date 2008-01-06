using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using dnppv.patternfilter;
using dnppv.textfileadapter;
using dnppv.contracts.patternrecognizer;

namespace test.blackbox.patternfilter
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void testAnalyse()
        {
            ralfw.Microkernel.DynamicBinder.LoadBindings();

            string text;
            using(System.IO.StreamReader sr = new System.IO.StreamReader(@"..\..\test.txt", Encoding.Default))
            {
                text = sr.ReadToEnd();
            }

            using(TextFileAdapter tfa = new TextFileAdapter(@"..\..\test.txt"))
            {
                PatternFilter pf;
                pf = new PatternFilter();

                IPatternList pl;
                pl = pf.Analyse(tfa);
                Assert.AreEqual(39, pl.Count);

                Console.WriteLine("# of patterns: {0}", pl.Count);
                foreach (IPattern p in pl)
                    Console.WriteLine("  <{0}>*{1}", text.Substring(p[0].Start, p.Size), p.Count);
            }
        }
    }
}
