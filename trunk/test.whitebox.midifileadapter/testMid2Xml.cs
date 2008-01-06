using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Xml;

using NUnit.Framework;

using dnppv.midifileadapter;

namespace test.whitebox.midifileadapter
{
    [TestFixture]
    public class testMid2Xml
    {
        [Test]
        public void testStartConverterInBackground()
        {
            Midi2XmlConverter c = new Midi2XmlConverter();
            XmlDocument xmlMid = c.Convert(@"..\..\test1 kling.mid");
            Assert.AreEqual("MIDIFile", xmlMid.DocumentElement.Name);
        }
    }
}
