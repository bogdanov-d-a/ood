using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shapes.Common;

namespace Shapes.ShapeTypes
{
    public class Rectangle : AbstractShape
    {
        public Rectangle(Common.Rectangle boundingRect)
            : base(boundingRect)
        {
        }

        public override ShapeTypes.Type GetShapeType()
        {
            return ShapeTypes.Type.Rectangle;
        }

        public override void Draw(IRenderTarget target)
        {
            target.DrawRectangle(GetBoundingRect());
        }

        public override bool HasPointInside(Position pos)
        {
            return GetBoundingRect().Contains(pos);
        }

        public override IShape Clone()
        {
            return new Rectangle(GetBoundingRect());
        }
    }
}
