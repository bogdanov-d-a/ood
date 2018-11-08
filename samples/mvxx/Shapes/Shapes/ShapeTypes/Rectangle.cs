using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public class Rectangle : AbstractShape
    {
        public Rectangle(Common.Rectangle boundingRect)
            : base(boundingRect)
        {
        }

        public override Common.ShapeType GetShapeType()
        {
            return Common.ShapeType.Rectangle;
        }

        public override void Draw(CanvasView.IRenderTarget target, Common.Rectangle rect)
        {
            target.DrawRectangle(rect);
        }

        public override bool HasPointInside(Common.Position pos)
        {
            return GetBoundingRect().Contains(pos);
        }
    }
}
