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
        private class SignallingShapeListImpl : Common.ISignallingList<RectangleI>
        {
            private readonly CanvasView _view;

            public SignallingShapeListImpl(CanvasView view)
            {
                _view = view;
            }

            public int Count => _view._shapeList.Count;

            public event Common.IndexValueDelegate<RectangleI> AfterInsertEvent
            {
                add => _view._afterInsertEvent += value;
                remove => _view._afterInsertEvent -= value;
            }

            public event Common.IndexValueDelegate<RectangleI> BeforeRemoveEvent
            {
                add => _view._beforeRemoveEvent += value;
                remove => _view._beforeRemoveEvent -= value;
            }

            public event Common.IndexTwoValuesDelegate<RectangleI> AfterSetEvent
            {
                add => _view._afterSetEvent += value;
                remove => _view._afterSetEvent -= value;
            }

            public RectangleI GetAt(int index)
            {
                return _view._shapeList[index];
            }

            public void Insert(int index, RectangleI item)
            {
                _view._shapeList.Insert(index, item);
                _view.InvalidateEvent();
                _view._afterInsertEvent(index, item);
            }

            public void RemoveAt(int index)
            {
                _view._beforeRemoveEvent(index, GetAt(index));
                _view._shapeList.RemoveAt(index);
                _view.InvalidateEvent();
            }

            public void SetAt(int index, RectangleI value)
            {
                RectangleI oldValue = GetAt(index);
                if (oldValue.Equals(value))
                {
                    return;
                }

                RectangleI clipValue = new Common.RectangleInt(value.Left, value.Top, value.Width, value.Height);

                clipValue.Left = Math.Max(clipValue.Left, 0);
                clipValue.Top = Math.Max(clipValue.Top, 0);

                int rightOutbound = Math.Max(clipValue.Right - _view.CanvasSize.width, 0);
                clipValue.Left -= rightOutbound;

                int bottomOutbound = Math.Max(clipValue.Bottom - _view.CanvasSize.height, 0);
                clipValue.Top -= bottomOutbound;

                if (oldValue.Equals(clipValue))
                {
                    return;
                }

                _view._shapeList[index] = clipValue;
                _view.InvalidateEvent();
                _view._afterSetEvent(index, oldValue, clipValue);
            }
        }

        public const int DrawOffset = 50;

        public readonly SignallingInt SelectedShapeIndex = new SignallingInt(-1);

        private readonly List<RectangleI> _shapeList = new List<RectangleI>();
        private readonly SignallingShapeListImpl _signallingShapeList;
        private Common.IndexValueDelegate<RectangleI> _afterInsertEvent = delegate { };
        private Common.IndexValueDelegate<RectangleI> _beforeRemoveEvent = delegate { };
        private Common.IndexTwoValuesDelegate<RectangleI> _afterSetEvent = delegate { };

        public Common.ISignallingList<RectangleI> ShapeList
        {
            get => _signallingShapeList;
        }

        private int redrawCounter = 0;

        public Common.Size<int> CanvasSize
        {
            get => new Common.Size<int>(700, 350);
        }

        public delegate void MovingDelegate(int index, RectangleI pos);
        public MovingDelegate OnFinishMovingEvent = delegate {};

        public CanvasView()
        {
            _signallingShapeList = new SignallingShapeListImpl(this);
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
