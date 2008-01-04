using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;

using dnppv.contracts.fileadapter;


namespace dnppv.fileadapterfactory
{
    public class FileAdapterFactory : IFileAdapterFactory
    {
        // mapping file extension to file adapter type
        private Dictionary<string, Type> fileAdapterMappings;


        public FileAdapterFactory()
        {
            LoadFileAdapterMappingsViaMicrokernel();
        }

        internal void LoadFileAdapterMappingsViaMicrokernel()
        {
            string[] comments = ralfw.Microkernel.DynamicBinder.GetComments<IFileAdapter>();

            this.fileAdapterMappings = new Dictionary<string, Type>();
            for (int i = 0; i < comments.Length; i++)
                this.fileAdapterMappings.Add(
                    comments[i],
                    ralfw.Microkernel.DynamicBinder.GetInstance<IFileAdapter>(i).GetType());
        }

        #region IFileAdapterFactory Members
        public IFileAdapter CreateFileAdapter(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename).Substring(1).ToLower(); // get extension and trim leading "."
            if (this.fileAdapterMappings.ContainsKey(ext))
            {
                IFileAdapter fa = (IFileAdapter)Activator.CreateInstance(this.fileAdapterMappings[ext]);
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
                return new List<string>(this.fileAdapterMappings.Keys).ToArray();
            }
        }
        #endregion
    }
}
