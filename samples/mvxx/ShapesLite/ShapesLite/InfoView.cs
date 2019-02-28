using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class InfoView
    {
        public Common.SignallingValue<Common.Rectangle<double>> ModelRect =
            new Common.SignallingValue<Common.Rectangle<double>>(new Common.RectangleDouble(0, 0, 0, 0));

        private static string RectToString(Common.Rectangle<double> rect)
        {
            return String.Format("{{ {0:F1}, {1:F1}, {2:F1}, {3:F1} }}",
                rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public string GetText()
        {
            return "ModelRect = " + RectToString(ModelRect.Value);
        }
    }
}
