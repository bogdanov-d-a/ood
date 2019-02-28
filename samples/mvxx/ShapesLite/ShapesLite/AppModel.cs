using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class AppModel
    {
        public readonly Common.SignallingValue<Common.RectangleT<double>> Position =
            new Common.SignallingValue<Common.RectangleT<double>>(
                Common.RectangleFactory.MakeRectangleDouble(
                    new Common.Position<double>(), new Common.Size<double>()));

        public readonly Common.SignallingValue<bool> IsSelected = new Common.SignallingValue<bool>(false);
    }
}
