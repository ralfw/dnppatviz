using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using dnppv.contracts.fileadapter;

namespace dnppv.midifileadapter
{
    public class MidiFileAdapter : IFileAdapter
    {
        private string filename;
        private List<string> signals;
        private int indexCurrSignal;


        public MidiFileAdapter() { }

        public MidiFileAdapter(string filename)
        {
            this.Open(filename);
        }


        #region IFileAdapter Members

        public void Open(string filename)
        {
            this.filename = filename;
            this.signals = this.ConvertMidi2Signals(filename);
            this.indexCurrSignal = -1;
        }

        protected virtual List<string> ConvertMidi2Signals(string filename)
        {
            Midi2XmlConverter midConv = new Midi2XmlConverter();
            Xml2SignalConverter xmlConv = new Xml2SignalConverter();
            return xmlConv.Convert(midConv.Convert(filename));
        }


        public void Close()
        {
            this.signals = null;
        }


        public bool Read()
        {
            if (this.signals != null)
            {
                if (this.indexCurrSignal < this.signals.Count - 1)
                {
                    this.indexCurrSignal++;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public string CurrentSignal
        {
            get
            {
                if (this.signals != null && this.indexCurrSignal >= 0)
                    return this.signals[this.indexCurrSignal];
                else
                    return null;
            }
        }


        public string Filename
        {
            get { return this.filename; }
        }

        public int Length
        {
            get { return this.signals != null ? this.signals.Count : 0; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Close();
        }

        #endregion
    }
}
