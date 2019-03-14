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

    public class CanvasView
    {
        public const int DrawOffset = 50;

        public readonly List<RectangleI> ShapeList = new List<RectangleI>();
        public readonly SignallingInt SelectedShapeIndex = new SignallingInt(-1);

        public Common.Size<int> CanvasSize
        {
            get => new Common.Size<int>(700, 350);
        }

        public delegate void MovingDelegate(int index, RectangleI pos);
        public MovingDelegate OnMoveEvent = delegate {};
        public MovingDelegate OnFinishMovingEvent = delegate {};

        public CanvasView()
        {
            OnMoveEvent += (int index, RectangleI pos) => InvalidateEvent();
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

            foreach (RectangleI shape in ShapeList)
            {
                var rect2 = OffsetDrawRect(shape);
                g.FillRectangle(new SolidBrush(Color.Yellow), rect2);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rect2);
            }

            if (SelectedShapeIndex.Value != -1)
            {
                const int SelOffset = 5;
                var rect2 = OffsetDrawRect(ShapeList[SelectedShapeIndex.Value]);
                rect2.Inflate(new Size(SelOffset, SelOffset));
                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), rect2);
            }
        }

        public delegate void VoidDelegate();
        public VoidDelegate InvalidateEvent = delegate {};
    }
}
