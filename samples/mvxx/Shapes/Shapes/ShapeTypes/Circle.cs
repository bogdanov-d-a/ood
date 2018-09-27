using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shapes.Common;

namespace Shapes.ShapeTypes
{
    public class Circle : AbstractShape
    {
        private static int Sqr(int a)
        {
            return a * a;
        }

        public Circle(Common.Rectangle boundingRect)
            : base(boundingRect)
        {
        }

        public override ShapeTypes.Type GetShapeType()
        {
            return ShapeTypes.Type.Circle;
        }

        public override void Draw(IRenderTarget target)
        {
            target.DrawCircle(GetBoundingRect());
        }

        public override bool HasPointInside(Position pos)
        {
            var rect = GetBoundingRect();

            Position origin = new Position(
                (rect.Left + rect.Right) / 2,
                (rect.Top + rect.Bottom) / 2);

            Common.Size radius = new Common.Size(
                rect.Width / 2,
                rect.Height / 2);

            if (radius.width == 0 || radius.height == 0)
            {
                return false;
            }

            return (1.0 * Sqr(pos.x - origin.x) / Sqr(radius.width)) +
                (1.0 * Sqr(pos.y - origin.y) / Sqr(radius.height)) < 1;
        }

        public override IShape Clone()
        {
            return new Circle(GetBoundingRect());
        }
    }
}
