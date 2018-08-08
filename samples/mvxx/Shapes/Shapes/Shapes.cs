using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shapes
{
    public partial class Shapes : Form
    {
        private readonly Presenter presenter;

        public Shapes(Presenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
            this.presenter.LayoutUpdatedEvent += new Presenter.LayoutUpdatedDelegate(this.OnLayoutUpdated);
        }

        private void addRectangleButton_Click(object sender, EventArgs e)
        {
            presenter.AddRectangle();
        }

        private void Shapes_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int i = 0; i < presenter.RectangleCount; ++i)
            {
                Common.Rectangle rect = presenter.GetRectangle(i);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)),
                    new Rectangle(rect.leftTop.x, rect.leftTop.y, rect.size.width, rect.size.height));
                if (presenter.IsRectangleSelected(i))
                {
                    g.DrawRectangle(new Pen(new SolidBrush(Color.Red)),
                        new Rectangle(rect.leftTop.x - 2, rect.leftTop.y - 2, rect.size.width + 4, rect.size.height + 4));
                }
            }
        }

        private void OnLayoutUpdated()
        {
            Invalidate();
        }
    }
}
