using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public abstract class AbstractShape : IRenderShape
    {
        private Common.Rectangle boundingRect;

        public Common.Rectangle GetBoundingRect()
        {
            return boundingRect;
        }

        public void SetBoundingRect(Common.Rectangle rect)
        {
            boundingRect = rect;
        }

        public abstract void Draw(IRenderTarget target);
        public abstract bool HasPointInside(Common.Position pos);
        public abstract IShape Clone();

        public AbstractShape(Common.Rectangle boundingRect)
        {
            this.boundingRect = boundingRect;
        }
    }
}
