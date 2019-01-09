using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Utils
{
    public delegate void ShapeInfoDelegate(Common.ShapeType type, Common.Rectangle boundingRect);
    public delegate void ShapeEnumerator(ShapeInfoDelegate delegate_);

    public interface IShapeSerializer
    {
        void SerializeShapes(ShapeEnumerator shapeEnumerator);
    }
}
