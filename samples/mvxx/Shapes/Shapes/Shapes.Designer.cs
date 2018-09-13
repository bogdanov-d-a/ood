namespace Shapes
{
    partial class Shapes
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
            this.addRectangleButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.addTriangleButton = new System.Windows.Forms.ToolStripButton();
            this.removeShapeButton = new System.Windows.Forms.ToolStripButton();
            this.addCircleButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // addRectangleButton
            // 
            this.addRectangleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addRectangleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addRectangleButton.Name = "addRectangleButton";
            this.addRectangleButton.Size = new System.Drawing.Size(85, 22);
            this.addRectangleButton.Text = "Add rectangle";
            this.addRectangleButton.Click += new System.EventHandler(this.addRectangleButton_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRectangleButton,
            this.addTriangleButton,
            this.addCircleButton,
            this.removeShapeButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(734, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // addTriangleButton
            // 
            this.addTriangleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addTriangleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addTriangleButton.Name = "addTriangleButton";
            this.addTriangleButton.Size = new System.Drawing.Size(76, 22);
            this.addTriangleButton.Text = "Add triangle";
            this.addTriangleButton.Click += new System.EventHandler(this.addTriangleButton_Click);
            // 
            // removeShapeButton
            // 
            this.removeShapeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.removeShapeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeShapeButton.Name = "removeShapeButton";
            this.removeShapeButton.Size = new System.Drawing.Size(88, 22);
            this.removeShapeButton.Text = "Remove shape";
            this.removeShapeButton.Click += new System.EventHandler(this.removeShapeButton_Click);
            // 
            // addCircleButton
            // 
            this.addCircleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addCircleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addCircleButton.Name = "addCircleButton";
            this.addCircleButton.Size = new System.Drawing.Size(64, 22);
            this.addCircleButton.Text = "Add circle";
            this.addCircleButton.Click += new System.EventHandler(this.addCircleButton_Click);
            // 
            // Shapes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 561);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Shapes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shapes";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Shapes_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Shapes_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Shapes_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Shapes_MouseUp);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton addRectangleButton;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton removeShapeButton;
        private System.Windows.Forms.ToolStripButton addTriangleButton;
        private System.Windows.Forms.ToolStripButton addCircleButton;
    }
}

