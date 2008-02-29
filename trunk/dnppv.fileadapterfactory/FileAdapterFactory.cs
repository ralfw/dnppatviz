using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using dnppv.contracts.fileadapter;


namespace dnppv.fileadapterfactory
{
    public class FileAdapterFactory : IFileAdapterFactory
    {
        // mapping file extension to file adapter type
        private List<string> fileExtensionsSupported;


        public FileAdapterFactory()
        {
            LoadFileAdapterMappingsViaMicrokernel();
        }

        internal void LoadFileAdapterMappingsViaMicrokernel()
        {
            fileExtensionsSupported = new List<string>(ralfw.Unity.ContainerProvider.Get().GetAllNames<IFileAdapter>());
        }


        #region IFileAdapterFactory Members
        public IFileAdapter CreateFileAdapter(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename).Substring(1).ToLower(); // get extension and trim leading "."
            if (this.fileExtensionsSupported.Exists(delegate(string extSupported) { return extSupported==ext; }))
            {
                IFileAdapter fa = ralfw.Unity.ContainerProvider.Get().Get<IFileAdapter>(ext);
                fa.Open(filename);
                return fa;
            }
            else
                return null;
        }


        public string[] FileExtensionsSupported
        {
            get 
            {
                return this.fileExtensionsSupported.ToArray();
            }
        }
        #endregion
    }
}
