using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using dnppv.contracts.fileadapter;

namespace test.blackbox.textfileadapter
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void testOpenClose()
        {
            IFileAdapter fa = new dnppv.textfileadapter.TextFileAdapter();
            fa.Open("test1.txt");
            fa.Close();
            try
            {
                string s = fa.CurrentSignal;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            }

            using (fa = new dnppv.textfileadapter.TextFileAdapter())
            {
                fa.Open("test1.txt");
            }

            try
            {
                string s = fa.CurrentSignal;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            }
        }


        [Test]
        public void testReadCurrentSignal()
        {
            using (IFileAdapter fa = new dnppv.textfileadapter.TextFileAdapter())
            {
                fa.Open("test1.txt");

                Assert.IsTrue(fa.Read());
                Assert.AreEqual("a", fa.CurrentSignal);
                Assert.IsTrue(fa.Read());
                Assert.AreEqual("b", fa.CurrentSignal);
                Assert.IsTrue(fa.Read());
                Assert.AreEqual("ß", fa.CurrentSignal);
                Assert.IsFalse(fa.Read());
                Assert.AreEqual("ß", fa.CurrentSignal);
            }

            using (IFileAdapter fa = new dnppv.textfileadapter.TextFileAdapter())
            {
                fa.Open("testLeer.txt");

                Assert.IsFalse(fa.Read());
            }
        }
    }
}
