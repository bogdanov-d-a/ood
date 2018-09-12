using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Canvas
    {
        private readonly Common.Size canvasSize;
        private readonly List<Common.Rectangle> shapeList = new List<Common.Rectangle>();

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

        public void AddRectangle(Common.Rectangle rectangle)
        {
            if (!IsShapeInsideCanvas(rectangle))
            {
                throw new Exception();
            }
            shapeList.Add(rectangle);
        }

        public Common.Rectangle GetRectangle(int index)
        {
            return shapeList[index];
        }

        public void ResetRectangle(int index, Common.Rectangle rectangle)
        {
            if (!IsShapeInsideCanvas(rectangle))
            {
                throw new Exception();
            }
            shapeList[index] = rectangle;
        }

        public void RemoveRectangle(int index)
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
