using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public abstract class AbstractShape : IRenderShape
    {
        public delegate void OnMoveShape(IShape shape, Common.Rectangle rect);

        protected OnMoveShape _onMoveShape;
        private Common.Rectangle _boundingRect;

        public Common.Rectangle GetBoundingRect()
        {
            return _boundingRect;
        }

        public void SetBoundingRectDirect(Common.Rectangle rect)
        {
            _boundingRect = rect;
        }

        public void SetBoundingRect(Common.Rectangle rect)
        {
            _onMoveShape(this, rect);
        }

        public abstract void Draw(IRenderTarget target);
        public abstract bool HasPointInside(Common.Position pos);
        public abstract IShape Clone();

        public AbstractShape(OnMoveShape onMoveShape, Common.Rectangle boundingRect)
        {
            _onMoveShape = onMoveShape;
            _boundingRect = boundingRect;
        }
    }
}
