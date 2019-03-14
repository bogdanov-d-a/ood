using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    using RectangleSignallingValue = Common.SignallingValue<Common.Rectangle<double>>;

    public class DomainModel
    {
        public readonly RectangleSignallingValue ShapeBoundingRect =
            new RectangleSignallingValue(new Common.RectangleDouble(0, 0, 0, 0));
    }
}
