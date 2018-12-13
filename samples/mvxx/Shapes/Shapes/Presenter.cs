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

        private class ViewCommands : View.CanvasView.IViewCommands
        {
            private readonly Presenter _parent;

            public ViewCommands(Presenter parent)
            {
                _parent = parent;
            }

            public void AddCircle()
            {
                _parent._appModel.AddCircle();
            }

            public void AddRectangle()
            {
                _parent._appModel.AddRectangle();
            }

            public void AddTriangle()
            {
                _parent._appModel.AddTriangle();
            }

            public void CreateNewDocument()
            {
                _parent._document.New();
            }

            public bool FormClosing()
            {
                return _parent._document.New();
            }

            public Common.Size GetCanvasSize()
            {
                return _parent._appModel.CanvasSize;
            }

            public void MouseDown(Common.Position pos)
            {
                _parent._appModel.BeginMove(pos);
            }

            public void MouseMove(Common.Position pos)
            {
                _parent._appModel.Move(pos);
            }

            public void MouseUp(Common.Position pos)
            {
                _parent._appModel.EndMove(pos);
            }

            public void OpenDocument()
            {
                _parent._document.Open();
            }

            public void Redo()
            {
                _parent._appModel.ResetSelection();
                _parent._document.Redo();
            }

            public void RemoveShape()
            {
                _parent._appModel.RemoveSelectedShape();
            }

            public void SaveAsDocument()
            {
                _parent._document.SaveAs();
            }

            public void SaveDocument()
            {
                _parent._document.Save();
            }

            public void Undo()
            {
                _parent._appModel.ResetSelection();
                _parent._document.Undo();
            }
        }

        private readonly DomainModel.Document _document;
        private readonly DocumentDelegateProxy _documentDelegateProxy;
        private readonly AppModel.AppModel _appModel;
        private readonly View.CanvasView _view;

        public Presenter(DomainModel.Document document, DocumentDelegateProxy documentDelegateProxy, AppModel.AppModel appModel, View.CanvasView view)
        {
            _document = document;
            _documentDelegateProxy = documentDelegateProxy;
            _appModel = appModel;
            _view = view;
            _view.ViewCommands = new ViewCommands(this);

            Initialize();
        }

        private void Initialize()
        {
            _appModel.CompleteLayoutUpdateEvent += new AppModel.AppModel.VoidDelegate(() => {
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

                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.ShapeInsertEvent += new AppModel.AppModel.IndexDelegate((int index) => {
                var shape = _document.GetShape(index);
                _view.AddShape(index, new Common.Shape(shape.GetShapeType(), shape.GetBoundingRect()));
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.ShapeModifyEvent += new AppModel.AppModel.IndexDelegate((int index) => {
                _view.GetShape(index).boundingRect = _appModel.GetShape(index).boundingRect;
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.ShapeRemoveEvent += new AppModel.AppModel.IndexDelegate((int index) => {
                _view.RemoveShape(index);
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.SelectionChangeEvent += new AppModel.AppModel.IndexDelegate((int index) => {
                _view.SetSelectionIndex(index);
                _view.ViewHandlers.InvalidateLayout();
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
                return _view.ViewHandlers.ShowOpenFileDialog();
            });

            _documentDelegateProxy.RequestDocumentSavePathEvent += new DocumentDelegateProxy.RequestDocumentPathDelegate(() => {
                return _view.ViewHandlers.ShowSaveFileDialog();
            });

            _documentDelegateProxy.RequestUnsavedDocumentClosingEvent += new DocumentDelegateProxy.RequestUnsavedDocumentClosingDelegate(() => {
                return _view.ViewHandlers.ShowUnsavedDocumentClosePrompt();
            });
        }
    }
}
