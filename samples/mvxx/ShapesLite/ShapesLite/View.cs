using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ShapesLite
{
    public class View
    {
        public const int DrawOffset = 50;

        public readonly Common.SignallingValue<Common.Rectangle<int>> Position =
            new Common.SignallingValue<Common.Rectangle<int>>(new Common.RectangleInt(0, 0, 0, 0));

        public Common.Size<int> CanvasSize
        {
            get => new Common.Size<int>(700, 350);
        }

        public readonly Common.SignallingValue<bool> IsSelected = new Common.SignallingValue<bool>(false);

        public delegate void OnFinishMovingDelegate(Common.Rectangle<int> pos);
        public OnFinishMovingDelegate OnFinishMovingEvent = delegate {};

        public View()
        {
            Position.Event += (Common.Rectangle<int> pos) => InvalidateEvent();
            IsSelected.Event += (bool selected) => InvalidateEvent();
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
                var rect2 = OffsetDrawRect(Position.Value);
                g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);
            }

            if (IsSelected.Value)
            {
                const int SelOffset = 5;
                var rect2 = OffsetDrawRect(Position.Value);
                rect2.Inflate(new Size(SelOffset, SelOffset));
                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), rect2);
            }
        }

        public delegate void VoidDelegate();
        public event VoidDelegate InvalidateEvent = delegate {};
    }
}
