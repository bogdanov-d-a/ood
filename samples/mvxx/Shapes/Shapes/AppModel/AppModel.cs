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
            public Option<RectEdges> resize;
        }

        private static readonly Common.Rectangle defRect = new Common.Rectangle(new Common.Position(200, 100), new Common.Size(300, 200));

        private readonly DomainModel.Canvas canvas;
        private int selectedIndex = -1;
        private Option<MovingData> movingData = Option.None<MovingData>();

        public AppModel(DomainModel.Canvas canvas)
        {
            this.canvas = canvas;
        }

        private void AddShape(ShapeTypes.Type type)
        {
            canvas.AddShape(type, defRect);
            LayoutUpdatedEvent();
        }

        public void AddRectangle()
        {
            AddShape(ShapeTypes.Type.Rectangle);
        }

        public void AddTriangle()
        {
            AddShape(ShapeTypes.Type.Triangle);
        }

        public void AddCircle()
        {
            AddShape(ShapeTypes.Type.Circle);
        }

        public ShapeTypes.IShape GetShape(int index)
        {
            var shape = canvas.GetShape(index);
            if (index == selectedIndex)
            {
                var rect = GetTransformingRect();
                if (rect.HasValue)
                {
                    shape = shape.Clone();
                    shape.SetBoundingRect(rect.ValueOrFailure());
                }
            }
            return shape;
        }

        private void OffsetClampBounds(ref Common.Rectangle rectangle)
        {
            rectangle.LeftTop.x = Math.Max(0, rectangle.LeftTop.x);
            rectangle.LeftTop.y = Math.Max(0, rectangle.LeftTop.y);

            int moveX = Math.Min(CanvasSize.width - 1, rectangle.RightBottom.x) - rectangle.RightBottom.x;
            rectangle.LeftTop.x += moveX;

            int moveY = Math.Min(CanvasSize.height - 1, rectangle.RightBottom.y) - rectangle.RightBottom.y;
            rectangle.LeftTop.y += moveY;
        }

        private void ResizeClampBounds(ref Common.Rectangle rectangle)
        {
            var oldRightBottom = rectangle.RightBottom;

            rectangle.LeftTop.x = Math.Max(0, rectangle.LeftTop.x);
            rectangle.LeftTop.y = Math.Max(0, rectangle.LeftTop.y);

            rectangle.SetRight(Math.Min(CanvasSize.width - 1, oldRightBottom.x));
            rectangle.SetBottom(Math.Min(CanvasSize.height - 1, oldRightBottom.y));
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
                if (canvas.GetShape(i).HasPointInside(pos))
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

        private static bool DoesValueMatch(int target, int tolerance, int value)
        {
            return (value > target - tolerance) && (value < target + tolerance);
        }

        private enum RectEdgeHor
        {
            Top,
            Bottom,
            None,
        };

        private enum RectEdgeVert
        {
            Left,
            Right,
            None,
        };

        private struct RectEdges
        {
            public RectEdgeHor hor;
            public RectEdgeVert vert;
        };

        private static RectEdges FindRectEdges(Common.Rectangle rect, Common.Position point)
        {
            const int tolerance = 4;

            RectEdges result = new RectEdges();
            result.hor = RectEdgeHor.None;
            result.vert = RectEdgeVert.None;

            bool xMatchesLeft = DoesValueMatch(rect.LeftTop.x, tolerance, point.x);
            bool xMatchesRight = DoesValueMatch(rect.RightBottom.x, tolerance, point.x);
            bool xIntersects = (rect.LeftTop.x - tolerance < point.x
                && point.x < rect.RightBottom.x + tolerance);

            bool yMatchesTop = DoesValueMatch(rect.LeftTop.y, tolerance, point.y);
            bool yMatchesBottom = DoesValueMatch(rect.RightBottom.y, tolerance, point.y);
            bool yIntersects = (rect.LeftTop.y - tolerance < point.y
                && point.y < rect.RightBottom.y + tolerance);

            if (yIntersects)
            {
                result.vert = xMatchesLeft ? RectEdgeVert.Left :
                    xMatchesRight ? RectEdgeVert.Right :
                    RectEdgeVert.None;
            }

            if (xIntersects)
            {
                result.hor = yMatchesTop ? RectEdgeHor.Top :
                    yMatchesBottom ? RectEdgeHor.Bottom :
                    RectEdgeHor.None;
            }

            return result;
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
            movingData.ValueOrFailure().resize = Option.None<RectEdges>();

            if (selectedIndex != -1)
            {
                var rect = GetShape(selectedIndex).GetBoundingRect();
                var edges = FindRectEdges(rect, pos);

                if (edges.hor != RectEdgeHor.None || edges.vert != RectEdgeVert.None)
                {
                    movingData.ValueOrFailure().resize = Option.Some(edges);
                }
            }

            if (!movingData.ValueOrFailure().resize.HasValue)
            {
                selectedIndex = -1;
                SelectShapeAtPos(pos);
            }
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

        private Option<Common.Rectangle> GetTransformingRect()
        {
            if (!movingData.HasValue || selectedIndex == -1)
            {
                return Option.None<Common.Rectangle>();
            }

            var md = movingData.ValueOrFailure();
            var rect = canvas.GetShape(selectedIndex).GetBoundingRect();
            var offset = Common.Position.Sub(md.curPos, md.startPos);

            if (md.resize.HasValue)
            {
                var rs = md.resize.ValueOrFailure();
                const int minDimension = 10;

                if (rs.hor == RectEdgeHor.Top)
                {
                    var shift = offset.height > 0 ? Math.Min(rect.Size.height - minDimension, offset.height) : offset.height;
                    rect.Size.height -= shift;
                    rect.LeftTop.y += shift;
                }
                else if (rs.hor == RectEdgeHor.Bottom)
                {
                    var shift = offset.height < 0 ? Math.Max(-(rect.Size.height - minDimension), offset.height) : offset.height;
                    rect.Size.height += shift;
                }

                if (rs.vert == RectEdgeVert.Left)
                {
                    var shift = offset.width > 0 ? Math.Min(rect.Size.width - minDimension, offset.width) : offset.width;
                    rect.Size.width -= shift;
                    rect.LeftTop.x += shift;
                }
                else if (rs.vert == RectEdgeVert.Right)
                {
                    var shift = offset.width < 0 ? Math.Max(-(rect.Size.width - minDimension), offset.width) : offset.width;
                    rect.Size.width += shift;
                }

                ResizeClampBounds(ref rect);
            }
            else
            {
                rect.Offset(offset);
                OffsetClampBounds(ref rect);
            }

            return Option.Some(rect);
        }

        public void EndMove(Common.Position pos)
        {
            Move(pos);

            var rectOpt = GetTransformingRect();
            if (rectOpt.HasValue)
            {
                ResetShapeRectangle(selectedIndex, rectOpt.ValueOrFailure());
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
