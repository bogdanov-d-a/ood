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
            void Remove();
        }

        private readonly ICanvas _canvas;
        private readonly Common.ShapeType _type;
        private readonly Common.Rectangle _rect;

        public InsertShapeCommand(ICanvas canvas, Common.ShapeType type, Common.Rectangle rect)
        {
            _canvas = canvas;
            _type = type;
            _rect = rect;
        }

        protected override void ExecuteImpl()
        {
            _canvas.Add(_type, _rect);
        }

        protected override void UnexecuteImpl()
        {
            _canvas.Remove();
        }
    }
}
