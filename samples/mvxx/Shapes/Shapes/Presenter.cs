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
            view.AddTriangleEvent += new Shapes.VoidDelegate(appModel.AddTriangle);
            view.AddCircleEvent += new Shapes.VoidDelegate(appModel.AddCircle);
            view.RemoveShapeEvent += new Shapes.VoidDelegate(appModel.RemoveSelectedShape);

            view.MouseDownEvent += new Shapes.MouseDelegate(appModel.BeginMove);
            view.MouseMoveEvent += new Shapes.MouseDelegate(appModel.Move);
            view.MouseUpEvent += new Shapes.MouseDelegate(appModel.EndMove);

            view.AssignRequestShapesHandler(new Shapes.ShapeEnumeratorDelegate((Shapes.ShapeInfoDelegate infoDelegate) => {
                for (int i = 0; i < appModel.ShapeCount; ++i)
                {
                    var shape = appModel.GetShape(i);
                    infoDelegate((ShapeTypes.AbstractShape)shape, i == appModel.GetSelectedIndex());
                }
            }));
        }
    }
}
