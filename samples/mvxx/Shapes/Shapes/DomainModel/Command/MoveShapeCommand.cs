using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel.Command
{
    public class MoveShapeCommand : AbstractCommand
    {
        public interface IMovable
        {
            Common.Rectangle GetRect();
            void SetRect(Common.Rectangle rect);
        }

        private readonly IMovable _movable;
        private Common.Rectangle _rect;

        public MoveShapeCommand(IMovable movable, Common.Rectangle rect)
        {
            _movable = movable;
            _rect = rect;
        }

        private void SwapRectangles()
        {
            Common.Rectangle oldRect = _movable.GetRect();
            _movable.SetRect(_rect);
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
