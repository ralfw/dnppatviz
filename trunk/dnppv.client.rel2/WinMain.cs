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

namespace dnppv.client.rel2
{
    public partial class WinMain : Form
    {
        private IFileAdapterFactory faf;


        public WinMain()
        {
            InitializeComponent();

            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory;

            ralfw.Microkernel.DynamicBinder.LoadBindings();
            this.faf = ralfw.Microkernel.DynamicBinder.GetInstance<IFileAdapterFactory>();
        }


        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void analysierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileFilter = "";
            foreach (string supportedExtension in this.faf.FileExtensionsSupported)
            {
                if (fileFilter.Length > 0) fileFilter += "|";
                fileFilter += string.Format("*.{0}|*.{0}", supportedExtension);
            }
            this.openFileDialog1.Filter = fileFilter;

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                Analyse(openFileDialog1.FileName);
        }


        private void Analyse(string filename)
        {
            IPatternList pl;

            using (IFileAdapter fa = faf.CreateFileAdapter(filename))
            {
                fa.Open(filename);

                IPatternFilter pf;
                pf = new dnppv.patternfilter.PatternFilter();

                pl = pf.Analyse(fa);

                this.Text = string.Format("Mustervisualisierung [{0}]", System.IO.Path.GetFileName(filename));
                this.toolStripStatusLabel1.Text = string.Format("{0} Muster in {1} Signalen gefunden", pl.Count, pl.SignalCount);

                this.arcDiagram1.Patterns = pl;
            }
        }
    }
}