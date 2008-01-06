using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using TypeMock;

using dnppv.midifileadapter;

//Notice: This test requires the utility TypeMock. A copy of the library needs to be placed in the
//        source lib folder. TypeMock can be downloaded at: http://www.typemock.com/
//        There is a free community edition available - but the test requires Enterprise features.
//        However, during the eval period the Enterprise features are available.
namespace test.whitebox.midifileadapter
{
    [TestFixture]
    public class testAdapter
    {
        [Test]
        public void testReading()
        {
            MockManager.Init();
            Mock aMock = MockManager.Mock<Xml2SignalConverter>(Constructor.NotMocked);
            aMock.AlwaysReturn("Convert",
                               new List<string>(new string[] { "a", "b", "c" }));

            MidiFileAdapter a = new MidiFileAdapter(@"..\..\test1 kling.mid");
            Assert.AreEqual(@"..\..\test1 kling.mid", a.Filename);
            Assert.AreEqual(3, a.Length);
            Assert.IsNull(a.CurrentSignal);

            Assert.IsTrue(a.Read());
            Assert.AreEqual("a", a.CurrentSignal);
            Assert.IsTrue(a.Read());
            Assert.AreEqual("b", a.CurrentSignal);
            Assert.IsTrue(a.Read());
            Assert.AreEqual("c", a.CurrentSignal);

            Assert.IsFalse(a.Read());
            Assert.AreEqual("c", a.CurrentSignal);

            a.Close();

            Assert.IsFalse(a.Read());
            Assert.IsNull(a.CurrentSignal);
        }
    }
}
