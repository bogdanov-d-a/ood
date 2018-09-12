using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public struct Rectangle
    {
        public Position LeftTop;
        public Size Size;

        public Rectangle(Position leftTop, Size size)
        {
            this.LeftTop = leftTop;
            this.Size = size;
        }

        public Position RightBottom
        {
            get {
                return new Position(LeftTop.x + Size.width, LeftTop.y + Size.height);
            }
        }

        public void SetRight(int value)
        {
            int grow = value - RightBottom.x;
            Size.width += grow;
        }

        public void SetBottom(int value)
        {
            int grow = value - RightBottom.y;
            Size.height += grow;
        }

        public bool Contains(Position pos)
        {
            return pos.x >= LeftTop.x &&
                pos.x < RightBottom.x &&
                pos.y >= LeftTop.y &&
                pos.y < RightBottom.y;
        }

        public void Offset(Size offset)
        {
            LeftTop.x += offset.width;
            LeftTop.y += offset.height;
        }
    }
}
