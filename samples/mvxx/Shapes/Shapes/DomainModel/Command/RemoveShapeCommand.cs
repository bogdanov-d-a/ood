using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.DomainModel.Command
{
    class RemoveShapeCommand : AbstractCommand
    {
        private readonly IShapes _shapes;
        private readonly int _index;
        private Option<Common.Shape> _shape;

        public RemoveShapeCommand(IShapes shapes, int index)
        {
            _shapes = shapes;
            _index = index;
            _shape = Option.None<Common.Shape>();
        }

        protected override void ExecuteImpl()
        {
            _shape = Option.Some(_shapes.RemoveShapeAt(_index));
        }

        protected override void UnexecuteImpl()
        {
            _shapes.InsertShape(_index, _shape.ValueOrFailure());
            _shape = Option.None<Common.Shape>();
        }
    }
}
