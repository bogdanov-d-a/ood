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

            domainModel.Position.Event += (Common.RectangleT<double> pos) => {
                appModel.Position.Value = pos;
            };

            appModel.Position.Event += (Common.RectangleT<double> pos) => {
                view.Position.Value = Common.RectangleFactory.MakeRectangleInt(
                    new Common.Position<int>(DoubleToInt(pos.Left, size.width), DoubleToInt(pos.Top, size.height)),
                    new Common.Size<int>(DoubleToInt(pos.Width, size.width), DoubleToInt(pos.Height, size.height)));
            };

            appModel.IsSelected.Event += (bool selected) => {
                view.IsSelected.Value = selected;
            };

            view.Position.Event += (Common.RectangleT<int> pos) => {
                appModel.Position.Value = Common.RectangleFactory.MakeRectangleDouble(
                    new Common.Position<double>(IntToDouble(pos.Left, size.width), IntToDouble(pos.Top, size.height)),
                    new Common.Size<double>(IntToDouble(pos.Width, size.width), IntToDouble(pos.Height, size.height)));
            };

            view.IsSelected.Event += (bool selected) => {
                appModel.IsSelected.Value = selected;
            };

            view.OnFinishMovingEvent += (Common.RectangleT<int> pos) => {
                domainModel.Position.Value = Common.RectangleFactory.MakeRectangleDouble(
                    new Common.Position<double>(IntToDouble(pos.Left, size.width), IntToDouble(pos.Top, size.height)),
                    new Common.Size<double>(IntToDouble(pos.Width, size.width), IntToDouble(pos.Height, size.height)));
            };
        }
    }
}
