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

        public void AddRectangle()
        {
            canvas.AddRectangle(defRect);
            LayoutUpdatedEvent();
        }

        public Common.Rectangle GetRectangle(int index)
        {
            var rect = canvas.GetRectangle(index);
            if (index == selectedIndex)
            {
                var offset = GetMoveOffset();
                if (offset.HasValue)
                {
                    rect.Offset(offset.ValueOrFailure());
                    ClampBounds(ref rect);
                }
            }
            return rect;
        }

        private bool CheckBounds(Common.Rectangle rectangle)
        {
            return canvas.CheckBounds(rectangle);
        }

        private void ClampBounds(ref Common.Rectangle rectangle)
        {
            canvas.ClampBounds(ref rectangle);
        }

        public int GetSelectedIndex()
        {
            return selectedIndex;
        }

        private void SelectRectangle(int index)
        {
            selectedIndex = index;
            LayoutUpdatedEvent();
        }

        private void SelectRectangleAtPos(Common.Position pos)
        {
            for (int i = RectangleCount - 1; i >= 0; --i)
            {
                if (canvas.GetRectangle(i).Contains(pos))
                {
                    SelectRectangle(i);
                    return;
                }
            }
        }

        public void RemoveSelectedShape()
        {
            if (selectedIndex != -1)
            {
                canvas.RemoveRectangle(selectedIndex);
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
            SelectRectangleAtPos(pos);
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

        private void ResetRectangle(int index, Common.Rectangle rect)
        {
            canvas.ResetRectangle(selectedIndex, rect);
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
                Common.Rectangle rect = canvas.GetRectangle(selectedIndex);
                rect.Offset(offset);
                ClampBounds(ref rect);
                ResetRectangle(selectedIndex, rect);
            }

            movingData = Option.None<MovingData>();
            LayoutUpdatedEvent();
        }

        public int RectangleCount
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
