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
    using ShapeListType = Common.ISignallingList<Common.Rectangle<int>>;

    public class CanvasView
    {
        public const int DrawOffset = 50;

        public readonly ShapeListType ShapeList = new Common.SignallingList<RectangleI>();
        public readonly SignallingInt SelectedShapeIndex = new SignallingInt(-1);

        private int redrawCounter = 0;

        public Common.Size<int> CanvasSize
        {
            get => new Common.Size<int>(700, 350);
        }

        public delegate void MovingDelegate(int index, RectangleI pos);
        public MovingDelegate OnFinishMovingEvent = delegate {};

        public CanvasView()
        {
            bool insideAfterSetEventHandler = false;
            ShapeList.AfterSetEvent += (int index, RectangleI oldPos, RectangleI pos) => {
                if (insideAfterSetEventHandler)
                {
                    return;
                }
                insideAfterSetEventHandler = true;

                try
                {
                    RectangleI clipPos = new Common.RectangleInt(pos.Left, pos.Top, pos.Width, pos.Height);

                    clipPos.Left = Math.Max(clipPos.Left, 0);
                    clipPos.Top = Math.Max(clipPos.Top, 0);

                    int rightOutbound = Math.Max(clipPos.Right - CanvasSize.width, 0);
                    clipPos.Left -= rightOutbound;

                    int bottomOutbound = Math.Max(clipPos.Bottom - CanvasSize.height, 0);
                    clipPos.Top -= bottomOutbound;

                    ShapeList.SetAt(index, clipPos);

                    if (!oldPos.Equals(clipPos))
                    {
                        InvalidateEvent();
                    }
                }
                finally
                {
                    insideAfterSetEventHandler = false;
                }
            };

            ShapeList.AfterInsertEvent += (int index, RectangleI pos) => InvalidateEvent();
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

            g.DrawString(redrawCounter.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), 10 + DrawOffset, 10 + DrawOffset);
            ++redrawCounter;
        }

        public delegate void VoidDelegate();
        public event VoidDelegate InvalidateEvent = delegate {};
    }
}
