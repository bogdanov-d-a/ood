using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class InsertShapeCommand : AbstractCommand
    {
        private readonly Canvas canvas;
        private readonly ShapeTypes.Type type;
        private readonly Common.Rectangle rect;

        public InsertShapeCommand(Canvas canvas, ShapeTypes.Type type, Common.Rectangle rect)
        {
            this.canvas = canvas;
            this.type = type;
            this.rect = rect;
        }

        public override void ExecuteImpl()
        {
            canvas.AddShape(type, rect);
        }

        public override void UnexecuteImpl()
        {
            canvas.RemoveShape(canvas.ShapeCount - 1);
        }
    }
}
