using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public struct Rectangle
    {
        private Position leftTop;
        private Size size;

        public Rectangle(Position leftTop, Size size)
        {
            this.leftTop = leftTop;
            this.size = size;
        }

        public Position LeftTop
        {
            get {
                return leftTop;
            }
        }

        public int Left
        {
            get {
                return leftTop.x;
            }
            set {
                leftTop.x = value;
            }
        }

        public int Top
        {
            get {
                return leftTop.y;
            }
            set {
                leftTop.y = value;
            }
        }

        public Size Size
        {
            get {
                return size;
            }
        }

        public int Width
        {
            get {
                return size.width;
            }
            set {
                size.width = value;
            }
        }

        public int Height
        {
            get
            {
                return size.height;
            }
            set
            {
                size.height = value;
            }
        }

        public Position RightBottom
        {
            get {
                return new Position(Left + Width, Top + Height);
            }
        }

        public int Right
        {
            get
            {
                return RightBottom.x;
            }
            set
            {
                int grow = value - Right;
                Width += grow;
            }
        }

        public int Bottom
        {
            get
            {
                return RightBottom.y;
            }
            set
            {
                int grow = value - Bottom;
                Height += grow;
            }
        }

        public bool Contains(Position pos)
        {
            return pos.x >= Left &&
                pos.x < Right &&
                pos.y >= Top &&
                pos.y < Bottom;
        }

        public void Offset(Size offset)
        {
            Left += offset.width;
            Top += offset.height;
        }
    }
}
