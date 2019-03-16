using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optional;

namespace ShapesLite.Presenters
{
    using RectangleD = Common.Rectangle<double>;
    using RectangleI = Common.Rectangle<int>;
    using SizeI = Common.Size<int>;

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

        private RectangleI RectangleDoubleToInt(RectangleD rect, SizeI factor)
        {
            return new Common.RectangleInt(
                    DoubleToInt(rect.Left, factor.width), DoubleToInt(rect.Top, factor.height),
                    DoubleToInt(rect.Width, factor.width), DoubleToInt(rect.Height, factor.height));
        }

        private RectangleD RectangleIntToDouble(RectangleI rect, SizeI factor)
        {
            return new Common.RectangleDouble(
                    IntToDouble(rect.Left, factor.width), IntToDouble(rect.Top, factor.height),
                    IntToDouble(rect.Width, factor.width), IntToDouble(rect.Height, factor.height));
        }

        public CanvasPresenter(AppModel appModel, Views.CanvasView view)
        {
            SizeI size = view.CanvasSize;

            appModel.ShapeList.AfterInsertEvent += (int index, RectangleD value) => {
                view.ShapeList.Insert(index, RectangleDoubleToInt(value, size));
            };

            appModel.ShapeList.AfterSetEvent += (int index, RectangleD value) => {
                view.ShapeList.SetAt(index, RectangleDoubleToInt(value, size));
            };

            appModel.ShapeList.BeforeRemoveEvent += (int index, RectangleD value) => {
                view.ShapeList.RemoveAt(index);
            };

            appModel.SelectedShapeIndex.Event += (int index) => {
                view.SelectedShapeIndex.Value = index;
            };

            view.ShapeList.AfterSetEvent += (int index, RectangleI pos) => {
                appModel.ActualSelectedShape = Option.Some(RectangleIntToDouble(pos, size));
            };

            view.SelectedShapeIndex.Event += (int index) => {
                appModel.SelectedShapeIndex.Value = index;
            };

            view.OnFinishMovingEvent += (int index, RectangleI pos) => {
                appModel.ShapeList.SetAt(index, RectangleIntToDouble(pos, size));
            };
        }
    }
}
