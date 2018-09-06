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

        public bool CheckBounds(Common.Rectangle rectangle)
        {
            return rectangle.leftTop.x >= 0 &&
                rectangle.leftTop.y >= 0 &&
                rectangle.RightBottom.x < size.width &&
                rectangle.RightBottom.y < size.height;
        }

        public void ClampBounds(ref Common.Rectangle rectangle)
        {
            rectangle.leftTop.x = Math.Max(0, rectangle.leftTop.x);
            rectangle.leftTop.y = Math.Max(0, rectangle.leftTop.y);

            int moveX = Math.Min(size.width - 1, rectangle.RightBottom.x) - rectangle.RightBottom.x;
            rectangle.leftTop.x += moveX;

            int moveY = Math.Min(size.height - 1, rectangle.RightBottom.y) - rectangle.RightBottom.y;
            rectangle.leftTop.y += moveY;
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

        public Common.Size SizeP
        {
            get {
                return size;
            }
        }

        public int Count
        {
            get {
                return list.Count;
            }
        }
    }
}
