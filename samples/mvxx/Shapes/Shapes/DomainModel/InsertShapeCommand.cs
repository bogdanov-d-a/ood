using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class InsertShapeCommand : AbstractCommand
    {
        public interface ICanvas
        {
            void Add(Common.ShapeType type, Common.Rectangle rect);
            void ReAdd(Canvas.IShape shape);
            Canvas.IShape Get();
            void Remove();
        }

        private interface IShapeAdder
        {
            void Add(ICanvas canvas);
        }

        private class NewShapeAdder : IShapeAdder
        {
            private readonly Common.ShapeType _type;
            private readonly Common.Rectangle _rect;

            public NewShapeAdder(Common.ShapeType type, Common.Rectangle rect)
            {
                _type = type;
                _rect = rect;
            }

            void IShapeAdder.Add(ICanvas canvas)
            {
                canvas.Add(_type, _rect);
            }
        }

        private class ShapeReAdder : IShapeAdder
        {
            private readonly Canvas.IShape _shape;

            public ShapeReAdder(Canvas.IShape shape)
            {
                _shape = shape;
            }

            void IShapeAdder.Add(ICanvas canvas)
            {
                canvas.ReAdd(_shape);
            }
        }

        private readonly ICanvas _canvas;
        private IShapeAdder _shapeAdder;

        public InsertShapeCommand(ICanvas canvas, Common.ShapeType type, Common.Rectangle rect)
        {
            _canvas = canvas;
            _shapeAdder = new NewShapeAdder(type, rect);
        }

        protected override void ExecuteImpl()
        {
            _shapeAdder.Add(_canvas);
            _shapeAdder = null;
        }

        protected override void UnexecuteImpl()
        {
            _shapeAdder = new ShapeReAdder(_canvas.Get());
            _canvas.Remove();
        }
    }
}
