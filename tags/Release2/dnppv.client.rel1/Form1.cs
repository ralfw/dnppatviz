using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using dnppv.contracts.fileadapter;
using dnppv.contracts.patternfilter;
using dnppv.contracts.patternrecognizer;

namespace dnppv.client.rel1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory;

            ralfw.Microkernel.DynamicBinder.LoadBindings();
        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                this.columnHeader4.Width = this.listView1.Width - (this.columnHeader1.Width + this.columnHeader2.Width + this.columnHeader3.Width) - 22;
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void analysierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                Analyse(openFileDialog1.FileName);
        }


        private void Analyse(string filename)
        {
            IPatternList pl;

            // pattern extractor needs to match the textfileadapter!!! see below
            //using (IFileAdapter fa = new dnppv.textfileadapter.NonWhitespaceTextFileAdapter(filename))
            using (IFileAdapter fa = new dnppv.textfileadapter.RawTextFileAdapter(filename))
            {
                IPatternFilter pf;
                pf = new dnppv.patternfilter.PatternFilter();

                pl = pf.Analyse(fa);
            }


            PatternTextExtractor pte = new PatternTextExtractor(filename);

            this.listView1.Items.Clear();
            for (int i = pl.Count - 1; i >= 0; i--)
            {
                IPattern p = pl[i];
                ListViewItem li = this.listView1.Items.Add((i+1).ToString());
                li.SubItems.Add(p.Size.ToString());
                li.SubItems.Add(p.Count.ToString());
                li.SubItems.Add(pte.Extract(p));
            }
        }
    }


    internal class PatternTextExtractor
    {
        private string text;

        public PatternTextExtractor(string filename)
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename, Encoding.Default))
            {
                text = sr.ReadToEnd();

                // remove whitespace - only if NonWhitespaceTextfileAdapter is used!
                //text = text.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
            }
        }

        public string Extract(IPattern pattern)
        {
            string patternText = text.Substring(pattern[0].Start, pattern.Size);

            StringBuilder sb = new StringBuilder();
            foreach (char c in patternText)
                sb.Append(char.IsControl(c) ? '.' : c);
            return sb.ToString();
        }
    }
}