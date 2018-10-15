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
        private readonly AppModel.AppModel _appModel;
        private readonly Shapes _view;

        public Presenter(AppModel.AppModel appModel, Shapes view)
        {
            _appModel = appModel;
            _view = view;

            Initialize();
        }

        private void Initialize()
        {
            _appModel.LayoutUpdatedEvent += new AppModel.AppModel.LayoutUpdatedDelegate(_view.OnLayoutUpdated);

            _view.SetCanvasSize(_appModel.CanvasSize);
            _view.AddRectangleEvent += new Shapes.VoidDelegate(_appModel.AddRectangle);
            _view.AddTriangleEvent += new Shapes.VoidDelegate(_appModel.AddTriangle);
            _view.AddCircleEvent += new Shapes.VoidDelegate(_appModel.AddCircle);
            _view.RemoveShapeEvent += new Shapes.VoidDelegate(_appModel.RemoveSelectedShape);
            _view.UndoEvent += new Shapes.VoidDelegate(_appModel.Undo);
            _view.RedoEvent += new Shapes.VoidDelegate(_appModel.Redo);

            _view.MouseDownEvent += new Shapes.MouseDelegate(_appModel.BeginMove);
            _view.MouseMoveEvent += new Shapes.MouseDelegate(_appModel.Move);
            _view.MouseUpEvent += new Shapes.MouseDelegate(_appModel.EndMove);
        }
    }
}
