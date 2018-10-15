using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class InsertShapeCommand : AbstractCommand
    {
        private readonly Canvas _canvas;
        private readonly ShapeTypes.IShape _shape;

        public InsertShapeCommand(Canvas canvas, ShapeTypes.IShape shape)
        {
            _canvas = canvas;
            _shape = shape;
        }

        protected override void ExecuteImpl()
        {
            _canvas.AddShape(_shape);
        }

        protected override void UnexecuteImpl()
        {
            _canvas.RemoveShape(_canvas.ShapeCount - 1);
        }
    }
}
