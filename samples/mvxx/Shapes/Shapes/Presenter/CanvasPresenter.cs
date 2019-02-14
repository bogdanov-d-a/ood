using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Presenter
{
    public class CanvasPresenter
    {
        public CanvasPresenter(DomainModel.DocumentKeeper documentKeeper, AppModel.AppModel appModel, View.CanvasView view)
        {
            appModel.CompleteLayoutUpdateEvent += () => {
                view.SetSelectionIndex(-1);

                while (view.ShapeCount > 0)
                {
                    view.RemoveShape(0);
                }

                for (int i = 0; i < documentKeeper.Canvas.ShapeCount; ++i)
                {
                    view.AddShape(i, appModel.GetShape(i));
                }

                view.SetSelectionIndex(appModel.SelectedIndex);

                view.InvalidateLayout();
            };

            documentKeeper.ShapeInsertEvent += (int index) => {
                view.AddShape(index, appModel.GetShape(index));
                view.InvalidateLayout();
            };

            appModel.ShapeModifyEvent += (int index) => {
                view.GetShape(index).boundingRect = appModel.GetShape(index).boundingRect;
                view.InvalidateLayout();
            };

            appModel.ShapeRemoveEvent += (int index) => {
                view.RemoveShape(index);
                view.InvalidateLayout();
            };

            appModel.SelectionChangeEvent += (int index) => {
                view.SetSelectionIndex(index);
                view.InvalidateLayout();
            };

            view.CanvasSizeProvider += () => documentKeeper.Canvas.CanvasSize;
        }
    }
}
