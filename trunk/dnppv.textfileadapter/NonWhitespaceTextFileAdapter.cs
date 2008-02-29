using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using dnppv.contracts.fileadapter;

namespace dnppv.textfileadapter
{
    public class NonWhitespaceTextFileAdapter : IFileAdapter
    {
        string filename;
        string buffer;
        int index;


        public NonWhitespaceTextFileAdapter(string filename)
        {
            this.Open(filename);
        }

        [Microsoft.Practices.Unity.InjectionConstructor] // muss als letzter ctor notiert sein, sonst fehler bei instanzierung mit unity
        public NonWhitespaceTextFileAdapter() { }



        public string Text
        {
            get
            {
                return this.buffer;
            }
        }


        #region IFileAdapter Members

        public void Open(string filename)
        {
            this.buffer = System.IO.File.ReadAllText(filename, System.Text.Encoding.Default);
            this.buffer = this.buffer.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
            index = -1;
        }

        public void Close()
        {
            this.buffer = null;
        }


        public bool Read()
        {
            if (this.buffer == null)
                return false;
            else if (this.index < this.buffer.Length - 1)
            {
                this.index++;
                return true;
            }
            else
                return false;
        }


        public string CurrentSignal
        {
            get
            {
                if (this.buffer != null)
                    return this.buffer[this.index].ToString();
                else
                    throw new InvalidOperationException("No file open!");
            }
        }


        public int Length
        {
            get
            {
                return this.buffer.Length;
            }
        }


        public string Filename
        {
            get { return this.filename; }
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
