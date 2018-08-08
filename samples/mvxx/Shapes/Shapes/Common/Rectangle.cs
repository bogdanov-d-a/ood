using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public class Rectangle
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

        public bool Contains(Position pos)
        {
            return pos.x >= leftTop.x &&
                pos.x < RightBottom.x &&
                pos.y >= leftTop.y &&
                pos.y < RightBottom.y;
        }
    }
}
