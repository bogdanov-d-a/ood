using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class MoveShapeCommand : AbstractCommand
    {
        private readonly Canvas _canvas;
        private readonly int _index;
        private Common.Rectangle _rect;

        public MoveShapeCommand(Canvas canvas, int index, Common.Rectangle rect)
        {
            _canvas = canvas;
            _index = index;
            _rect = rect;
        }

        private void SwapRectangles()
        {
            Common.Rectangle oldRect = _canvas.GetShape(_index).GetBoundingRect();
            _canvas.ResetShapeRectangle(_index, _rect);
            _rect = oldRect;
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
