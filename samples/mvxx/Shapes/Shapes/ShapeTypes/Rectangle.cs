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

        public override int GetTypeId()
        {
            return 0;
        }

        public override void Draw(IRenderTarget target)
        {
            target.DrawRectangle(GetBoundingRect());
        }

        public override bool IsInside(Position pos)
        {
            throw new NotImplementedException();
        }

        public override IShape Clone()
        {
            return new Rectangle(GetBoundingRect());
        }
    }
}
