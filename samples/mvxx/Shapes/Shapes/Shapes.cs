using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Optional;
using Optional.Unsafe;

namespace Shapes
{
    public partial class Shapes : Form
    {
        private const int drawOffset = 50;

        private readonly AppModel.AppModel presenter;

        private Option<Common.Position> dragMousePos = Option.None<Common.Position>();
        private int dragShapeIndex = -1;

        public Shapes(AppModel.AppModel presenter)
        {
            InitializeComponent();
            DoubleBuffered = true;

            this.presenter = presenter;
            this.presenter.LayoutUpdatedEvent += new AppModel.AppModel.LayoutUpdatedDelegate(this.OnLayoutUpdated);
        }

        private void addRectangleButton_Click(object sender, EventArgs e)
        {
            presenter.AddRectangle();
        }

        private void Shapes_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            const int selOffset = 5;

            g.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(drawOffset,
                    drawOffset,
                    presenter.CanvasSize.width,
                    presenter.CanvasSize.height));

            for (int i = 0; i < presenter.RectangleCount; ++i)
            {
                Common.Rectangle rect = presenter.GetRectangle(i);
                Rectangle rect2 = new Rectangle(rect.leftTop.x + drawOffset,
                    rect.leftTop.y + drawOffset,
                    rect.size.width,
                    rect.size.height);

                g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);

                if (presenter.IsRectangleSelected(i))
                {
                    g.DrawRectangle(new Pen(new SolidBrush(Color.Red)),
                        new Rectangle(
                            rect2.X - selOffset,
                            rect2.Y - selOffset,
                            rect2.Width + 2 * selOffset,
                            rect2.Height + 2 * selOffset));
                }
            }
        }

        private void OnLayoutUpdated()
        {
            Invalidate();
        }

        private void ResetSelection()
        {
            for (int i = 0; i < presenter.RectangleCount; ++i)
            {
                presenter.SelectRectangle(i, false);
            }
        }

        private Common.Position GetMousePosition()
        {
            Point rawPos = PointToClient(MousePosition);
            return new Common.Position(rawPos.X - drawOffset, rawPos.Y - drawOffset);
        }

        private void Shapes_Click(object sender, EventArgs e)
        {
            ResetSelection();
            for (int i = presenter.RectangleCount - 1; i >= 0; --i)
            {
                if (presenter.GetRectangle(i).Contains(GetMousePosition()))
                {
                    presenter.SelectRectangle(i, true);
                    return;
                }
            }
        }

        private void Shapes_MouseDown(object sender, MouseEventArgs e)
        {
            dragMousePos = Option.Some(GetMousePosition());
            dragShapeIndex = presenter.GetSelectedIndex();
        }

        private void Shapes_MouseUp(object sender, MouseEventArgs e)
        {
            dragMousePos = Option.None<Common.Position>();
            dragShapeIndex = -1;
        }

        private void Shapes_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragMousePos.HasValue && dragShapeIndex >= 0)
            {
                Common.Position newMousePos = GetMousePosition();
                Common.Size offset = Common.Position.Sub(dragMousePos.ValueOrFailure(), newMousePos);

                Common.Rectangle rect = presenter.GetRectangle(dragShapeIndex);
                rect.Offset(offset);

                if (presenter.CheckBounds(rect))
                {
                    presenter.ResetRectangle(dragShapeIndex, rect);
                    dragMousePos = Option.Some(newMousePos);
                }
            }
        }
    }
}
