using System;
using System.Collections.Generic;
using System.Text;

using dnppv.contracts.fileadapter;

namespace test.fileadapterfactory
{
    public class MockupFileAdapterB : IFileAdapter
    {
        private string filename;


        #region IFileAdapter Members

        public void Close()
        {
        }

        public string CurrentSignal
        {
            get
            {
                return "b";
            }
        }

        public string Filename
        {
            get { return this.Filename; }
        }

        public int Length
        {
            get { return this.filename.Length; }
        }

        public void Open(string filename)
        {
            this.filename = filename;
        }

        public bool Read()
        {
            return true;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
