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

            appModel.AfterShapeInsertEvent += (int index) => {
                view.ShapeList.Insert(index, RectangleDoubleToInt(appModel.GetShapeAt(index), size));
                view.InvalidateEvent();
            };

            appModel.AfterShapeSetEvent += (int index) => {
                view.ShapeList[index] = RectangleDoubleToInt(appModel.GetShapeAt(index), size);
                view.InvalidateEvent();
            };

            appModel.BeforeShapeRemoveEvent += (int index) => {
                view.ShapeList.RemoveAt(index);
                view.InvalidateEvent();
            };

            appModel.SelectedShapeIndex.Event += (int index) => {
                view.SelectedShapeIndex.Value = index;
            };

            view.OnMoveEvent += (int index, RectangleI pos) => {
                var rect = RectangleIntToDouble(pos, size);

                rect.Left = Math.Max(rect.Left, 0);
                rect.Top = Math.Max(rect.Top, 0);

                double rightOutbound = Math.Max(rect.Right - 1, 0);
                rect.Left -= rightOutbound;

                double bottomOutbound = Math.Max(rect.Bottom - 1, 0);
                rect.Top -= bottomOutbound;

                appModel.ActualSelectedShape = Option.Some(rect);
                view.ShapeList[view.SelectedShapeIndex.Value] = RectangleDoubleToInt(rect, size);
            };

            view.SelectedShapeIndex.Event += (int index) => {
                appModel.SelectedShapeIndex.Value = index;
            };

            view.OnFinishMovingEvent += (int index, RectangleI pos) => {
                appModel.SetShapeAt(index, RectangleIntToDouble(pos, size));
            };
        }
    }
}
