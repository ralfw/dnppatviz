using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using dnppv.pile;

namespace test.blackbox.pile
{
    [TestFixture]
    public class testStringDemo
    {
        [Test]
        public void testAssimilate()
        {
            StringDemo sd = new StringDemo();
            RelationBase t = sd.Assimilate("hello, world!");
            Assert.AreSame(t, sd.Assimilate("hello, world!"));
            Assert.AreNotSame(t, sd.Assimilate("good bye!"));
        }


        [Test]
        public void testGenerate()
        {
            StringDemo sd = new StringDemo();
            RelationBase thw = sd.Assimilate("hello, world!");
            RelationBase tgb = sd.Assimilate("good bye!");
            RelationBase tjb = sd.Assimilate("the world is not enough");

            Assert.AreEqual("hello, world!", sd.GenerateWithRecursion(thw));
            Assert.AreEqual("good bye!", sd.GenerateWithRecursion(tgb));
            Assert.AreEqual("the world is not enough", sd.GenerateWithRecursion(tjb));
        }
    }
}
