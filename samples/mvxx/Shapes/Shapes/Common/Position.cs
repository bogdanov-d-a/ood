using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public struct Position
    {
        public static readonly Position Empty = new Position(0, 0);

        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Position o)
        {
            return x == o.x && y == o.y;
        }

        public static Size Sub(Position a, Position b)
        {
            return new Size(a.x - b.x, a.y - b.y);
        }
    }
}
