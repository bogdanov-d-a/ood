using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapesLite.Common
{
    public abstract class Rectangle<T> : IEquatable<Rectangle<T>> where T : IEquatable<T>, IComparable<T>
    {
        public abstract T Add(T a, T b);
        public abstract T Sub(T a, T b);

        private Position<T> _leftTop;
        private Size<T> _size;

        public Rectangle(T left, T top, T width, T height)
        {
            _leftTop = new Position<T>(left, top);
            _size = new Size<T>(width, height);
        }

        public Position<T> LeftTop
        {
            get => _leftTop;
        }

        public T Left
        {
            get => _leftTop.x;
            set => _leftTop.x = value;
        }

        public T Top
        {
            get => _leftTop.y;
            set => _leftTop.y = value;
        }

        public Size<T> Size
        {
            get => _size;
        }

        public T Width
        {
            get => _size.width;
            set => _size.width = value;
        }

        public T Height
        {
            get => _size.height;
            set => _size.height = value;
        }

        public Position<T> RightBottom
        {
            get => new Position<T>(Add(Left, Width), Add(Top, Height));
        }

        public T Right
        {
            get => RightBottom.x;
            set
            {
                T grow = Sub(value, Right);
                Width = Add(Width, grow);
            }
        }

        public T Bottom
        {
            get => RightBottom.y;
            set
            {
                T grow = Sub(value, Bottom);
                Height = Add(Height, grow);
            }
        }

        private bool Contains(T min, T max, T val)
        {
            return val.CompareTo(min) >= 0 && val.CompareTo(max) < 0;
        }

        public bool Contains(Position<T> pos)
        {
            return Contains(Left, Right, pos.x) &&
                Contains(Top, Bottom, pos.y);
        }

        public void Offset(Size<T> offset)
        {
            Left = Add(Left, offset.width);
            Top = Add(Top, offset.height);
        }

        public bool Equals(Rectangle<T> o)
        {
            return Equals(_leftTop, o._leftTop) && Equals(_size, o._size);
        }
    }
}
