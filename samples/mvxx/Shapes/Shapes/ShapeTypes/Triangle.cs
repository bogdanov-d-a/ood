using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public class Triangle : AbstractShape
    {
        public Triangle(OnMoveShape onMoveShape, Common.Rectangle boundingRect)
            : base(onMoveShape, boundingRect)
        {
        }

        public override void Draw(IRenderTarget target)
        {
            target.DrawTriangle(GetBoundingRect());
        }

        public override bool HasPointInside(Common.Position pos)
        {
            var rect = GetBoundingRect();

            if (!rect.Contains(pos))
            {
                return false;
            }

            Common.Position leftBottom = new Common.Position(rect.Left, rect.Bottom);
            Common.Position rightBottom = rect.RightBottom;
            Common.Position top = new Common.Position((rect.Left + rect.Right) / 2, rect.Top);

            if (leftBottom.x == rightBottom.x)
            {
                return false;
            }

            {
                double k = 1.0 * (top.y - leftBottom.y) / (top.x - leftBottom.x);
                double b = leftBottom.y - k * leftBottom.x;
                if (pos.y < k * pos.x + b)
                {
                    return false;
                }
            }

            {
                double k = 1.0 * (top.y - rightBottom.y) / (top.x - rightBottom.x);
                double b = rightBottom.y - k * rightBottom.x;
                if (pos.y < k * pos.x + b)
                {
                    return false;
                }
            }

            return true;
        }

        public override IShape Clone()
        {
            return new Triangle(_onMoveShape, GetBoundingRect());
        }
    }
}
