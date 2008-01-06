using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using dnppv.pile;

namespace test.blackbox.pile
{
    [TestFixture]
    class testsFürArtikel
    {
        [Test]
        public void testArtikel2()
        {
            MemoryPile<RootRelation, InnerRelation> p;
            p = new MemoryPile<RootRelation, InnerRelation>();

            Dictionary<string, RootRelation> rootRelations;
            rootRelations = new Dictionary<string, RootRelation>();

            p.Create("obelix");
            p.Create("idefix");

            p.Create(
                p.Get("obelix"),
                p.Get("idefix")
            );
        }


        [Test]
        public void testArtikel1()
        {
            MemoryPile<RootRelation, InnerRelation> p;
            p = new MemoryPile<RootRelation, InnerRelation>();

            RootRelation t1, t2;
            t1 = p.Create("t1");
            t2 = p.Create("t2");

            Assert.AreSame(
                p.Create(t1, t2),
                p.Create(t1, t2)
            );
        }
    }
}
