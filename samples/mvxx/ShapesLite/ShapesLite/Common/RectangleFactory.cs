using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Common
{
    public class RectangleFactory
    {
        private class RectangleInt : RectangleT<int>
        {
            public RectangleInt(Position<int> leftTop, Size<int> size)
                : base(leftTop, size)
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

            public override RectangleT<int> Copy()
            {
                return MakeRectangleInt(LeftTop, Size);
            }
        }

        private class RectangleDouble : RectangleT<double>
        {
            public RectangleDouble(Position<double> leftTop, Size<double> size)
                : base(leftTop, size)
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

            public override RectangleT<double> Copy()
            {
                return MakeRectangleDouble(LeftTop, Size);
            }
        }

        private RectangleFactory()
        {
        }

        public static RectangleT<int> MakeRectangleInt(Position<int> leftTop, Size<int> size)
        {
            return new RectangleInt(leftTop, size);
        }

        public static RectangleT<double> MakeRectangleDouble(Position<double> leftTop, Size<double> size)
        {
            return new RectangleDouble(leftTop, size);
        }
    }
}
