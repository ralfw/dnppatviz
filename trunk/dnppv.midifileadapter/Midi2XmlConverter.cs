using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;

namespace dnppv.midifileadapter
{
    internal class Midi2XmlConverter
    {
        public XmlDocument Convert(string midFilename)
        {
            Process p = new Process();
            p.StartInfo.FileName = "mid2xml.exe";
            p.StartInfo.Arguments = string.Format(@"-t 1 {0}", midFilename);
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            XmlDocument xmlMid = new XmlDocument();
            using (StreamReader sr = p.StandardOutput)
            {
                StringBuilder sbXmlWithoutDTDRef = new StringBuilder();
                // copy PI
                sbXmlWithoutDTDRef.AppendLine(sr.ReadLine());

                // skip DTD ref
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();

                // copy rest of file
                sbXmlWithoutDTDRef.Append(sr.ReadToEnd());

                // write to temp file and read from there
                // otherwise there are strange errors when loading the xml from the stringbuilder string
                File.WriteAllText("temp.xml", sbXmlWithoutDTDRef.ToString());
                xmlMid.Load("temp.xml");
                File.Delete("temp.xml");
            }
            return xmlMid;
        }
    }
}
