using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel.Command
{
    class InsertShapeCommand : AbstractCommand
    {
        private readonly IShapes _shapes;
        private readonly Common.Shape _shape;
        private int _index;

        public InsertShapeCommand(IShapes shapes, Common.Shape shape)
        {
            _shapes = shapes;
            _shape = shape;
        }

        protected override void ExecuteImpl()
        {
            _index = _shapes.ShapeCount;
            _shapes.InsertShape(_index, _shape);
        }

        protected override void UnexecuteImpl()
        {
            _shapes.RemoveShapeAt(_index);
        }
    }
}
