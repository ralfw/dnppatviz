using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace test.arcdiagram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void nullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.arcDiagram1.Patterns = null;
        }

        private void leerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.arcDiagram1.Patterns = new PatternList(10);
        }

        private void kleinerAlsCtlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatternList pl = new PatternList(100);
            Pattern p;

            p = new Pattern(5);
            p.occurrences.Add(new PatternOccurrence(0, 10));
            p.occurrences.Add(new PatternOccurrence(80, 95));
            pl.patterns.Add(p);

            p = new Pattern(2);
            p.occurrences.Add(new PatternOccurrence(20, 30));
            p.occurrences.Add(new PatternOccurrence(30, 40));
            p.occurrences.Add(new PatternOccurrence(40, 50));
            p.occurrences.Add(new PatternOccurrence(50, 60));
            pl.patterns.Add(p);

            this.arcDiagram1.Patterns = pl;
        }

        private void größerAlsCtlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatternList pl = new PatternList(3000);
            Pattern p;

            p = new Pattern(5);
            p.occurrences.Add(new PatternOccurrence(0, 10));
            p.occurrences.Add(new PatternOccurrence(1500, 2996));
            pl.patterns.Add(p);

            p = new Pattern(2);
            p.occurrences.Add(new PatternOccurrence(200, 300));
            p.occurrences.Add(new PatternOccurrence(300, 400));
            p.occurrences.Add(new PatternOccurrence(400, 500));
            p.occurrences.Add(new PatternOccurrence(500, 600));
            pl.patterns.Add(p);

            p = new Pattern(50);
            p.occurrences.Add(new PatternOccurrence(50, 500));
            p.occurrences.Add(new PatternOccurrence(1000, 1500));
            pl.patterns.Add(p);

            this.arcDiagram1.Patterns = pl;
        }
    }
}