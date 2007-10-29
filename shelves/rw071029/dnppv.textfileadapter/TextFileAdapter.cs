using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using dnppv.contracts.fileadapter;

namespace dnppv.textfileadapter
{
    public class TextFileAdapter : IFileAdapter
    {
        StreamReader sr;
        char[] buffer;


        #region IFileAdapter Members
 
        public void Open(string filename)
        {
            if (this.sr != null) this.sr.Close();

            this.sr = new StreamReader(filename, Encoding.Default);
            this.buffer = new char[1];  // a buffer len of 1 might be slow - but it�s simple to start with
        }
 
        public void Close()
        {
            if (this.sr != null)
            {
                this.sr.Close();
                this.sr = null;
            }
        }


        public bool Read()
        {
            if (this.sr == null)
                return false;
            else if (this.sr.EndOfStream)
                return false;
            else
                return this.sr.Read(this.buffer, 0, this.buffer.Length) > 0;
        }


        public string CurrentSignal
        {
            get
            {
                if (this.sr != null)
                    return this.buffer[0].ToString();
                else
                    throw new InvalidOperationException("No file open!");
            }
        }


        public int Length
        {
            get
            {
                return -1; // length of file is not necessarily equal to number of chars in file
            }
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
