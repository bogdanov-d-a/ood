using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shapes.Common;

namespace Shapes.ShapeTypes
{
    public abstract class AbstractShape : IShape
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

        public abstract int GetTypeId();
        public abstract void Draw(IRenderTarget target);
        public abstract bool IsInside(Position pos);

        public AbstractShape(Common.Rectangle boundingRect)
        {
            this.boundingRect = boundingRect;
        }
    }
}
