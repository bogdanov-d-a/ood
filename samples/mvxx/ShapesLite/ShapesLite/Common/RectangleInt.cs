using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Common
{
    public class RectangleInt : Rectangle<int>
    {
        public RectangleInt(int left, int top, int width, int height)
            : base(left, top, width, height)
        {
        }

        public override int Add(int a, int b)
        {
            return a + b;
        }

        public override int Sub(int a, int b)
        {
            return a - b;
        }
    }
}
