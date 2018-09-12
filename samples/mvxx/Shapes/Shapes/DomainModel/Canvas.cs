using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Canvas
    {
        private readonly Common.Size size;
        private readonly List<Common.Rectangle> list = new List<Common.Rectangle>();

        public Canvas(Common.Size size)
        {
            this.size = size;
        }

        private bool CheckBounds(Common.Rectangle rectangle)
        {
            return rectangle.leftTop.x >= 0 &&
                rectangle.leftTop.y >= 0 &&
                rectangle.RightBottom.x < size.width &&
                rectangle.RightBottom.y < size.height;
        }

        public void AddRectangle(Common.Rectangle rectangle)
        {
            if (!CheckBounds(rectangle))
            {
                throw new Exception();
            }
            list.Add(rectangle);
        }

        public Common.Rectangle GetRectangle(int index)
        {
            return list[index];
        }

        public void ResetRectangle(int index, Common.Rectangle rectangle)
        {
            if (!CheckBounds(rectangle))
            {
                throw new Exception();
            }
            list[index] = rectangle;
        }

        public void RemoveRectangle(int index)
        {
            list.RemoveAt(index);
        }

        public Common.Size CanvasSize
        {
            get {
                return size;
            }
        }

        public int ShapeCount
        {
            get {
                return list.Count;
            }
        }
    }
}
