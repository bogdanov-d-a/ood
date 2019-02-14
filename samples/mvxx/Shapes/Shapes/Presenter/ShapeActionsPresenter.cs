using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Presenter
{
    public class ShapeActionsPresenter
    {
        private static readonly Common.Rectangle defRect = new Common.Rectangle(new Common.Position(200, 100), new Common.Size(300, 200));

        public ShapeActionsPresenter(DomainModel.DocumentKeeper documentKeeper, AppModel.AppModel appModel, View.ShapeActionsView view)
        {
            view.OnAddRectangle += () => documentKeeper.Document.AddShape(Common.ShapeType.Rectangle, defRect);
            view.OnAddTriangle += () => documentKeeper.Document.AddShape(Common.ShapeType.Triangle, defRect);
            view.OnAddCircle += () => documentKeeper.Document.AddShape(Common.ShapeType.Circle, defRect);
            view.OnRemove += () => appModel.RemoveSelectedShape();
        }
    }
}
