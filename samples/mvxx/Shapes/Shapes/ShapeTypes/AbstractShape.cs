using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public abstract class AbstractShape
    {
        private Common.Rectangle _boundingRect;

        public Common.Rectangle GetBoundingRect()
        {
            return _boundingRect;
        }

        public void SetBoundingRect(Common.Rectangle rect)
        {
            _boundingRect = rect;
        }

        public abstract Common.ShapeType GetShapeType();
        public abstract void Draw(Shapes.IRenderTarget target, Common.Rectangle rect);
        public abstract bool HasPointInside(Common.Position pos);

        public AbstractShape(Common.Rectangle boundingRect)
        {
            _boundingRect = boundingRect;
        }
    }
}
