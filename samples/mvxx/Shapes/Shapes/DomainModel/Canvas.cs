using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Canvas
    {
        public interface IShape
        {
            Common.ShapeType GetShapeType();
            Common.Rectangle GetBoundingRect();
            void SetBoundingRect(Common.Rectangle rect);
        }

        public interface IShapeList
        {
            void Insert(int index, Common.ShapeType type, Common.Rectangle rect);
            IShape GetAt(int index);
            void RemoveAt(int index);
            int GetCount();
        }

        private readonly Common.Size _canvasSize;
        private readonly IShapeList _shapeList;

        public Canvas(Common.Size canvasSize, IShapeList shapeList)
        {
            _canvasSize = canvasSize;
            _shapeList = shapeList;
        }

        private bool IsShapeInsideCanvas(Common.Rectangle rectangle)
        {
            return rectangle.Left >= 0 &&
                rectangle.Top >= 0 &&
                rectangle.Right < _canvasSize.width &&
                rectangle.Bottom < _canvasSize.height;
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            InsertShape(ShapeCount, type, rect);
        }

        public void InsertShape(int index, Common.ShapeType type, Common.Rectangle rect)
        {
            if (!IsShapeInsideCanvas(rect))
            {
                throw new Exception();
            }
            _shapeList.Insert(index, type, rect);
            LayoutUpdatedEvent();
        }

        public IShape GetShape(int index)
        {
            return _shapeList.GetAt(index);
        }

        public void RemoveShape(int index)
        {
            _shapeList.RemoveAt(index);
            LayoutUpdatedEvent();
        }

        public void RemoveAllShapes()
        {
            while (_shapeList.GetCount() > 0)
            {
                _shapeList.RemoveAt(0);
            }
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
                return _shapeList.GetCount();
            }
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
