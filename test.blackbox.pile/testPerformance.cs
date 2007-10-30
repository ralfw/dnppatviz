using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using dnppv.pile;

namespace test.blackbox.pile
{
    [TestFixture]
    public class testPerformance
    {
        [Test]
        public void testTextSample()
        {
            MemoryPile<TerminalValueBase, InnerRelationBase> pile = new MemoryPile<TerminalValueBase,InnerRelationBase>();

            string text;
            using(System.IO.StreamReader sr = new System.IO.StreamReader(@"..\..\textsample.txt", Encoding.Default))
            {
                text = sr.ReadToEnd();
            }

            List<RelationBase> textAsTVs = new List<RelationBase>();
            foreach (char c in text)
                textAsTVs.Add(pile.Create(c.ToString()));
            Console.WriteLine("# of TVs: {0}, len of text: {1}", pile.CountTV, textAsTVs.Count);

            List<RelationBase> nextRelationLayer = new List<RelationBase>();
            for (int i = 1; i < textAsTVs.Count; i++)
                nextRelationLayer.Add(pile.Create(textAsTVs[i - 1], textAsTVs[i]));
            Console.WriteLine("# of inner rels: {0}", pile.CountInnerRelation);
        }
    }
}
