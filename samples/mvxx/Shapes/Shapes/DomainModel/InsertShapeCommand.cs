using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class InsertShapeCommand : AbstractCommand
    {
        private readonly Canvas _canvas;
        private readonly ShapeTypes.Type _type;
        private readonly Common.Rectangle _rect;

        public InsertShapeCommand(Canvas canvas, ShapeTypes.Type type, Common.Rectangle rect)
        {
            _canvas = canvas;
            _type = type;
            _rect = rect;
        }

        protected override void ExecuteImpl()
        {
            _canvas.AddShape(_type, _rect);
        }

        protected override void UnexecuteImpl()
        {
            _canvas.RemoveShape(_canvas.ShapeCount - 1);
        }
    }
}
