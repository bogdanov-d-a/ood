using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public struct Rectangle
    {
        public Position leftTop;
        public Size size;

        public Rectangle(Position leftTop, Size size)
        {
            this.leftTop = leftTop;
            this.size = size;
        }

        public Position RightBottom
        {
            get {
                return new Position(leftTop.x + size.width, leftTop.y + size.height);
            }
        }

        public void SetRight(int value)
        {
            int grow = value - RightBottom.x;
            size.width += grow;
        }

        public void SetBottom(int value)
        {
            int grow = value - RightBottom.y;
            size.height += grow;
        }

        public bool Contains(Position pos)
        {
            return pos.x >= leftTop.x &&
                pos.x < RightBottom.x &&
                pos.y >= leftTop.y &&
                pos.y < RightBottom.y;
        }

        public void Offset(Size offset)
        {
            leftTop.x += offset.width;
            leftTop.y += offset.height;
        }
    }
}
