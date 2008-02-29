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
            p.occurrences.Add(new PatternOccurrence(0, 4));
            p.occurrences.Add(new PatternOccurrence(80, 84));
            pl.patterns.Add(p);

            p = new Pattern(2);
            p.occurrences.Add(new PatternOccurrence(20, 21));
            p.occurrences.Add(new PatternOccurrence(30, 31));
            p.occurrences.Add(new PatternOccurrence(40, 41));
            p.occurrences.Add(new PatternOccurrence(50, 51));
            pl.patterns.Add(p);

            this.arcDiagram1.Patterns = pl;
        }

        private void größerAlsCtlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatternList pl = new PatternList(1505);
            Pattern p;

            p = new Pattern(5);
            p.occurrences.Add(new PatternOccurrence(0, 4));
            p.occurrences.Add(new PatternOccurrence(1500, 1504));
            pl.patterns.Add(p);

            p = new Pattern(2);
            p.occurrences.Add(new PatternOccurrence(200, 201));
            p.occurrences.Add(new PatternOccurrence(300, 301));
            p.occurrences.Add(new PatternOccurrence(400, 401));
            p.occurrences.Add(new PatternOccurrence(500, 501));
            pl.patterns.Add(p);

            p = new Pattern(50);
            p.occurrences.Add(new PatternOccurrence(50, 99));
            p.occurrences.Add(new PatternOccurrence(1000, 1049));
            pl.patterns.Add(p);

            this.arcDiagram1.Patterns = pl;
        }

        private void ababaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatternList pl = new PatternList(5);
            Pattern p;

            p = new Pattern(2);
            p.occurrences.Add(new PatternOccurrence(0, 1));
            p.occurrences.Add(new PatternOccurrence(2, 3));
            pl.patterns.Add(p);

            p = new Pattern(2);
            p.occurrences.Add(new PatternOccurrence(1, 2));
            p.occurrences.Add(new PatternOccurrence(3, 4));
            pl.patterns.Add(p);

            p = new Pattern(3);
            p.occurrences.Add(new PatternOccurrence(0, 2));
            p.occurrences.Add(new PatternOccurrence(2, 4));
            pl.patterns.Add(p);

            this.arcDiagram1.Patterns = pl;
        }
    }
}