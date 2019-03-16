using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ShapesLite.Views
{
    using RectangleI = Common.Rectangle<int>;
    using SignallingInt = Common.SignallingValue<int>;
    using ShapeListType = Common.SignallingList<Common.Rectangle<int>>;

    public class CanvasView
    {
        public const int DrawOffset = 50;

        public readonly ShapeListType ShapeList = new ShapeListType();
        public readonly SignallingInt SelectedShapeIndex = new SignallingInt(-1);

        public Common.Size<int> CanvasSize
        {
            get => new Common.Size<int>(700, 350);
        }

        public delegate void MovingDelegate(int index, RectangleI pos);
        public MovingDelegate OnFinishMovingEvent = delegate {};

        public CanvasView()
        {
            ShapeList.AfterSetEvent += (int index, RectangleI pos) => {
                pos.Left = Math.Max(pos.Left, 0);
                pos.Top = Math.Max(pos.Top, 0);

                int rightOutbound = Math.Max(pos.Right - CanvasSize.width, 0);
                pos.Left -= rightOutbound;

                int bottomOutbound = Math.Max(pos.Bottom - CanvasSize.height, 0);
                pos.Top -= bottomOutbound;

                ShapeList.SetAt(index, pos);
            };

            ShapeList.AfterInsertEvent += (int index, RectangleI pos) => InvalidateEvent();
            ShapeList.AfterSetEvent += (int index, RectangleI pos) => InvalidateEvent();
            ShapeList.BeforeRemoveEvent += (int index, RectangleI pos) => InvalidateEvent();
            SelectedShapeIndex.Event += (int index) => InvalidateEvent();
        }

        private static Rectangle OffsetDrawRect(Common.Rectangle<int> rect)
        {
            return new Rectangle(rect.Left + DrawOffset,
                rect.Top + DrawOffset,
                rect.Width,
                rect.Height);
        }

        public void Draw(Graphics g)
        {
            {
                var rect2 = OffsetDrawRect(new Common.RectangleInt(0, 0, CanvasSize.width, CanvasSize.height));
                g.FillRectangle(new SolidBrush(Color.White), rect2);
            }

            for (int i = 0; i < ShapeList.Count; ++i)
            {
                var rect2 = OffsetDrawRect(ShapeList.GetAt(i));
                g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);
            }

            if (SelectedShapeIndex.Value != -1)
            {
                const int SelOffset = 5;
                var rect2 = OffsetDrawRect(ShapeList.GetAt(SelectedShapeIndex.Value));
                rect2.Inflate(new Size(SelOffset, SelOffset));
                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), rect2);
            }
        }

        public delegate void VoidDelegate();
        public event VoidDelegate InvalidateEvent = delegate {};
    }
}
