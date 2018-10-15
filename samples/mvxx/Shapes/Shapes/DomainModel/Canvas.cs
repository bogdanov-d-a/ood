using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Canvas
    {
        private readonly Common.Size _canvasSize;
        private readonly List<ShapeTypes.IShape> _shapeList = new List<ShapeTypes.IShape>();

        public Canvas(Common.Size canvasSize)
        {
            _canvasSize = canvasSize;
        }

        private bool IsShapeInsideCanvas(Common.Rectangle rectangle)
        {
            return rectangle.Left >= 0 &&
                rectangle.Top >= 0 &&
                rectangle.Right < _canvasSize.width &&
                rectangle.Bottom < _canvasSize.height;
        }

        public void AddShape(ShapeTypes.IShape shape)
        {
            InsertShape(ShapeCount, shape);
        }

        public void InsertShape(int index, ShapeTypes.IShape shape)
        {
            if (!IsShapeInsideCanvas(shape.GetBoundingRect()))
            {
                throw new Exception();
            }
            _shapeList.Insert(index, shape);
            LayoutUpdatedEvent();
        }

        public ShapeTypes.IShape GetShape(int index)
        {
            return _shapeList[index];
        }

        public void ResetShapeRectangle(int index, Common.Rectangle rectangle)
        {
            if (!IsShapeInsideCanvas(rectangle))
            {
                throw new Exception();
            }
            _shapeList[index].SetBoundingRect(rectangle);
            LayoutUpdatedEvent();
        }

        public void RemoveShape(int index)
        {
            _shapeList.RemoveAt(index);
            LayoutUpdatedEvent();
        }

        public Common.Size CanvasSize
        {
            get {
                return _canvasSize;
            }
        }

        public int ShapeCount
        {
            get {
                return _shapeList.Count;
            }
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
