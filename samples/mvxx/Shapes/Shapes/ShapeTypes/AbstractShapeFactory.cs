using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    class AbstractShapeFactory
    {
        private delegate AbstractShape ShapeCreator(Common.Rectangle boundingRect);
        private static readonly SortedDictionary<Common.ShapeType, ShapeCreator> typeToCreatorMap = new SortedDictionary<Common.ShapeType, ShapeCreator>(){
            { Common.ShapeType.Rectangle, (Common.Rectangle boundingRect) => { return new Rectangle(boundingRect); } },
            { Common.ShapeType.Triangle, (Common.Rectangle boundingRect) => { return new Triangle(boundingRect); } },
            { Common.ShapeType.Circle, (Common.Rectangle boundingRect) => { return new Circle(boundingRect); } },
        };

        public AbstractShape CreateShape(Common.ShapeType type, Common.Rectangle boundingRect)
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
