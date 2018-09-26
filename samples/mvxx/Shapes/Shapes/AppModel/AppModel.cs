using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.AppModel
{
    public class AppModel
    {
        private class MovingData
        {
            public Common.Position startPos;
            public Common.Position curPos;
        }

        private static readonly Common.Rectangle defRect = new Common.Rectangle(new Common.Position(200, 100), new Common.Size(300, 200));

        private readonly DomainModel.Canvas canvas;
        private int selectedIndex = -1;
        private Option<MovingData> movingData = Option.None<MovingData>();

        public AppModel(DomainModel.Canvas canvas)
        {
            this.canvas = canvas;
        }

        private void AddShape(int type)
        {
            canvas.AddShape(type, defRect);
            LayoutUpdatedEvent();
        }

        public void AddRectangle()
        {
            AddShape(0);
        }

        public void AddTriangle()
        {
            AddShape(1);
        }

        public void AddCircle()
        {
            AddShape(2);
        }

        public ShapeTypes.IShape GetShape(int index)
        {
            var shape = canvas.GetShape(index);
            if (index == selectedIndex)
            {
                var offset = GetMoveOffset();
                if (offset.HasValue)
                {
                    var rect = shape.GetBoundingRect();
                    rect.Offset(offset.ValueOrFailure());
                    ClampBounds(ref rect);
                    shape = shape.Clone();
                    shape.SetBoundingRect(rect);
                }
            }
            return shape;
        }

        private void ClampBounds(ref Common.Rectangle rectangle)
        {
            rectangle.LeftTop.x = Math.Max(0, rectangle.LeftTop.x);
            rectangle.LeftTop.y = Math.Max(0, rectangle.LeftTop.y);

            int moveX = Math.Min(CanvasSize.width - 1, rectangle.RightBottom.x) - rectangle.RightBottom.x;
            rectangle.LeftTop.x += moveX;

            int moveY = Math.Min(CanvasSize.height - 1, rectangle.RightBottom.y) - rectangle.RightBottom.y;
            rectangle.LeftTop.y += moveY;
        }

        public int GetSelectedIndex()
        {
            return selectedIndex;
        }

        private void SelectShape(int index)
        {
            selectedIndex = index;
            LayoutUpdatedEvent();
        }

        private void SelectShapeAtPos(Common.Position pos)
        {
            for (int i = ShapeCount - 1; i >= 0; --i)
            {
                if (canvas.GetShape(i).GetBoundingRect().Contains(pos))
                {
                    SelectShape(i);
                    return;
                }
            }
        }

        public void RemoveSelectedShape()
        {
            if (selectedIndex != -1)
            {
                canvas.RemoveShape(selectedIndex);
                selectedIndex = -1;
                LayoutUpdatedEvent();
            }
        }

        public void BeginMove(Common.Position pos)
        {
            if (movingData.HasValue)
            {
                throw new Exception();
            }

            movingData = Option.Some(new MovingData());
            movingData.ValueOrFailure().startPos = pos;
            movingData.ValueOrFailure().curPos = pos;

            selectedIndex = -1;
            SelectShapeAtPos(pos);
        }

        public void Move(Common.Position pos)
        {
            if (!movingData.HasValue)
            {
                return;
            }
            movingData.ValueOrFailure().curPos = pos;
            LayoutUpdatedEvent();
        }

        private void ResetShapeRectangle(int index, Common.Rectangle rect)
        {
            canvas.ResetShapeRectangle(selectedIndex, rect);
            LayoutUpdatedEvent();
        }

        private Option<Common.Size> GetMoveOffset()
        {
            if (!movingData.HasValue)
            {
                return Option.None<Common.Size>();
            }
            return Option.Some(Common.Position.Sub(movingData.ValueOrFailure().curPos, movingData.ValueOrFailure().startPos));
        }

        public void EndMove(Common.Position pos)
        {
            Move(pos);
            Common.Size offset = GetMoveOffset().ValueOrFailure();

            if (selectedIndex != -1)
            {
                Common.Rectangle rect = canvas.GetShape(selectedIndex).GetBoundingRect();
                rect.Offset(offset);
                ClampBounds(ref rect);
                ResetShapeRectangle(selectedIndex, rect);
            }

            movingData = Option.None<MovingData>();
            LayoutUpdatedEvent();
        }

        public int ShapeCount
        {
            get {
                return canvas.ShapeCount;
            }
        }

        public Common.Size CanvasSize
        {
            get {
                return canvas.CanvasSize;
            }
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
