using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;

namespace dnppv.arcdiagram
{
    internal class ArcDiagramDesigner : ControlDesigner
    {
        private const int WM_PAINT = 0xF;

        private ArcDiagram ctlDiagram;


        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            ctlDiagram = component as ArcDiagram;
        }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
                this.DrawDummy(this.ctlDiagram.CreateGraphics());
        }


        private void DrawDummy(Graphics g)
        {
            g.FillRectangle(Brushes.White, 0, 0, this.ctlDiagram.Width, this.ctlDiagram.Height);

            const int lineWidth = 10;
            int yBaseLine = this.ctlDiagram.Height;
            this.ctlDiagram.DrawArc(g, 0, this.ctlDiagram.Width / 2 - lineWidth, lineWidth, yBaseLine);
            this.ctlDiagram.DrawArc(g, this.ctlDiagram.Width / 3, this.ctlDiagram.Width - lineWidth, lineWidth, yBaseLine);
        }
    }
}
