using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class DomainModel
    {
        public readonly Common.SignallingValue<Common.Rectangle<double>> ShapeBoundingRect =
            new Common.SignallingValue<Common.Rectangle<double>>(new Common.RectangleDouble(0, 0, 0, 0));

        public DomainModel()
        {
            ShapeBoundingRect.Event += (Common.Rectangle<double> rect) => {
                rect.Left = Math.Max(rect.Left, 0);
                rect.Top = Math.Max(rect.Top, 0);

                double rightOutbound = Math.Max(rect.Right - 1, 0);
                rect.Left -= rightOutbound;

                double bottomOutbound = Math.Max(rect.Bottom - 1, 0);
                rect.Top -= bottomOutbound;
            };
        }
    }
}
