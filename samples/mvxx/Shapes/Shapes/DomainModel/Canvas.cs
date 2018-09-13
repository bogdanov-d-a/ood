using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Canvas
    {
        public enum ShapeType
        {
            Rectangle,
            Triangle,
            Circle,
        }

        public struct Shape
        {
            public Shape(ShapeType type, Common.Rectangle boundingRect)
            {
                this.type = type;
                this.boundingRect = boundingRect;
            }

            public ShapeType type;
            public Common.Rectangle boundingRect;
        }

        private readonly Common.Size canvasSize;
        private readonly List<Shape> shapeList = new List<Shape>();

        public Canvas(Common.Size canvasSize)
        {
            this.canvasSize = canvasSize;
        }

        private bool IsShapeInsideCanvas(Common.Rectangle rectangle)
        {
            return rectangle.LeftTop.x >= 0 &&
                rectangle.LeftTop.y >= 0 &&
                rectangle.RightBottom.x < canvasSize.width &&
                rectangle.RightBottom.y < canvasSize.height;
        }

        public void AddShape(Shape shape)
        {
            if (!IsShapeInsideCanvas(shape.boundingRect))
            {
                throw new Exception();
            }
            shapeList.Add(shape);
        }

        public Shape GetShape(int index)
        {
            return shapeList[index];
        }

        public void ResetShapeRectangle(int index, Common.Rectangle rectangle)
        {
            if (!IsShapeInsideCanvas(rectangle))
            {
                throw new Exception();
            }
            shapeList[index] = new Shape(shapeList[index].type, rectangle);
        }

        public void RemoveShape(int index)
        {
            shapeList.RemoveAt(index);
        }

        public Common.Size CanvasSize
        {
            get {
                return canvasSize;
            }
        }

        public int ShapeCount
        {
            get {
                return shapeList.Count;
            }
        }
    }
}
