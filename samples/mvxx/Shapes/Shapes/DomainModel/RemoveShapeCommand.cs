using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class RemoveShapeCommand : AbstractCommand
    {
        private readonly Canvas canvas;
        private readonly int index;
        private ShapeTypes.IShape shape;

        public RemoveShapeCommand(Canvas canvas, int index)
        {
            this.canvas = canvas;
            this.index = index;
            shape = null;
        }

        public override void ExecuteImpl()
        {
            shape = canvas.GetShape(index).Clone();
            canvas.RemoveShape(index);
        }

        public override void UnexecuteImpl()
        {
            // TODO: fix z-order
            canvas.AddShape(shape);
            shape = null;
        }
    }
}
