using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Presenter
{
    public class ShapeActionsPresenter
    {
        public ShapeActionsPresenter(AppModel.Facade appModel, View.ShapeActionsView view)
        {
            view.OnAddRectangle += () => {
                appModel.AddRectangle();
            };
            view.OnAddTriangle += () => {
                appModel.AddTriangle();
            };
            view.OnAddCircle += () => {
                appModel.AddCircle();
            };
            view.OnRemove += () => {
                appModel.RemoveSelectedShape();
            };
        }
    }
}
