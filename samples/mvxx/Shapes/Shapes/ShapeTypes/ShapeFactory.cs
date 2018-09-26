using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shapes.Common;

namespace Shapes.ShapeTypes
{
    class ShapeFactory : IShapeFactory
    {
        public IShape CreateShape(int type, Common.Rectangle boundingRect)
        {
            switch (type)
            {
                case 0:
                    return new Rectangle(boundingRect);
                case 1:
                    return new Triangle(boundingRect);
                case 2:
                    return new Circle(boundingRect);
                default:
                    throw new Exception();
            }
        }
    }
}
