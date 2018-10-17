using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    class ShapeFactory : IShapeFactory
    {
        private delegate IShape ShapeCreator(AbstractShape.OnMoveShape onMoveShape, Common.Rectangle boundingRect);
        private static readonly SortedDictionary<Type, ShapeCreator> typeToCreatorMap = new SortedDictionary<Type, ShapeCreator>(){
            { Type.Rectangle, (AbstractShape.OnMoveShape onMoveShape, Common.Rectangle boundingRect) => { return new Rectangle(onMoveShape, boundingRect); } },
            { Type.Triangle, (AbstractShape.OnMoveShape onMoveShape, Common.Rectangle boundingRect) => { return new Triangle(onMoveShape, boundingRect); } },
            { Type.Circle, (AbstractShape.OnMoveShape onMoveShape, Common.Rectangle boundingRect) => { return new Circle(onMoveShape, boundingRect); } },
        };

        private readonly AbstractShape.OnMoveShape _onMoveShape;

        public ShapeFactory(AbstractShape.OnMoveShape onMoveShape)
        {
            _onMoveShape = onMoveShape;
        }

        public IShape CreateShape(Type type, Common.Rectangle boundingRect)
        {
            ShapeCreator creator;
            if (typeToCreatorMap.TryGetValue(type, out creator))
            {
                return creator(_onMoveShape, boundingRect);
            }
            throw new Exception();
        }
    }
}
