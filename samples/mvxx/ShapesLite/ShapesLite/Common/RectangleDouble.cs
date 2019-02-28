using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Common
{
    public class RectangleDouble : Rectangle<double>
    {
        public RectangleDouble(double left, double top, double width, double height)
            : base(left, top, width, height)
        {
        }

        public override double Add(double a, double b)
        {
            return a + b;
        }

        public override double Sub(double a, double b)
        {
            return a - b;
        }
    }
}
