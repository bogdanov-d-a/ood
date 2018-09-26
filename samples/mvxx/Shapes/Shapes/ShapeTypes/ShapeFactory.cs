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
        private static readonly SortedDictionary<int, ShapeCreator> typeToCreatorMap = new SortedDictionary<int, ShapeCreator>(){
            { 0, (Common.Rectangle boundingRect) => { return new Rectangle(boundingRect); } },
            { 1, (Common.Rectangle boundingRect) => { return new Triangle(boundingRect); } },
            { 2, (Common.Rectangle boundingRect) => { return new Circle(boundingRect); } },
        };

        public IShape CreateShape(int type, Common.Rectangle boundingRect)
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
