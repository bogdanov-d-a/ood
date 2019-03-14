using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    using RectangleSignallingValue = Common.SignallingValue<Common.Rectangle<double>>;
    using BoolSignallingValue = Common.SignallingValue<bool>;

    public class AppModel
    {
        public readonly RectangleSignallingValue ShapeBoundingRect =
            new RectangleSignallingValue(new Common.RectangleDouble(0, 0, 0, 0));

        public readonly BoolSignallingValue IsShapeSelected = new BoolSignallingValue(false);

        private readonly DomainModel _domainModel;

        public AppModel(DomainModel domainModel)
        {
            _domainModel = domainModel;

            _domainModel.ShapeBoundingRect.Event += (Common.Rectangle<double> pos) => {
                ShapeBoundingRect.Value = pos;
            };

            ShapeBoundingRect.Event += (Common.Rectangle<double> rect) => {
                rect.Left = Math.Max(rect.Left, 0);
                rect.Top = Math.Max(rect.Top, 0);

                double rightOutbound = Math.Max(rect.Right - 1, 0);
                rect.Left -= rightOutbound;

                double bottomOutbound = Math.Max(rect.Bottom - 1, 0);
                rect.Top -= bottomOutbound;
            };
        }

        public RectangleSignallingValue DomainShapeBoundingRect
        {
            get => _domainModel.ShapeBoundingRect;
        }
    }
}
