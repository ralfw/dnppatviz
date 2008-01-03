using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using System.Xml;

using System.Diagnostics;

namespace spike.fileadapterfactory.config
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConfigurationSection fafSection = appConfig.GetSection("fileAdapterFactory");
                
                XmlDocument xmlFafSection = new XmlDocument();
                xmlFafSection.LoadXml(fafSection.SectionInformation.GetRawXml());

                foreach (XmlElement xmlFileAdapter in xmlFafSection.SelectNodes("fileAdapterFactory/fileAdapter"))
                {
                    string ext = xmlFileAdapter.Attributes["extension"].Value;
                    Type implType = Type.GetType(xmlFileAdapter.Attributes["implementation"].Value);
                    Console.WriteLine(ext + "=" + implType.FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " /// " + ex.GetType().Name);
            }
        }
    }

    public class MyClass1
    {
    }

    public class MyClass2
    {
    }
}
