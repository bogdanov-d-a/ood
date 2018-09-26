using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shapes.Common;

namespace Shapes.ShapeTypes
{
    class ShapeFactory : IShapeFactory
    {
        private delegate IShape ShapeCreator(Common.Rectangle boundingRect);
        private static readonly SortedDictionary<ShapeTypes.Type, ShapeCreator> typeToCreatorMap = new SortedDictionary<ShapeTypes.Type, ShapeCreator>(){
            { ShapeTypes.Type.Rectangle, (Common.Rectangle boundingRect) => { return new Rectangle(boundingRect); } },
            { ShapeTypes.Type.Triangle, (Common.Rectangle boundingRect) => { return new Triangle(boundingRect); } },
            { ShapeTypes.Type.Circle, (Common.Rectangle boundingRect) => { return new Circle(boundingRect); } },
        };

        public IShape CreateShape(ShapeTypes.Type type, Common.Rectangle boundingRect)
        {
            ShapeCreator creator;
            if (typeToCreatorMap.TryGetValue(type, out creator))
            {
                return creator(boundingRect);
            }
            throw new Exception();
        }
    }
}
