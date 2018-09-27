using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shapes.Common;

namespace Shapes.ShapeTypes
{
    public class Triangle : AbstractShape
    {
        public Triangle(Common.Rectangle boundingRect)
            : base(boundingRect)
        {
        }

        public override ShapeTypes.Type GetShapeType()
        {
            return ShapeTypes.Type.Triangle;
        }

        public override void Draw(IRenderTarget target)
        {
            target.DrawTriangle(GetBoundingRect());
        }

        public override bool HasPointInside(Position pos)
        {
            var rect = GetBoundingRect();

            if (!rect.Contains(pos))
            {
                return false;
            }

            Position leftBottom = new Position(rect.Left, rect.Bottom);
            Position rightBottom = rect.RightBottom;
            Position top = new Position((rect.Left + rect.Right) / 2, rect.Top);

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
            return new Triangle(GetBoundingRect());
        }
    }
}
