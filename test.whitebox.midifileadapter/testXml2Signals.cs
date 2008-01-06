using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
    public class testXml2Signals
    {
        [Test]
        public void testConversion()
        {
            MockManager.Init();
            Mock aMock = MockManager.Mock<Midi2XmlConverter>(Constructor.NotMocked);
            XmlDocument xmlMid = new XmlDocument();
            xmlMid.Load(@"..\..\small.mid.xml");
            aMock.AlwaysReturn("Convert", xmlMid);

            MidiFileAdapter a = new MidiFileAdapter(@"..\..\test1-kling.mid");
            Assert.AreEqual(12, a.Length);
        }
    }
}
