using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public class Triangle : AbstractShape
    {
        public Triangle(Common.Rectangle boundingRect)
            : base(boundingRect)
        {
        }

        public override Common.ShapeType GetShapeType()
        {
            return Common.ShapeType.Triangle;
        }

        public override void Draw(Shapes.IRenderTarget target, Common.Rectangle rect)
        {
            target.DrawTriangle(rect);
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
    }
}
