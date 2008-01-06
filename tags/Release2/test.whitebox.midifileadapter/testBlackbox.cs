using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using dnppv.contracts.fileadapter;

namespace test.whitebox.midifileadapter
{
    [TestFixture]
    public class testBlackbox
    {
        [Test]
        public void testLoad()
        {
            using (IFileAdapter fa = new dnppv.midifileadapter.MidiFileAdapter())
            {
                fa.Open(@"..\..\test1 kling.mid");
                Assert.AreEqual(150, fa.Length);
            }
            using (IFileAdapter fa = new dnppv.midifileadapter.MidiFileAdapter())
            {
                fa.Open(@"..\..\test2 entchen.mid");
                Assert.AreEqual(144, fa.Length);
            }
            using (IFileAdapter fa = new dnppv.midifileadapter.MidiFileAdapter())
            {
                fa.Open(@"..\..\test3 bach.mid");
                Assert.AreEqual(1620, fa.Length);
            }
        }
    }
}
