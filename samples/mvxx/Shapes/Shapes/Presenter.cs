﻿using System;
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

            public void OpenDocument()
            {
                _parent._document.Open();
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
        }

        private readonly DomainModel.Document _document;
        private readonly DocumentDelegateProxy _documentDelegateProxy;
        private readonly AppModel.AppModel _appModel;
        private readonly View.CanvasView _view;
        private readonly View.CanvasViewData _viewData;

        public Presenter(DomainModel.Document document, DocumentDelegateProxy documentDelegateProxy, AppModel.AppModel appModel, View.CanvasView view, View.CanvasViewData viewData)
        {
            _document = document;
            _documentDelegateProxy = documentDelegateProxy;
            _appModel = appModel;
            _view = view;
            _view.ViewCommands = new ViewCommands(this);
            _viewData = viewData;

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

                _view.LayoutUpdatedEvent();
            });

            _appModel.ShapeInsertEvent += new AppModel.AppModel.IndexDelegate((int index) => {
                var shape = _document.GetShape(index);
                _view.AddShape(index, new Common.Shape(shape.GetShapeType(), shape.GetBoundingRect()));
                _view.LayoutUpdatedEvent();
            });

            _appModel.ShapeModifyEvent += new AppModel.AppModel.IndexDelegate((int index) => {
                _view.GetShape(index).boundingRect = _appModel.GetShape(index).boundingRect;
                _view.LayoutUpdatedEvent();
            });

            _appModel.ShapeRemoveEvent += new AppModel.AppModel.IndexDelegate((int index) => {
                _view.RemoveShape(index);
                _view.LayoutUpdatedEvent();
            });

            _appModel.SelectionChangeEvent += new AppModel.AppModel.IndexDelegate((int index) => {
                _view.SetSelectionIndex(index);
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

            _viewData.UndoEvent += new View.CanvasViewData.VoidDelegate(_appModel.ResetSelection);
            _viewData.UndoEvent += new View.CanvasViewData.VoidDelegate(() => { _document.Undo(); });
            _viewData.RedoEvent += new View.CanvasViewData.VoidDelegate(_appModel.ResetSelection);
            _viewData.RedoEvent += new View.CanvasViewData.VoidDelegate(() => { _document.Redo(); });

            _viewData.MouseDownEvent += new View.CanvasViewData.MouseDelegate(_appModel.BeginMove);
            _viewData.MouseMoveEvent += new View.CanvasViewData.MouseDelegate(_appModel.Move);
            _viewData.MouseUpEvent += new View.CanvasViewData.MouseDelegate(_appModel.EndMove);

            _viewData.FormClosingEvent += new View.CanvasViewData.BoolDelegate(_document.New);
        }
    }
}
