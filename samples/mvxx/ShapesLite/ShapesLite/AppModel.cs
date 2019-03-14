using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class AppModel
    {
        public readonly Common.SignallingValue<Common.Rectangle<double>> ShapeBoundingRect =
            new Common.SignallingValue<Common.Rectangle<double>>(new Common.RectangleDouble(0, 0, 0, 0));

        public readonly Common.SignallingValue<bool> IsShapeSelected = new Common.SignallingValue<bool>(false);

        private readonly DomainModel _domainModel;

        public AppModel(DomainModel domainModel)
        {
            _domainModel = domainModel;

            ShapeBoundingRect.Event += (Common.Rectangle<double> rect) => {
                rect.Left = Math.Max(rect.Left, 0);
                rect.Top = Math.Max(rect.Top, 0);

                double rightOutbound = Math.Max(rect.Right - 1, 0);
                rect.Left -= rightOutbound;

                double bottomOutbound = Math.Max(rect.Bottom - 1, 0);
                rect.Top -= bottomOutbound;
            };
        }

        public Common.SignallingValue<Common.Rectangle<double>> DomainShapeBoundingRect
        {
            get => _domainModel.ShapeBoundingRect;
        }
    }
}
