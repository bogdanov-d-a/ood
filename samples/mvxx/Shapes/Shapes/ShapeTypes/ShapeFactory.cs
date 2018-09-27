using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    class ShapeFactory : IShapeFactory
    {
        private delegate IShape ShapeCreator(Common.Rectangle boundingRect);
        private static readonly SortedDictionary<Type, ShapeCreator> typeToCreatorMap = new SortedDictionary<Type, ShapeCreator>(){
            { Type.Rectangle, (Common.Rectangle boundingRect) => { return new Rectangle(boundingRect); } },
            { Type.Triangle, (Common.Rectangle boundingRect) => { return new Triangle(boundingRect); } },
            { Type.Circle, (Common.Rectangle boundingRect) => { return new Circle(boundingRect); } },
        };

        public IShape CreateShape(Type type, Common.Rectangle boundingRect)
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
