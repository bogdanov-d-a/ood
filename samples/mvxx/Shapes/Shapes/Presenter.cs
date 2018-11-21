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
            void DomainModel.Document.IDelegate.OnOpenDocument(string path)
            {
                OpenFileEvent(path);
            }

            void DomainModel.Document.IDelegate.OnSaveDocument(string path)
            {
                SaveFileEvent(path);
            }

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

            public delegate void OpenSaveFileDelegate(string path);
            public delegate Option<string> RequestDocumentPathDelegate();
            public delegate DomainModel.DocumentLifecycleController.ClosingAction RequestUnsavedDocumentClosingDelegate();

            public OpenSaveFileDelegate OpenFileEvent;
            public OpenSaveFileDelegate SaveFileEvent;
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
        private readonly CanvasViewData _viewData;

        public Presenter(DomainModel.Document document, DocumentDelegateProxy documentDelegateProxy, AppModel.AppModel appModel, ShapeTypes.AbstractShapeList shapeList, CanvasView view, CanvasViewData viewData)
        {
            _document = document;
            _documentDelegateProxy = documentDelegateProxy;
            _appModel = appModel;
            _shapeList = shapeList;
            _view = view;
            _viewData = viewData;

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

            _documentDelegateProxy.OpenFileEvent += new DocumentDelegateProxy.OpenSaveFileDelegate((string path) => {
                _document.ReplaceCanvasData((DomainModel.Document.AddShapeDelegate delegate_) => {
                    CanvasReaderWriter.Read(path, (Common.ShapeType type, Common.Rectangle boundingRect) => {
                        delegate_(type, boundingRect);
                    });
                });
            });

            _documentDelegateProxy.SaveFileEvent += new DocumentDelegateProxy.OpenSaveFileDelegate((string path) => {
                CanvasReaderWriter.Write((CanvasReaderWriter.WriteShapeDelegate delegate_) => {
                    for (int i = 0; i < _document.ShapeCount; ++i)
                    {
                        var shape = _document.GetShape(i);
                        delegate_(shape.GetShapeType(), shape.GetBoundingRect());
                    }
                }, path);
            });

            _documentDelegateProxy.RequestDocumentOpenPathEvent += new DocumentDelegateProxy.RequestDocumentPathDelegate(() => {
                return _viewData.ShowOpenFileDialogEvent();
            });

            _documentDelegateProxy.RequestDocumentSavePathEvent += new DocumentDelegateProxy.RequestDocumentPathDelegate(() => {
                return _viewData.ShowSaveFileDialogEvent();
            });

            _documentDelegateProxy.RequestUnsavedDocumentClosingEvent += new DocumentDelegateProxy.RequestUnsavedDocumentClosingDelegate(() => {
                return _viewData.ShowUnsavedDocumentClosePrompt();
            });

            _viewData.CanvasSize = Option.Some(_appModel.CanvasSize);
            _viewData.AddRectangleEvent += new CanvasViewData.VoidDelegate(_appModel.AddRectangle);
            _viewData.AddTriangleEvent += new CanvasViewData.VoidDelegate(_appModel.AddTriangle);
            _viewData.AddCircleEvent += new CanvasViewData.VoidDelegate(_appModel.AddCircle);
            _viewData.RemoveShapeEvent += new CanvasViewData.VoidDelegate(_appModel.RemoveSelectedShape);

            _viewData.CreateNewDocumentEvent += new CanvasViewData.VoidDelegate(() => {
                _document.New();
            });
            _viewData.OpenDocumentEvent += new CanvasViewData.VoidDelegate(_document.Open);
            _viewData.SaveDocumentEvent += new CanvasViewData.VoidDelegate(_document.Save);
            _viewData.SaveAsDocumentEvent += new CanvasViewData.VoidDelegate(_document.SaveAs);

            _viewData.UndoEvent += new CanvasViewData.VoidDelegate(_appModel.ResetSelection);
            _viewData.UndoEvent += new CanvasViewData.VoidDelegate(() => { _document.Undo(); });
            _viewData.RedoEvent += new CanvasViewData.VoidDelegate(_appModel.ResetSelection);
            _viewData.RedoEvent += new CanvasViewData.VoidDelegate(() => { _document.Redo(); });

            _viewData.MouseDownEvent += new CanvasViewData.MouseDelegate(_appModel.BeginMove);
            _viewData.MouseMoveEvent += new CanvasViewData.MouseDelegate(_appModel.Move);
            _viewData.MouseUpEvent += new CanvasViewData.MouseDelegate(_appModel.EndMove);

            _viewData.FormClosingEvent += new CanvasViewData.BoolDelegate(_document.New);
        }
    }
}
