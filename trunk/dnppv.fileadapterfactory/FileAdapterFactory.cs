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
            LoadFileAdapterMappingsFromConfig();
        }

        //<configuration>
        //  <fileAdapterFactory>
        //    <fileAdapter extension="txt" implementation="typename, assemblyname"/>
        //    <fileAdapter extension="mid" implementation="typename, assemblyname"/>
        //  </fileAdapterFactory>
        internal void LoadFileAdapterMappingsFromConfig()
        {
            try
            {
                Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConfigurationSection fafSection = appConfig.GetSection("fileAdapterFactory");

                XmlDocument xmlFafSection = new XmlDocument();
                xmlFafSection.LoadXml(fafSection.SectionInformation.GetRawXml());

                this.fileAdapterMappings = new Dictionary<string, Type>();
                foreach (XmlElement xmlFileAdapter in xmlFafSection.SelectNodes("fileAdapterFactory/fileAdapter"))
                    this.fileAdapterMappings.Add(
                        xmlFileAdapter.Attributes["extension"].Value.ToLower(),
                        Type.GetType(xmlFileAdapter.Attributes["implementation"].Value));                        
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Unable to load file adapter mappings from app.config!", ex);
            }
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
