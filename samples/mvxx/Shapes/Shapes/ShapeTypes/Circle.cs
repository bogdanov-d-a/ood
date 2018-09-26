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

        public override bool IsInside(Position pos)
        {
            throw new NotImplementedException();
        }

        public override IShape Clone()
        {
            return new Circle(GetBoundingRect());
        }
    }
}
