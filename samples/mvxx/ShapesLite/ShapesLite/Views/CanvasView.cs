using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ShapesLite.Views
{
    using RectangleSignallingValue = Common.SignallingValue<Common.Rectangle<int>>;
    using BoolSignallingValue = Common.SignallingValue<bool>;

    public class CanvasView
    {
        public const int DrawOffset = 50;

        public readonly RectangleSignallingValue ShapeBoundingRect =
            new RectangleSignallingValue(new Common.RectangleInt(0, 0, 0, 0));

        public Common.Size<int> CanvasSize
        {
            get => new Common.Size<int>(700, 350);
        }

        public readonly BoolSignallingValue IsShapeSelected = new BoolSignallingValue(false);

        public delegate void OnFinishMovingDelegate(Common.Rectangle<int> pos);
        public OnFinishMovingDelegate OnFinishMovingEvent = delegate {};

        public CanvasView()
        {
            ShapeBoundingRect.Event += (Common.Rectangle<int> pos) => InvalidateEvent();
            IsShapeSelected.Event += (bool selected) => InvalidateEvent();
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

            {
                var rect2 = OffsetDrawRect(ShapeBoundingRect.Value);
                g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);
            }

            if (IsShapeSelected.Value)
            {
                const int SelOffset = 5;
                var rect2 = OffsetDrawRect(ShapeBoundingRect.Value);
                rect2.Inflate(new Size(SelOffset, SelOffset));
                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), rect2);
            }
        }

        public delegate void VoidDelegate();
        public event VoidDelegate InvalidateEvent = delegate {};
    }
}
