using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

using dnppv.contracts.patternrecognizer;

namespace dnppv.arcdiagram
{
    [Designer(typeof(ArcDiagramDesigner))]
    public partial class ArcDiagram : UserControl
    {
        private IPatternList patterns;
        private Color arcLineColor;

        private TraceSource ts = new TraceSource("ArcDiagram", SourceLevels.Off);


        public ArcDiagram()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.ArcLineColor = Color.Blue;
        }


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


        public Color ArcLineColor
        {
            get
            {
                return this.arcLineColor;
            }

            set
            {
                this.arcLineColor = Color.FromArgb(40, value);
                #region trace
                this.ts.TraceEvent(TraceEventType.Verbose, 10, "Set ArcLineColor: {0}", value);
                #endregion

                this.Invalidate();
            }
        }


        private void ArcDiagram_Paint(object sender, PaintEventArgs e)
        {
            DrawDiagram(e.Graphics);
        }


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

                    foreach (IPattern p in this.patterns)
                    {
                        foreach (IPatternOccurrence po in p)
                        {
                            #region trace
                            this.ts.TraceEvent(TraceEventType.Verbose, 27, "pattern: start={0}, end={1}, size={2}", po.Start, po.End, p.Size);
                            #endregion
                            this.DrawArc(g, po.Start*displayRatio, po.End*displayRatio, p.Size*displayRatio, this.Height);
                        }
                    }
                }
                else
                    g.DrawString("No patterns found!", this.Font, Brushes.Black, 5, this.Height - this.Font.Height- 5);
            #endregion
        }


        internal void DrawArc(Graphics g, float xFrom, float xTo, float lineWidth, float yBaseLine)
        {
            #region trace
            this.ts.TraceEvent(TraceEventType.Information, 30, "DrawArc: xFrom={0}, xTo={1}, lineWidth={2}, yBaseLine={3}",
                                xFrom, xTo, lineWidth, yBaseLine);
            #endregion
            float dR = xTo + lineWidth - xFrom;
            float dC = dR - 2 * lineWidth / 2;

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new Rectangle((int)xFrom, (int)(yBaseLine - dR / 2), (int)dR, (int)(dR / 2)));
            g.Clip = new Region(path);

            Pen p = new Pen(this.arcLineColor, lineWidth);
            int adjust = lineWidth % 2 == 0 ? 1 : 0;
            float wArc = dC + adjust;
            float hArc = dC + adjust;

            #region trace
            this.ts.TraceEvent(TraceEventType.Verbose, 35, "DrawEllipse: x={0}, y={1}, w={2}, h={3}",
                               xFrom + lineWidth / 2 - adjust, yBaseLine - dC / 2, wArc, hArc);
            #endregion
            g.DrawEllipse(p, xFrom + lineWidth / 2 - adjust, yBaseLine - dC / 2, wArc, hArc);

            p.Dispose();
            g.Clip.Dispose();
            g.ResetClip();
            path.Dispose();
        }        
    }
}