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

        private static Rectangle OffsetDrawRect(Common.Rectangle rect)
        {
            return new Rectangle(rect.LeftTop.x + drawOffset,
                rect.LeftTop.y + drawOffset,
                rect.Size.width,
                rect.Size.height);
        }

        private class RenderTarget : ShapeTypes.IRenderTarget
        {
            private Graphics g;

            public RenderTarget(Graphics g)
            {
                this.g = g;
            }

            public void DrawRectangle(Common.Rectangle rect)
            {
                var rect2 = OffsetDrawRect(rect);
                g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);
            }

            public void DrawTriangle(Common.Rectangle rect)
            {
                var rect2 = OffsetDrawRect(rect);
                Point[] points = {
                            new Point((rect2.Left + rect2.Right) / 2, rect2.Top),
                            new Point(rect2.Left, rect2.Bottom),
                            new Point(rect2.Right, rect2.Bottom),
                        };
                g.FillPolygon(new SolidBrush(Color.Yellow), points);
                g.DrawPolygon(new Pen(new SolidBrush(Color.Black)), points);
            }

            public void DrawCircle(Common.Rectangle rect)
            {
                var rect2 = OffsetDrawRect(rect);
                g.FillEllipse(new SolidBrush(Color.Yellow), rect2);
                g.DrawEllipse(new Pen(new SolidBrush(Color.Black)), rect2);
            }
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

        private void addCircleButton_Click(object sender, EventArgs e)
        {
            AddCircleEvent();
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

            RenderTarget target = new RenderTarget(g);

            RequestShapes((ShapeTypes.AbstractShape shape, bool isSelected) => {
                shape.Draw(target);

                if (isSelected)
                {
                    Rectangle rect2 = OffsetDrawRect(shape.GetBoundingRect());
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
        public event VoidDelegate AddCircleEvent;
        public event VoidDelegate RemoveShapeEvent;

        public delegate void MouseDelegate(Common.Position pos);
        public event MouseDelegate MouseDownEvent;
        public event MouseDelegate MouseUpEvent;
        public event MouseDelegate MouseMoveEvent;

        public delegate void ShapeInfoDelegate(ShapeTypes.AbstractShape shape, bool isSelected);
        public delegate void ShapeEnumeratorDelegate(ShapeInfoDelegate infoDelegate);
        private ShapeEnumeratorDelegate RequestShapes;

        public void AssignRequestShapesHandler(ShapeEnumeratorDelegate handler)
        {
            RequestShapes = handler;
        }

        private void removeShapeButton_Click(object sender, EventArgs e)
        {
            RemoveShapeEvent();
        }
    }
}
