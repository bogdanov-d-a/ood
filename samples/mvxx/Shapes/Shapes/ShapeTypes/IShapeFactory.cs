using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public interface IShapeFactory
    {
        IShape CreateShape(Type type, Common.Rectangle boundingRect);
    }
}
