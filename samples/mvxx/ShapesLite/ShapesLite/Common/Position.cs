using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapesLite.Common
{
    public struct Position<T> : IEquatable<Position<T>> where T : IEquatable<T>
    {
        public T x;
        public T y;

        public Position(T x, T y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Position<T> o)
        {
            return x.Equals(o.x) && y.Equals(o.y);
        }
    }
}
