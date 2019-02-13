using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Presenter
{
    public class CanvasPresenter
    {
        public CanvasPresenter(AppModel.Facade model, View.CanvasView view)
        {
            model.CompleteLayoutUpdateEvent += () => {
                view.SetSelectionIndex(-1);

                while (view.ShapeCount > 0)
                {
                    view.RemoveShape(0);
                }

                for (int i = 0; i < model.ShapeCount; ++i)
                {
                    view.AddShape(i, model.GetShape(i));
                }

                view.SetSelectionIndex(model.SelectedIndex);

                view.InvalidateLayout();
            };

            model.ShapeInsertEvent += (int index) => {
                view.AddShape(index, model.GetShape(index));
                view.InvalidateLayout();
            };

            model.ShapeModifyEvent += (int index) => {
                view.GetShape(index).boundingRect = model.GetShape(index).boundingRect;
                view.InvalidateLayout();
            };

            model.ShapeRemoveEvent += (int index) => {
                view.RemoveShape(index);
                view.InvalidateLayout();
            };

            model.SelectionChangeEvent += (int index) => {
                view.SetSelectionIndex(index);
                view.InvalidateLayout();
            };

            view.CanvasSizeProvider += () => {
                return model.CanvasSize;
            };
        }
    }
}
