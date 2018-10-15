using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public struct Rectangle
    {
        private Position _leftTop;
        private Size _size;

        public Rectangle(Position leftTop, Size size)
        {
            _leftTop = leftTop;
            _size = size;
        }

        public Position LeftTop
        {
            get {
                return _leftTop;
            }
        }

        public int Left
        {
            get {
                return _leftTop.x;
            }
            set {
                _leftTop.x = value;
            }
        }

        public int Top
        {
            get {
                return _leftTop.y;
            }
            set {
                _leftTop.y = value;
            }
        }

        public Size Size
        {
            get {
                return _size;
            }
        }

        public int Width
        {
            get {
                return _size.width;
            }
            set {
                _size.width = value;
            }
        }

        public int Height
        {
            get
            {
                return _size.height;
            }
            set
            {
                _size.height = value;
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

        public bool Equals(Rectangle o)
        {
            return Equals(_leftTop, o._leftTop) && Equals(_size, o._size);
        }
    }
}
