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
        public WinMain()
        {
            InitializeComponent();

            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory;

            ralfw.Microkernel.DynamicBinder.LoadBindings();
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

            using (IFileAdapter fa = new dnppv.textfileadapter.RawTextFileAdapter(filename))
            {
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