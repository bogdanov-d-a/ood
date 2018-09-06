using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes
{
    public class Presenter
    {
        private readonly AppModel.AppModel appModel;
        private readonly Shapes view;

        public Presenter(AppModel.AppModel appModel, Shapes view)
        {
            this.appModel = appModel;
            this.view = view;

            Initialize();
        }

        private void Initialize()
        {
            appModel.LayoutUpdatedEvent += new AppModel.AppModel.LayoutUpdatedDelegate(view.OnLayoutUpdated);

            view.SetCanvasSize(appModel.CanvasSize);
            view.AddRectangleEvent += new Shapes.VoidDelegate(appModel.AddRectangle);
            view.RemoveShapeEvent += new Shapes.VoidDelegate(appModel.RemoveSelectedShape);

            view.MouseDownEvent += new Shapes.MouseDelegate(appModel.BeginMove);
            view.MouseMoveEvent += new Shapes.MouseDelegate(appModel.Move);
            view.MouseUpEvent += new Shapes.MouseDelegate(appModel.EndMove);

            view.RequestRectangles += new Shapes.RectangleEnumeratorDelegate((Shapes.RectangleInfoDelegate infoDelegate) => {
                Option<Common.Size> moveOffset = appModel.GetMoveOffsetIfMoving();

                for (int i = 0; i < appModel.RectangleCount; ++i)
                {
                    if (moveOffset.HasValue && i == appModel.GetSelectedIndex())
                    {
                        continue;
                    }
                    infoDelegate(appModel.GetRectangle(i), i == appModel.GetSelectedIndex());
                }

                if (moveOffset.HasValue)
                {
                    var moValue = moveOffset.ValueOrFailure();
                    var rect = appModel.GetRectangle(appModel.GetSelectedIndex());
                    rect.Offset(moValue);
                    infoDelegate(rect, true);
                }
            });
        }
    }
}
