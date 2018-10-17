using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public class Rectangle : AbstractShape
    {
        public Rectangle(OnMoveShape onMoveShape, Common.Rectangle boundingRect)
            : base(onMoveShape, boundingRect)
        {
        }

        public override void Draw(IRenderTarget target)
        {
            target.DrawRectangle(GetBoundingRect());
        }

        public override bool HasPointInside(Common.Position pos)
        {
            return GetBoundingRect().Contains(pos);
        }

        public override IShape Clone()
        {
            return new Rectangle(_onMoveShape, GetBoundingRect());
        }
    }
}
