using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class Presenter
    {
        private int DoubleToInt(double value, int factor)
        {
            return (int)Math.Round(value * factor);
        }

        private double IntToDouble(int value, double factor)
        {
            return 1.0 * value / factor;
        }

        public Presenter(DomainModel domainModel, AppModel appModel, View view)
        {
            Common.Size<int> size = view.CanvasSize;

            domainModel.Position.Event += (Common.Rectangle<double> pos) => {
                appModel.Position.Value = pos;
            };

            appModel.Position.Event += (Common.Rectangle<double> pos) => {
                view.Position.Value = new Common.RectangleInt(
                    DoubleToInt(pos.Left, size.width), DoubleToInt(pos.Top, size.height),
                    DoubleToInt(pos.Width, size.width), DoubleToInt(pos.Height, size.height));
            };

            appModel.IsSelected.Event += (bool selected) => {
                view.IsSelected.Value = selected;
            };

            view.Position.Event += (Common.Rectangle<int> pos) => {
                appModel.Position.Value = new Common.RectangleDouble(
                    IntToDouble(pos.Left, size.width), IntToDouble(pos.Top, size.height),
                    IntToDouble(pos.Width, size.width), IntToDouble(pos.Height, size.height));
            };

            view.IsSelected.Event += (bool selected) => {
                appModel.IsSelected.Value = selected;
            };

            view.OnFinishMovingEvent += (Common.Rectangle<int> pos) => {
                domainModel.Position.Value = new Common.RectangleDouble(
                    IntToDouble(pos.Left, size.width), IntToDouble(pos.Top, size.height),
                    IntToDouble(pos.Width, size.width), IntToDouble(pos.Height, size.height));
            };
        }
    }
}
