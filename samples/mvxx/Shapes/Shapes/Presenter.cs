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
        private readonly DomainModel.Document _document;
        private readonly AppModel.AppModel _appModel;
        private readonly CanvasView _view;

        public Presenter(DomainModel.Document document, AppModel.AppModel appModel, CanvasView view)
        {
            _document = document;
            _appModel = appModel;
            _view = view;

            Initialize();
        }

        private void Initialize()
        {
            _appModel.LayoutUpdatedEvent += new AppModel.AppModel.LayoutUpdatedDelegate(() => {
                _view.LayoutUpdatedEvent();
            });

            _view.CanvasSize = Option.Some(_appModel.CanvasSize);
            _view.AddRectangleEvent += new CanvasView.VoidDelegate(_appModel.AddRectangle);
            _view.AddTriangleEvent += new CanvasView.VoidDelegate(_appModel.AddTriangle);
            _view.AddCircleEvent += new CanvasView.VoidDelegate(_appModel.AddCircle);
            _view.RemoveShapeEvent += new CanvasView.VoidDelegate(_appModel.RemoveSelectedShape);

            _view.UndoEvent += new CanvasView.VoidDelegate(_appModel.ResetSelection);
            _view.UndoEvent += new CanvasView.VoidDelegate(() => { _document.Undo(); });
            _view.RedoEvent += new CanvasView.VoidDelegate(_appModel.ResetSelection);
            _view.RedoEvent += new CanvasView.VoidDelegate(() => { _document.Redo(); });

            _view.MouseDownEvent += new CanvasView.MouseDelegate(_appModel.BeginMove);
            _view.MouseMoveEvent += new CanvasView.MouseDelegate(_appModel.Move);
            _view.MouseUpEvent += new CanvasView.MouseDelegate(_appModel.EndMove);
        }
    }
}
