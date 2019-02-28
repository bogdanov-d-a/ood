using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapesLite.Common
{
    public struct Size<T> : IEquatable<Size<T>> where T : IEquatable<T>
    {
        public T width;
        public T height;

        public Size(T width, T height)
        {
            this.width = width;
            this.height = height;
        }

        public bool Equals(Size<T> o)
        {
            return width.Equals(o.width) && height.Equals(o.height);
        }
    }
}
