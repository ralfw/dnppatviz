namespace test.arcdiagram
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.arcDiagram1 = new dnppv.arcdiagram.ArcDiagram();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.diagrammtestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nullToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kleinerAlsCtlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.größerAlsCtlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // arcDiagram1
            // 
            this.arcDiagram1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.arcDiagram1.ArcLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.arcDiagram1.Location = new System.Drawing.Point(12, 36);
            this.arcDiagram1.Name = "arcDiagram1";
            this.arcDiagram1.Patterns = null;
            this.arcDiagram1.Size = new System.Drawing.Size(358, 175);
            this.arcDiagram1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diagrammtestsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(382, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // diagrammtestsToolStripMenuItem
            // 
            this.diagrammtestsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nullToolStripMenuItem,
            this.leerToolStripMenuItem,
            this.kleinerAlsCtlToolStripMenuItem,
            this.größerAlsCtlToolStripMenuItem});
            this.diagrammtestsToolStripMenuItem.Name = "diagrammtestsToolStripMenuItem";
            this.diagrammtestsToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.diagrammtestsToolStripMenuItem.Text = "Diagrammtests";
            // 
            // nullToolStripMenuItem
            // 
            this.nullToolStripMenuItem.Name = "nullToolStripMenuItem";
            this.nullToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.nullToolStripMenuItem.Text = "null";
            this.nullToolStripMenuItem.Click += new System.EventHandler(this.nullToolStripMenuItem_Click);
            // 
            // leerToolStripMenuItem
            // 
            this.leerToolStripMenuItem.Name = "leerToolStripMenuItem";
            this.leerToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.leerToolStripMenuItem.Text = "leer";
            this.leerToolStripMenuItem.Click += new System.EventHandler(this.leerToolStripMenuItem_Click);
            // 
            // kleinerAlsCtlToolStripMenuItem
            // 
            this.kleinerAlsCtlToolStripMenuItem.Name = "kleinerAlsCtlToolStripMenuItem";
            this.kleinerAlsCtlToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.kleinerAlsCtlToolStripMenuItem.Text = "kleiner als ctl";
            this.kleinerAlsCtlToolStripMenuItem.Click += new System.EventHandler(this.kleinerAlsCtlToolStripMenuItem_Click);
            // 
            // größerAlsCtlToolStripMenuItem
            // 
            this.größerAlsCtlToolStripMenuItem.Name = "größerAlsCtlToolStripMenuItem";
            this.größerAlsCtlToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.größerAlsCtlToolStripMenuItem.Text = "größer als ctl";
            this.größerAlsCtlToolStripMenuItem.Click += new System.EventHandler(this.größerAlsCtlToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 223);
            this.Controls.Add(this.arcDiagram1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Test";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private dnppv.arcdiagram.ArcDiagram arcDiagram1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem diagrammtestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nullToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kleinerAlsCtlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem größerAlsCtlToolStripMenuItem;
    }
}

