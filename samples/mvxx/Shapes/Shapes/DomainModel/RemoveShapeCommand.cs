using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class RemoveShapeCommand : AbstractCommand
    {
        private readonly Canvas _canvas;
        private readonly int _index;
        private ShapeTypes.IShape _shape;

        public RemoveShapeCommand(Canvas canvas, int index)
        {
            _canvas = canvas;
            _index = index;
            _shape = null;
        }

        protected override void ExecuteImpl()
        {
            _shape = _canvas.GetShape(_index).Clone();
            _canvas.RemoveShape(_index);
        }

        protected override void UnexecuteImpl()
        {
            _canvas.InsertShape(_index, _shape);
            _shape = null;
        }
    }
}
