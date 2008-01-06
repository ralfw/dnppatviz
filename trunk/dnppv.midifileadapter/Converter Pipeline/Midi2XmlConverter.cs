using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace dnppv.midifileadapter
{
    internal class Midi2XmlConverter
    {
        // source: http://www.c-sharpcorner.com/UploadFile/crajesh1981/RajeshPage103142006044841AM/RajeshPage1.aspx?ArticleID=63e02c1f-761f-44ab-90dd-8d2348b8c6d2
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
                 [MarshalAs(UnmanagedType.LPTStr)]
                 string path,
                 [MarshalAs(UnmanagedType.LPTStr)]
                 StringBuilder shortPath,
                 int shortPathLength
                 );


        public XmlDocument Convert(string midFilename)
        {
            #region convert long filename to short for mid2xml.exe
            StringBuilder bufferFilename = new StringBuilder(255);
            GetShortPathName(midFilename, bufferFilename, bufferFilename.Capacity);
            midFilename = bufferFilename.ToString();
            #endregion

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
