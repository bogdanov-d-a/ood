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

        public override bool IsInside(Position pos)
        {
            throw new NotImplementedException();
        }

        public override IShape Clone()
        {
            return new Triangle(GetBoundingRect());
        }
    }
}
