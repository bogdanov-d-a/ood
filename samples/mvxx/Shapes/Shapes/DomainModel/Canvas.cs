using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Canvas
    {
        private readonly Common.Size canvasSize;
        private readonly List<ShapeTypes.IShape> shapeList = new List<ShapeTypes.IShape>();
        private readonly ShapeTypes.IShapeFactory shapeFactory;

        public Canvas(Common.Size canvasSize, ShapeTypes.IShapeFactory shapeFactory)
        {
            this.canvasSize = canvasSize;
            this.shapeFactory = shapeFactory;
        }

        private bool IsShapeInsideCanvas(Common.Rectangle rectangle)
        {
            return rectangle.Left >= 0 &&
                rectangle.Top >= 0 &&
                rectangle.Right < canvasSize.width &&
                rectangle.Bottom < canvasSize.height;
        }

        public void AddShape(ShapeTypes.IShape shape)
        {
            if (!IsShapeInsideCanvas(shape.GetBoundingRect()))
            {
                throw new Exception();
            }
            shapeList.Add(shape);
            LayoutUpdatedEvent();
        }

        public void AddShape(ShapeTypes.Type type, Common.Rectangle boundingRect)
        {
            AddShape(shapeFactory.CreateShape(type, boundingRect));
        }

        public ShapeTypes.IShape GetShape(int index)
        {
            return shapeList[index];
        }

        public void ResetShapeRectangle(int index, Common.Rectangle rectangle)
        {
            if (!IsShapeInsideCanvas(rectangle))
            {
                throw new Exception();
            }
            shapeList[index].SetBoundingRect(rectangle);
            LayoutUpdatedEvent();
        }

        public void RemoveShape(int index)
        {
            shapeList.RemoveAt(index);
            LayoutUpdatedEvent();
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

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
