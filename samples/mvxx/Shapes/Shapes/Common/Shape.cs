using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public class Shape
    {
        public Common.ShapeType type;
        public Common.Rectangle boundingRect;

        public Shape(Common.ShapeType type, Common.Rectangle boundingRect)
        {
            this.type = type;
            this.boundingRect = boundingRect;
        }
    }
}
