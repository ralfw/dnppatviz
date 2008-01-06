using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace dnppv.midifileadapter
{
    internal class Xml2SignalConverter
    {
        public List<string> Convert(XmlDocument xmlMid)
        {
            List<string> signals = new List<string>();

            foreach (XmlElement noteOn in xmlMid.SelectNodes("MIDIFile/Track/Event/NoteOn"))
                signals.Add(noteOn.Attributes["Note"].Value);

            return signals;
        }
    }
}
