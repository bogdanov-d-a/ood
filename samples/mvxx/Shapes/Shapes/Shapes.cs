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
        public interface IRenderTarget
        {
            void DrawRectangle(Common.Rectangle rect);
            void DrawTriangle(Common.Rectangle rect);
            void DrawCircle(Common.Rectangle rect);
        }

        public interface IDrawable
        {
            void Draw(IRenderTarget target);
        }

        private const int DrawOffset = 50;
        private Option<Common.Size> _canvasSizeOption;

        private static Rectangle OffsetDrawRect(Common.Rectangle rect)
        {
            return new Rectangle(rect.Left + DrawOffset,
                rect.Top + DrawOffset,
                rect.Width,
                rect.Height);
        }

        private class RenderTarget : IRenderTarget
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

        public Shapes(RequestPaintingDelegate requestPainting)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _requestPainting = requestPainting;
        }

        public void SetCanvasSize(Common.Size size)
        {
            _canvasSizeOption = Option.Some(size);
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
            const int SelOffset = 5;

            if (!_canvasSizeOption.HasValue)
            {
                return;
            }
            Common.Size canvasSize = _canvasSizeOption.ValueOrFailure();

            g.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(DrawOffset,
                    DrawOffset,
                    canvasSize.width,
                    canvasSize.height));

            RenderTarget target = new RenderTarget(g);

            _requestPainting((IDrawable drawable) => {
                drawable.Draw(target);
            },
            (Common.Rectangle rect) => {
                Rectangle rect2 = OffsetDrawRect(rect);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)),
                    new Rectangle(
                        rect2.X - SelOffset,
                        rect2.Y - SelOffset,
                        rect2.Width + 2 * SelOffset,
                        rect2.Height + 2 * SelOffset));
            });
        }

        public void OnLayoutUpdated()
        {
            Invalidate();
        }

        private Common.Position GetMousePosition()
        {
            Point rawPos = PointToClient(MousePosition);
            return new Common.Position(rawPos.X - DrawOffset, rawPos.Y - DrawOffset);
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
        public event VoidDelegate UndoEvent;
        public event VoidDelegate RedoEvent;

        public delegate void MouseDelegate(Common.Position pos);
        public event MouseDelegate MouseDownEvent;
        public event MouseDelegate MouseUpEvent;
        public event MouseDelegate MouseMoveEvent;

        public delegate void DrawableDelegate(IDrawable drawable);
        public delegate void SelectionDelegate(Common.Rectangle rect);
        public delegate void RequestPaintingDelegate(DrawableDelegate drawableDelegate, SelectionDelegate selectionDelegate);
        private readonly RequestPaintingDelegate _requestPainting;

        private void removeShapeButton_Click(object sender, EventArgs e)
        {
            RemoveShapeEvent();
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            UndoEvent();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            RedoEvent();
        }
    }
}
