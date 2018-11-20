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
        public class DocumentDelegateProxy : DomainModel.Document.IDelegate
        {
            public Option<string> RequestDocumentOpenPath()
            {
                return RequestDocumentOpenPathEvent();
            }

            public Option<string> RequestDocumentSavePath()
            {
                return RequestDocumentSavePathEvent();
            }

            public DomainModel.DocumentLifecycleController.ClosingAction RequestUnsavedDocumentClosing()
            {
                return RequestUnsavedDocumentClosingEvent();
            }

            public delegate Option<string> RequestDocumentPathDelegate();
            public delegate DomainModel.DocumentLifecycleController.ClosingAction RequestUnsavedDocumentClosingDelegate();

            public RequestDocumentPathDelegate RequestDocumentOpenPathEvent;
            public RequestDocumentPathDelegate RequestDocumentSavePathEvent;
            public RequestUnsavedDocumentClosingDelegate RequestUnsavedDocumentClosingEvent;
        }

        private class Drawable : CanvasView.IDrawable
        {
            private readonly ShapeTypes.AbstractShape _shape;
            private readonly Common.Rectangle _rect;

            public Drawable(ShapeTypes.AbstractShape shape, Common.Rectangle rect)
            {
                _shape = shape;
                _rect = rect;
            }

            public void Draw(CanvasView.IRenderTarget target)
            {
                _shape.Draw(target, _rect);
            }
        }

        private readonly DomainModel.Document _document;
        private readonly DocumentDelegateProxy _documentDelegateProxy;
        private readonly AppModel.AppModel _appModel;
        private readonly ShapeTypes.AbstractShapeList _shapeList;
        private readonly CanvasView _view;

        public Presenter(DomainModel.Document document, DocumentDelegateProxy documentDelegateProxy, AppModel.AppModel appModel, ShapeTypes.AbstractShapeList shapeList, CanvasView view)
        {
            _document = document;
            _documentDelegateProxy = documentDelegateProxy;
            _appModel = appModel;
            _shapeList = shapeList;
            _view = view;

            Initialize();
        }

        private void Initialize()
        {
            _appModel.LayoutUpdatedEvent += new AppModel.AppModel.LayoutUpdatedDelegate(() => {
                List<CanvasView.IDrawable> drawables = new List<CanvasView.IDrawable>();
                for (int i = 0; i < _appModel.ShapeCount; ++i)
                {
                    drawables.Add(new Drawable(_shapeList.GetAt(i), _appModel.GetShapeBoundingRect(i)));
                }

                Option<Common.Rectangle> selRect = Option.None<Common.Rectangle>();
                int selIndex = _appModel.GetSelectedIndex();
                if (selIndex != -1)
                {
                    selRect = Option.Some(_appModel.GetShapeBoundingRect(selIndex));
                }

                _view.UpdateLayout(drawables, selRect);
            });

            _documentDelegateProxy.RequestDocumentOpenPathEvent += new DocumentDelegateProxy.RequestDocumentPathDelegate(() => {
                return _view.ShowOpenFileDialogEvent();
            });

            _documentDelegateProxy.RequestDocumentSavePathEvent += new DocumentDelegateProxy.RequestDocumentPathDelegate(() => {
                return _view.ShowSaveFileDialogEvent();
            });

            _documentDelegateProxy.RequestUnsavedDocumentClosingEvent += new DocumentDelegateProxy.RequestUnsavedDocumentClosingDelegate(() => {
                return _view.ShowUnsavedDocumentClosePrompt();
            });

            _view.CanvasSize = Option.Some(_appModel.CanvasSize);
            _view.AddRectangleEvent += new CanvasView.VoidDelegate(_appModel.AddRectangle);
            _view.AddTriangleEvent += new CanvasView.VoidDelegate(_appModel.AddTriangle);
            _view.AddCircleEvent += new CanvasView.VoidDelegate(_appModel.AddCircle);
            _view.RemoveShapeEvent += new CanvasView.VoidDelegate(_appModel.RemoveSelectedShape);

            _view.CreateNewDocumentEvent += new CanvasView.VoidDelegate(() => {
                _document.New();
            });
            _view.OpenDocumentEvent += new CanvasView.VoidDelegate(_document.Open);
            _view.SaveDocumentEvent += new CanvasView.VoidDelegate(_document.Save);
            _view.SaveAsDocumentEvent += new CanvasView.VoidDelegate(_document.SaveAs);

            _view.UndoEvent += new CanvasView.VoidDelegate(_appModel.ResetSelection);
            _view.UndoEvent += new CanvasView.VoidDelegate(() => { _document.Undo(); });
            _view.RedoEvent += new CanvasView.VoidDelegate(_appModel.ResetSelection);
            _view.RedoEvent += new CanvasView.VoidDelegate(() => { _document.Redo(); });

            _view.MouseDownEvent += new CanvasView.MouseDelegate(_appModel.BeginMove);
            _view.MouseMoveEvent += new CanvasView.MouseDelegate(_appModel.Move);
            _view.MouseUpEvent += new CanvasView.MouseDelegate(_appModel.EndMove);

            _view.FormClosingEvent += new CanvasView.BoolDelegate(_document.New);
        }
    }
}
