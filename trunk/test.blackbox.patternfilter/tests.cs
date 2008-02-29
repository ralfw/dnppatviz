using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using dnppv.patternfilter;
using dnppv.textfileadapter;

using dnppv.contracts.fileadapter;
using dnppv.contracts.patternfilter;
using dnppv.contracts.patternrecognizer;
using dnppv.contracts.domainmodel;

namespace test.blackbox.patternfilter
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void testAnalyse()
        {
            ralfw.Unity.ContainerProvider.Clear();
            ralfw.Unity.ContainerProvider.Configure();

            string text;
            using(System.IO.StreamReader sr = new System.IO.StreamReader(@"..\..\test.txt", Encoding.Default))
            {
                text = sr.ReadToEnd();
            }

            using(RawTextFileAdapter tfa = new RawTextFileAdapter(@"..\..\test.txt"))
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


        [Test]
        public void testCreateWithUnity()
        {
            ralfw.Unity.ContainerProvider.Clear();
            ralfw.Unity.ContainerProvider.Configure().Register<IFileAdapter, RawTextFileAdapter>();

            using (IFileAdapter tfa = ralfw.Unity.ContainerProvider.Get().Get<IFileAdapter>())
            {
                tfa.Open(@"..\..\test.txt");

                IPatternFilter pf;
                pf = ralfw.Unity.ContainerProvider.Get().Get<IPatternFilter>();

                IPatternList pl;
                pl = pf.Analyse(tfa);
                Assert.AreEqual(39, pl.Count);
            }
        }
    }
}
