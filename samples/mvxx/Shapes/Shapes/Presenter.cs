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

        private readonly DomainModel.Document _document;
        private readonly DocumentDelegateProxy _documentDelegateProxy;
        private readonly AppModel.AppModel _appModel;
        private readonly CanvasView _view;
        private readonly CanvasViewData _viewData;

        public Presenter(DomainModel.Document document, DocumentDelegateProxy documentDelegateProxy, AppModel.AppModel appModel, CanvasView view, CanvasViewData viewData)
        {
            _document = document;
            _documentDelegateProxy = documentDelegateProxy;
            _appModel = appModel;
            _view = view;
            _viewData = viewData;

            Initialize();
        }

        private void Initialize()
        {
            _appModel.LayoutUpdatedEvent += new AppModel.AppModel.LayoutUpdatedDelegate(() => {
                _view.SetSelectionIndex(-1);

                while (_view.ShapeCount > 0)
                {
                    _view.RemoveShape(0);
                }

                for (int i = 0; i < _appModel.ShapeCount; ++i)
                {
                    _view.AddShape(i, _appModel.GetShape(i));
                }

                _view.SetSelectionIndex(_appModel.GetSelectedIndex());

                _view.LayoutUpdatedEvent();
            });

            _documentDelegateProxy.OpenFileEvent += new DocumentDelegateProxy.OpenSaveFileDelegate((string path) => {
                _document.ReplaceCanvasData((DomainModel.Document.AddShapeDelegate delegate_) => {
                    Utils.CanvasReaderWriter.Read(path, (Common.ShapeType type, Common.Rectangle boundingRect) => {
                        delegate_(type, boundingRect);
                    });
                });
            });

            _documentDelegateProxy.SaveFileEvent += new DocumentDelegateProxy.OpenSaveFileDelegate((string path) => {
                Utils.CanvasReaderWriter.Write((Utils.CanvasReaderWriter.WriteShapeDelegate delegate_) => {
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
