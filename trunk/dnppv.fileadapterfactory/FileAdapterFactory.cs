using System;
using System.Collections.Generic;
using System.Text;

using dnppv.contracts.fileadapter;

namespace dnppv.fileadapterfactory
{
    public class FileAdapterFactory : IFileAdapterFactory
    {
        public FileAdapterFactory()
        {
        }


        #region IFileAdapterFactory Members

        public IFileAdapter CreateFileAdapter(string filename)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dictionary<string, string> Filetypes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
