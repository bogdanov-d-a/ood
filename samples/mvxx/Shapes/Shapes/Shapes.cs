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

            const int offset = 50;
            const int selOffset = 5;

            g.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(offset, offset, presenter.CanvasSize.width, presenter.CanvasSize.height));

            for (int i = 0; i < presenter.RectangleCount; ++i)
            {
                Common.Rectangle rect = presenter.GetRectangle(i);
                Rectangle rect2 = new Rectangle(rect.leftTop.x, rect.leftTop.y, rect.size.width, rect.size.height);

                g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);

                if (presenter.IsRectangleSelected(i))
                {
                    g.DrawRectangle(new Pen(new SolidBrush(Color.Red)),
                        new Rectangle(
                            rect.leftTop.x - selOffset,
                            rect.leftTop.y - selOffset,
                            rect.size.width + 2 * selOffset,
                            rect.size.height + 2 * selOffset));
                }
            }
        }

        private void OnLayoutUpdated()
        {
            Invalidate();
        }
    }
}
