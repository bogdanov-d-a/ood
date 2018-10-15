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

        protected override void ExecuteImpl()
        {
            shape = canvas.GetShape(index).Clone();
            canvas.RemoveShape(index);
        }

        protected override void UnexecuteImpl()
        {
            canvas.InsertShape(index, shape);
            shape = null;
        }
    }
}
