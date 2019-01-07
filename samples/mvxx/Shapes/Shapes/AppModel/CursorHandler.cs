using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.AppModel
{
    class CursorHandler
    {
        public interface IShape
        {
            Common.Rectangle GetBoundingRect();
            void SetBoundingRect(Common.Rectangle rect);
            bool HasPointInside(Common.Position pos);
        }

        public interface IModel
        {
            int GetShapeCount();
            IShape GetShape(int index);

            void SelectShape(int index);
            int GetSelectionIndex();

            Common.Size GetCanvasSize();
            void OnShapeTransform();
        }

        private class MovingData
        {
            public Common.Position startPos;
            public Common.Position curPos;
            public Option<CursorHandlerUtils.RectEdges> resize;

            public MovingData(Common.Position startPos, Common.Position curPos, Option<CursorHandlerUtils.RectEdges> resize)
            {
                this.startPos = startPos;
                this.curPos = curPos;
                this.resize = resize;
            }
        }

        private readonly IModel _model;
        private Option<MovingData> _movingData = Option.None<MovingData>();

        public CursorHandler(IModel model)
        {
            _model = model;
        }

        private int GetShapeAtPos(Common.Position pos)
        {
            for (int i = _model.GetShapeCount() - 1; i >= 0; --i)
            {
                if (_model.GetShape(i).HasPointInside(pos))
                {
                    return i;
                }
            }
            return -1;
        }

        private void SelectShapeAtPos(Common.Position pos)
        {
            int i = GetShapeAtPos(pos);
            if (i != -1)
            {
                _model.SelectShape(i);
            }
        }

        public void BeginMove(Common.Position pos)
        {
            if (_movingData.HasValue)
            {
                throw new Exception();
            }

            var movingData = new MovingData(pos, pos, Option.None<CursorHandlerUtils.RectEdges>());
            _movingData = Option.Some(movingData);

            if (_model.GetSelectionIndex() != -1)
            {
                var rect = _model.GetShape(_model.GetSelectionIndex()).GetBoundingRect();
                var edges = CursorHandlerUtils.FindRectEdges(rect, pos);

                if (edges.hor != CursorHandlerUtils.RectEdgeHor.None || edges.vert != CursorHandlerUtils.RectEdgeVert.None)
                {
                    movingData.resize = Option.Some(edges);
                }
            }

            if (!movingData.resize.HasValue)
            {
                _model.SelectShape(-1);
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
            _model.OnShapeTransform();
        }

        public Option<Common.Rectangle> GetTransformingRect()
        {
            if (!_movingData.HasValue || _model.GetSelectionIndex() == -1)
            {
                return Option.None<Common.Rectangle>();
            }

            var md = _movingData.ValueOrFailure();
            var rect = _model.GetShape(_model.GetSelectionIndex()).GetBoundingRect();
            var offset = Common.Position.Sub(md.curPos, md.startPos);

            if (md.resize.HasValue)
            {
                var rs = md.resize.ValueOrFailure();
                const int MinDimension = 10;

                if (rs.hor == CursorHandlerUtils.RectEdgeHor.Top)
                {
                    var shift = offset.height > 0 ? Math.Min(rect.Height - MinDimension, offset.height) : offset.height;
                    rect.Height -= shift;
                    rect.Top += shift;
                }
                else if (rs.hor == CursorHandlerUtils.RectEdgeHor.Bottom)
                {
                    var shift = offset.height < 0 ? Math.Max(-(rect.Height - MinDimension), offset.height) : offset.height;
                    rect.Height += shift;
                }

                if (rs.vert == CursorHandlerUtils.RectEdgeVert.Left)
                {
                    var shift = offset.width > 0 ? Math.Min(rect.Width - MinDimension, offset.width) : offset.width;
                    rect.Width -= shift;
                    rect.Left += shift;
                }
                else if (rs.vert == CursorHandlerUtils.RectEdgeVert.Right)
                {
                    var shift = offset.width < 0 ? Math.Max(-(rect.Width - MinDimension), offset.width) : offset.width;
                    rect.Width += shift;
                }

                CursorHandlerUtils.ResizeClampBounds(ref rect, _model.GetCanvasSize());
            }
            else
            {
                rect.Offset(offset);
                CursorHandlerUtils.OffsetClampBounds(ref rect, _model.GetCanvasSize());
            }

            return Option.Some(rect);
        }

        public void EndMove(Common.Position pos)
        {
            Move(pos);

            var rectOpt = GetTransformingRect();
            if (rectOpt.HasValue)
            {
                _movingData = Option.None<MovingData>();
                _model.GetShape(_model.GetSelectionIndex()).SetBoundingRect(rectOpt.ValueOrFailure());
            }

            _movingData = Option.None<MovingData>();
        }
    }
}
