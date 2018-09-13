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
        private Option<Common.Size> canvasSizeOption;

        public enum ShapeType
        {
            Rectangle,
            Triangle,
        }

        public Shapes()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void SetCanvasSize(Common.Size size)
        {
            canvasSizeOption = Option.Some(size);
        }

        private void addRectangleButton_Click(object sender, EventArgs e)
        {
            AddRectangleEvent();
        }

        private void addTriangleButton_Click(object sender, EventArgs e)
        {
            AddTriangleEvent();
        }

        private void Shapes_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            const int selOffset = 5;

            if (!canvasSizeOption.HasValue)
            {
                return;
            }
            Common.Size canvasSize = canvasSizeOption.ValueOrFailure();

            g.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(drawOffset,
                    drawOffset,
                    canvasSize.width,
                    canvasSize.height));

            RequestRectangles((Common.Rectangle rect, ShapeType type, bool isSelected) => {
                Rectangle rect2 = new Rectangle(rect.LeftTop.x + drawOffset,
                    rect.LeftTop.y + drawOffset,
                    rect.Size.width,
                    rect.Size.height);

                switch (type)
                {
                    case ShapeType.Rectangle:
                        g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                        g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);
                        break;

                    case ShapeType.Triangle:
                        Point[] points = {
                            new Point((rect2.Left + rect2.Right) / 2, rect2.Top),
                            new Point(rect2.Left, rect2.Bottom),
                            new Point(rect2.Right, rect2.Bottom),
                        };
                        g.FillPolygon(new SolidBrush(Color.Yellow), points);
                        g.DrawPolygon(new Pen(new SolidBrush(Color.Black)), points);
                        break;
                }

                if (isSelected)
                {
                    g.DrawRectangle(new Pen(new SolidBrush(Color.Red)),
                        new Rectangle(
                            rect2.X - selOffset,
                            rect2.Y - selOffset,
                            rect2.Width + 2 * selOffset,
                            rect2.Height + 2 * selOffset));
                }
            });
        }

        public void OnLayoutUpdated()
        {
            Invalidate();
        }

        private Common.Position GetMousePosition()
        {
            Point rawPos = PointToClient(MousePosition);
            return new Common.Position(rawPos.X - drawOffset, rawPos.Y - drawOffset);
        }

        private void Shapes_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownEvent(GetMousePosition());
        }

        private void Shapes_MouseUp(object sender, MouseEventArgs e)
        {
            MouseUpEvent(GetMousePosition());
        }

        private void Shapes_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMoveEvent(GetMousePosition());
        }

        public delegate void VoidDelegate();
        public event VoidDelegate AddRectangleEvent;
        public event VoidDelegate AddTriangleEvent;
        public event VoidDelegate RemoveShapeEvent;

        public delegate void MouseDelegate(Common.Position pos);
        public event MouseDelegate MouseDownEvent;
        public event MouseDelegate MouseUpEvent;
        public event MouseDelegate MouseMoveEvent;

        public delegate void RectangleInfoDelegate(Common.Rectangle rect, ShapeType type, bool isSelected);
        public delegate void RectangleEnumeratorDelegate(RectangleInfoDelegate infoDelegate);
        private RectangleEnumeratorDelegate RequestRectangles;

        public void AssignRequestRectanglesHandler(RectangleEnumeratorDelegate handler)
        {
            RequestRectangles = handler;
        }

        private void removeShapeButton_Click(object sender, EventArgs e)
        {
            RemoveShapeEvent();
        }
    }
}
