namespace ShapesLite
{
    partial class ShapesLiteForm
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
            this.infoTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.addShapeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoTextBox
            // 
            this.infoTextBox.Location = new System.Drawing.Point(12, 418);
            this.infoTextBox.Name = "infoTextBox";
            this.infoTextBox.Size = new System.Drawing.Size(776, 20);
            this.infoTextBox.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addShapeToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // addShapeToolStripMenuItem
            // 
            this.addShapeToolStripMenuItem.Name = "addShapeToolStripMenuItem";
            this.addShapeToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.addShapeToolStripMenuItem.Text = "Add shape";
            this.addShapeToolStripMenuItem.Click += new System.EventHandler(this.addShapeToolStripMenuItem_Click);
            // 
            // ShapesLiteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.infoTextBox);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ShapesLiteForm";
            this.Text = "ShapesLiteForm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShapesLiteForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ShapesLiteForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShapesLiteForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ShapesLiteForm_MouseUp);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox infoTextBox;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem addShapeToolStripMenuItem;
    }
}

