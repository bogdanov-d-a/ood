using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class CanvasPresenter
    {
        private int DoubleToInt(double value, int factor)
        {
            return (int)Math.Round(value * factor);
        }

        private double IntToDouble(int value, double factor)
        {
            return 1.0 * value / factor;
        }

        public CanvasPresenter(DomainModel domainModel, AppModel appModel, CanvasView view)
        {
            Common.Size<int> size = view.CanvasSize;

            domainModel.ShapeBoundingRect.Event += (Common.Rectangle<double> pos) => {
                appModel.ShapeBoundingRect.Value = pos;
            };

            appModel.ShapeBoundingRect.Event += (Common.Rectangle<double> pos) => {
                view.ShapeBoundingRect.Value = new Common.RectangleInt(
                    DoubleToInt(pos.Left, size.width), DoubleToInt(pos.Top, size.height),
                    DoubleToInt(pos.Width, size.width), DoubleToInt(pos.Height, size.height));
            };

            appModel.IsShapeSelected.Event += (bool selected) => {
                view.IsShapeSelected.Value = selected;
            };

            view.ShapeBoundingRect.Event += (Common.Rectangle<int> pos) => {
                appModel.ShapeBoundingRect.Value = new Common.RectangleDouble(
                    IntToDouble(pos.Left, size.width), IntToDouble(pos.Top, size.height),
                    IntToDouble(pos.Width, size.width), IntToDouble(pos.Height, size.height));
            };

            view.IsShapeSelected.Event += (bool selected) => {
                appModel.IsShapeSelected.Value = selected;
            };

            view.OnFinishMovingEvent += (Common.Rectangle<int> pos) => {
                domainModel.ShapeBoundingRect.Value = new Common.RectangleDouble(
                    IntToDouble(pos.Left, size.width), IntToDouble(pos.Top, size.height),
                    IntToDouble(pos.Width, size.width), IntToDouble(pos.Height, size.height));
            };
        }
    }
}
