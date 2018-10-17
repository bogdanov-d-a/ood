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

        private readonly DomainModel.Document _document;
        private readonly ShapeTypes.IShapeFactory _shapeFactory;
        private int _selectedIndex = -1;
        private Option<MovingData> _movingData = Option.None<MovingData>();

        public AppModel(DomainModel.Document document, ShapeTypes.IShapeFactory shapeFactory)
        {
            _document = document;
            _document.LayoutUpdatedEvent += new DomainModel.Document.LayoutUpdatedDelegate(() => {
                LayoutUpdatedEvent();
            });
            _shapeFactory = shapeFactory;
        }

        private void AddShape(ShapeTypes.Type type)
        {
            _document.AddShape(_shapeFactory.CreateShape(type, defRect));
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
            var shape = _document.GetShape(index);
            if (index == _selectedIndex)
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
            rectangle.Left = Math.Max(0, rectangle.Left);
            rectangle.Top = Math.Max(0, rectangle.Top);

            int moveX = Math.Min(CanvasSize.width - 1, rectangle.Right) - rectangle.Right;
            rectangle.Left += moveX;

            int moveY = Math.Min(CanvasSize.height - 1, rectangle.Bottom) - rectangle.Bottom;
            rectangle.Top += moveY;
        }

        private void ResizeClampBounds(ref Common.Rectangle rectangle)
        {
            var oldRightBottom = rectangle.RightBottom;

            rectangle.Left = Math.Max(0, rectangle.Left);
            rectangle.Top = Math.Max(0, rectangle.Top);

            rectangle.Right = Math.Min(CanvasSize.width - 1, oldRightBottom.x);
            rectangle.Bottom = Math.Min(CanvasSize.height - 1, oldRightBottom.y);
        }

        public int GetSelectedIndex()
        {
            return _selectedIndex;
        }

        private void SelectShape(int index)
        {
            _selectedIndex = index;
            LayoutUpdatedEvent();
        }

        private void SelectShapeAtPos(Common.Position pos)
        {
            for (int i = ShapeCount - 1; i >= 0; --i)
            {
                if (_document.GetShape(i).HasPointInside(pos))
                {
                    SelectShape(i);
                    return;
                }
            }
        }

        public void RemoveSelectedShape()
        {
            if (_selectedIndex != -1)
            {
                _document.RemoveShape(_selectedIndex);
                _selectedIndex = -1;
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
            const int Tolerance = 4;

            RectEdges result = new RectEdges();
            result.hor = RectEdgeHor.None;
            result.vert = RectEdgeVert.None;

            bool xMatchesLeft = DoesValueMatch(rect.Left, Tolerance, point.x);
            bool xMatchesRight = DoesValueMatch(rect.Right, Tolerance, point.x);
            bool xIntersects = (rect.Left - Tolerance < point.x
                && point.x < rect.Right + Tolerance);

            bool yMatchesTop = DoesValueMatch(rect.Top, Tolerance, point.y);
            bool yMatchesBottom = DoesValueMatch(rect.Bottom, Tolerance, point.y);
            bool yIntersects = (rect.Top - Tolerance < point.y
                && point.y < rect.Bottom + Tolerance);

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
            if (_movingData.HasValue)
            {
                throw new Exception();
            }

            _movingData = Option.Some(new MovingData());
            _movingData.ValueOrFailure().startPos = pos;
            _movingData.ValueOrFailure().curPos = pos;
            _movingData.ValueOrFailure().resize = Option.None<RectEdges>();

            if (_selectedIndex != -1)
            {
                var rect = GetShape(_selectedIndex).GetBoundingRect();
                var edges = FindRectEdges(rect, pos);

                if (edges.hor != RectEdgeHor.None || edges.vert != RectEdgeVert.None)
                {
                    _movingData.ValueOrFailure().resize = Option.Some(edges);
                }
            }

            if (!_movingData.ValueOrFailure().resize.HasValue)
            {
                _selectedIndex = -1;
                SelectShapeAtPos(pos);
            }
        }

        public void Move(Common.Position pos)
        {
            if (!_movingData.HasValue)
            {
                return;
            }
            _movingData.ValueOrFailure().curPos = pos;
            LayoutUpdatedEvent();
        }

        private Option<Common.Rectangle> GetTransformingRect()
        {
            if (!_movingData.HasValue || _selectedIndex == -1)
            {
                return Option.None<Common.Rectangle>();
            }

            var md = _movingData.ValueOrFailure();
            var rect = _document.GetShape(_selectedIndex).GetBoundingRect();
            var offset = Common.Position.Sub(md.curPos, md.startPos);

            if (md.resize.HasValue)
            {
                var rs = md.resize.ValueOrFailure();
                const int MinDimension = 10;

                if (rs.hor == RectEdgeHor.Top)
                {
                    var shift = offset.height > 0 ? Math.Min(rect.Height - MinDimension, offset.height) : offset.height;
                    rect.Height -= shift;
                    rect.Top += shift;
                }
                else if (rs.hor == RectEdgeHor.Bottom)
                {
                    var shift = offset.height < 0 ? Math.Max(-(rect.Height - MinDimension), offset.height) : offset.height;
                    rect.Height += shift;
                }

                if (rs.vert == RectEdgeVert.Left)
                {
                    var shift = offset.width > 0 ? Math.Min(rect.Width - MinDimension, offset.width) : offset.width;
                    rect.Width -= shift;
                    rect.Left += shift;
                }
                else if (rs.vert == RectEdgeVert.Right)
                {
                    var shift = offset.width < 0 ? Math.Max(-(rect.Width - MinDimension), offset.width) : offset.width;
                    rect.Width += shift;
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
                _document.GetShape(_selectedIndex).SetBoundingRect(rectOpt.ValueOrFailure());
            }

            _movingData = Option.None<MovingData>();
            LayoutUpdatedEvent();
        }

        public void ResetSelection()
        {
            SelectShape(-1);
        }

        public int ShapeCount
        {
            get {
                return _document.ShapeCount;
            }
        }

        public Common.Size CanvasSize
        {
            get {
                return _document.CanvasSize;
            }
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
