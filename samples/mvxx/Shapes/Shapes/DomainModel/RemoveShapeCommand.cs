using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.DomainModel
{
    public class RemoveShapeCommand : AbstractCommand
    {
        public interface IShape
        {
            Common.ShapeType GetShapeType();
            Common.Rectangle GetBoundingRect();
        }

        public interface ICanvas
        {
            IShape GetAt(int index);
            void Insert(int index, Common.ShapeType type, Common.Rectangle rect);
            void RemoveAt(int index);
        }

        private struct ShapeInfo
        {
            public Common.ShapeType type;
            public Common.Rectangle rect;

            public ShapeInfo(Common.ShapeType type, Common.Rectangle rect)
            {
                this.type = type;
                this.rect = rect;
            }
        }

        private readonly ICanvas _canvas;
        private readonly int _index;
        private Option<ShapeInfo> _info;

        public RemoveShapeCommand(ICanvas canvas, int index)
        {
            _canvas = canvas;
            _index = index;
            _info = Option.None<ShapeInfo>();
        }

        protected override void ExecuteImpl()
        {
            var shape = _canvas.GetAt(_index);
            _info = Option.Some(new ShapeInfo(shape.GetShapeType(), shape.GetBoundingRect()));
            _canvas.RemoveAt(_index);
        }

        protected override void UnexecuteImpl()
        {
            _canvas.Insert(_index, _info.ValueOrFailure().type, _info.ValueOrFailure().rect);
            _info = Option.None<ShapeInfo>();
        }
    }
}
