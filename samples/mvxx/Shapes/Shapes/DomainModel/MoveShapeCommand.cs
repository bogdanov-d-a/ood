using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class MoveShapeCommand : AbstractCommand
    {
        private readonly Canvas canvas;
        private readonly int index;
        private Common.Rectangle rect;

        public MoveShapeCommand(Canvas canvas, int index, Common.Rectangle rect)
        {
            this.canvas = canvas;
            this.index = index;
            this.rect = rect;
        }

        private void SwapRectangles()
        {
            Common.Rectangle oldRect = canvas.GetShape(index).GetBoundingRect();
            canvas.ResetShapeRectangle(index, rect);
            rect = oldRect;
        }

        protected override void ExecuteImpl()
        {
            SwapRectangles();
        }

        protected override void UnexecuteImpl()
        {
            SwapRectangles();
        }
    }
}
