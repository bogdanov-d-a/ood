using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class AppModel
    {
        public readonly Common.SignallingValue<Common.Rectangle<double>> Position =
            new Common.SignallingValue<Common.Rectangle<double>>(new Common.RectangleDouble(0, 0, 0, 0));

        public readonly Common.SignallingValue<bool> IsSelected = new Common.SignallingValue<bool>(false);
    }
}
