using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

using dnppv.contracts.domainmodel;

namespace dnppv.arcdiagram
{
    [Designer(typeof(ArcDiagramDesigner))]
    public partial class ArcDiagram : UserControl
    {
        private IPatternList patterns;

        private Color arcLineColor;
        private int arcLineAlpha;
        
        private Color arcLineColorDraw;

        private TraceSource ts = new TraceSource("ArcDiagram", SourceLevels.Off);


        public ArcDiagram()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.arcLineAlpha = 50;
            this.ArcLineColor = Color.Blue;
        }


        #region public properties
        [Browsable(false)]
        public IPatternList Patterns
        {
            get
            {
                return this.patterns;
            }

            set
            {
                this.patterns = value;
                this.Invalidate();
            }
        }


        #region designer properties
        [Category("ArcDiagram")]
        public Color ArcLineColor
        {
            get
            {
                return this.arcLineColor;
            }

            set
            {
                this.arcLineColor = value;
                RecalcTransparentArcLineColor();
                #region trace
                this.ts.TraceEvent(TraceEventType.Verbose, 10, "Set ArcLineColor: {0}", value);
                #endregion
                this.Invalidate();
            }
        }


        [Category("ArcDiagram")]
        public int ArcLineAlpha
        {
            get { return this.arcLineAlpha; }
            set
            {
                this.arcLineAlpha = Math.Abs(value) % 256;
                RecalcTransparentArcLineColor();
                #region trace
                this.ts.TraceEvent(TraceEventType.Verbose, 15, "Set ArcLineAlpha: {0}", value);
                #endregion
                this.Invalidate();
            }
        }


        private void RecalcTransparentArcLineColor()
        {
            this.arcLineColorDraw = Color.FromArgb(this.arcLineAlpha, this.arcLineColor);
        }
        #endregion
        #endregion


        private void ArcDiagram_Paint(object sender, PaintEventArgs e)
        {
            DrawDiagram(e.Graphics);
        }


        #region draw diagram
        private void DrawDiagram(Graphics g)
        {
            g.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);

            #region draw arcs
            if (this.patterns != null)
                if (this.patterns.Count > 0)
                {
                    #region trace
                    this.ts.TraceEvent(TraceEventType.Information, 20, "---DrawDiagram---");
                    #endregion

                    float displayRatio = (float)this.Width / (float)this.patterns.SignalCount;
                    #region trace
                    this.ts.TraceEvent(TraceEventType.Information, 25, "displayRatio={0}, canvas width={1}, signal count={2}", displayRatio, this.Width, this.patterns.SignalCount);
                    #endregion

                    float yBaseLine = this.Height; // base line so wählen, dass die kreise unten abgeschnitten werden, damit halbkreisbögen entstehen
                    foreach (IPattern p in this.patterns)
                    {
                        IPatternOccurrence po = p[0];
                        for (int i = 1; i < p.Count; i++)
                        {
                            IPatternOccurrence poNext = p[i];

                            #region trace
                            this.ts.TraceEvent(TraceEventType.Verbose, 27, "pattern: {0}. {1} -> {2}, size={3}", i, po.Start, poNext.Start, p.Size);
                            #endregion
                            this.DrawArc(g, po.Start * displayRatio, poNext.Start * displayRatio, p.Size * displayRatio, yBaseLine);

                            po = poNext;
                        }
                    }
                }
                //else
                //    g.DrawString("No patterns found!", this.Font, Brushes.Black, 5, this.Height - this.Font.Height- 5);
            #endregion
        }


        internal void DrawArc(Graphics g, float xFrom, float xTo, float lineWidth, float yBaseLine)
        {
            #region trace
            this.ts.TraceEvent(TraceEventType.Information, 30, "DrawArc: xFrom={0}, xTo={1}, lineWidth={2}, yBaseLine={3}",
                                xFrom, xTo, lineWidth, yBaseLine);
            #endregion
            Pen p = new Pen(this.arcLineColorDraw, lineWidth); // stift mit transparenter farbe für den bogen

            float wArc = xTo - xFrom; // durchmesser bzw. weite des bogens
            float hArc = xTo - xFrom; // höhe = durchmesser, da halbkreise gezeichnet werden
            #region trace
            this.ts.TraceEvent(TraceEventType.Verbose, 35, "DrawEllipse: x={0}, y={1}, w={2}, h={3}",
                               xFrom + lineWidth / 2, yBaseLine - hArc / 2, wArc, hArc);
            #endregion
            g.DrawEllipse(p, xFrom + lineWidth / 2, yBaseLine - hArc / 2, wArc, hArc);
                // stift in der mitte des striches ansetzen, daher xFrom+lineWidth/2

            p.Dispose();
        }
        #endregion
    }
}