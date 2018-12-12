using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes
{
    public class ShapeBoundsChecker
    {
        private static int Sqr(int a)
        {
            return a * a;
        }

        private ShapeBoundsChecker()
        {
        }

        public static bool IsInsideRectangle(Common.Rectangle boundingRect, Common.Position pos)
        {
            return boundingRect.Contains(pos);
        }

        public static bool IsInsideTriangle(Common.Rectangle boundingRect, Common.Position pos)
        {
            if (!boundingRect.Contains(pos))
            {
                return false;
            }

            Common.Position leftBottom = new Common.Position(boundingRect.Left, boundingRect.Bottom);
            Common.Position rightBottom = boundingRect.RightBottom;
            Common.Position top = new Common.Position((boundingRect.Left + boundingRect.Right) / 2, boundingRect.Top);

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

        public static bool IsInsideCircle(Common.Rectangle boundingRect, Common.Position pos)
        {
            Common.Position origin = new Common.Position(
                (boundingRect.Left + boundingRect.Right) / 2,
                (boundingRect.Top + boundingRect.Bottom) / 2);

            Common.Size radius = new Common.Size(
                boundingRect.Width / 2,
                boundingRect.Height / 2);

            if (radius.width == 0 || radius.height == 0)
            {
                return false;
            }

            return (1.0 * Sqr(pos.x - origin.x) / Sqr(radius.width)) +
                (1.0 * Sqr(pos.y - origin.y) / Sqr(radius.height)) < 1;
        }

        public static bool IsInsideShape(Common.Shape shape, Common.Position pos)
        {
            switch (shape.type)
            {
                case Common.ShapeType.Rectangle:
                    return IsInsideRectangle(shape.boundingRect, pos);
                case Common.ShapeType.Triangle:
                    return IsInsideTriangle(shape.boundingRect, pos);
                case Common.ShapeType.Circle:
                    return IsInsideCircle(shape.boundingRect, pos);
                default:
                    throw new Exception();
            }
        }
    }
}
