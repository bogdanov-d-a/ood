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
        public class ModelDelegate : DomainModel.Facade.IDelegate
        {
            private readonly View.CanvasView _view;

            public ModelDelegate(View.CanvasView view)
            {
                _view = view;
            }

            private View.CanvasView.IDialogHandlers GetDialogHandlers()
            {
                return _view.ViewHandlers.GetDialogHandlers();
            }

            public Option<string> RequestDocumentOpenPath()
            {
                return GetDialogHandlers().ShowOpenFileDialog();
            }

            public Option<string> RequestDocumentSavePath()
            {
                return GetDialogHandlers().ShowSaveFileDialog();
            }

            public Common.ClosingAction RequestUnsavedDocumentClosing()
            {
                return GetDialogHandlers().ShowUnsavedDocumentClosePrompt();
            }
        }

        private class ViewEvents : View.CanvasView.IViewEvents
        {
            private class DocumentLifecycleEvents : View.CanvasView.IDocumentLifecycleEvents
            {
                private readonly AppModel.Facade _appModel;

                public DocumentLifecycleEvents(AppModel.Facade appModel)
                {
                    _appModel = appModel;
                }

                public void New()
                {
                    _appModel.New();
                }

                public void Open()
                {
                    _appModel.Open();
                }

                public void Save()
                {
                    _appModel.Save();
                }

                public void SaveAs()
                {
                    _appModel.SaveAs();
                }
            }

            private class ShapeOperationEvents : View.CanvasView.IShapeOperationEvents
            {
                private readonly AppModel.Facade _appModel;

                public ShapeOperationEvents(AppModel.Facade appModel)
                {
                    _appModel = appModel;
                }

                public void AddCircle()
                {
                    _appModel.AddCircle();
                }

                public void AddRectangle()
                {
                    _appModel.AddRectangle();
                }

                public void AddTriangle()
                {
                    _appModel.AddTriangle();
                }

                public void Remove()
                {
                    _appModel.RemoveSelectedShape();
                }
            }

            private class HistoryEvents : View.CanvasView.IHistoryEvents
            {
                private readonly AppModel.Facade _appModel;

                public HistoryEvents(AppModel.Facade appModel)
                {
                    _appModel = appModel;
                }

                public void Undo()
                {
                    _appModel.Undo();
                }

                public void Redo()
                {
                    _appModel.Redo();
                }
            }

            private class MouseEvents : View.CanvasView.IMouseEvents
            {
                private readonly AppModel.Facade _appModel;

                public MouseEvents(AppModel.Facade appModel)
                {
                    _appModel = appModel;
                }

                public void Down(Common.Position pos)
                {
                    _appModel.BeginMove(pos);
                }

                public void Move(Common.Position pos)
                {
                    _appModel.Move(pos);
                }

                public void Up(Common.Position pos)
                {
                    _appModel.EndMove(pos);
                }
            }

            private readonly AppModel.Facade _appModel;
            private readonly DocumentLifecycleEvents _documentLifecycleEvents;
            private readonly ShapeOperationEvents _shapeOperationEvents;
            private readonly HistoryEvents _historyEvents;
            private readonly MouseEvents _mouseEvents;

            public ViewEvents(AppModel.Facade appModel)
            {
                _appModel = appModel;
                _documentLifecycleEvents = new DocumentLifecycleEvents(_appModel);
                _shapeOperationEvents = new ShapeOperationEvents(_appModel);
                _historyEvents = new HistoryEvents(_appModel);
                _mouseEvents = new MouseEvents(_appModel);
            }

            public View.CanvasView.IDocumentLifecycleEvents GetDocumentLifecycleEvents()
            {
                return _documentLifecycleEvents;
            }

            public View.CanvasView.IShapeOperationEvents GetShapeOperationEvents()
            {
                return _shapeOperationEvents;
            }

            public View.CanvasView.IHistoryEvents GetHistoryEvents()
            {
                return _historyEvents;
            }

            public View.CanvasView.IMouseEvents GetMouseEvents()
            {
                return _mouseEvents;
            }

            public bool FormClosing()
            {
                return _appModel.New();
            }

            public Common.Size GetCanvasSize()
            {
                return _appModel.CanvasSize;
            }
        }

        private readonly AppModel.Facade _appModel;
        private readonly View.CanvasView _view;

        public Presenter(AppModel.Facade appModel, View.CanvasView view)
        {
            _appModel = appModel;
            _view = view;

            _appModel.SetDelegate(new ModelDelegate(_view));
            _view.ViewEvents = new ViewEvents(_appModel);

            Initialize();
        }

        private void Initialize()
        {
            _appModel.CompleteLayoutUpdateEvent += new AppModel.Facade.VoidDelegate(() => {
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

            _appModel.ShapeInsertEvent += new AppModel.Facade.IndexDelegate((int index) => {
                _view.AddShape(index, _appModel.GetShape(index));
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.ShapeModifyEvent += new AppModel.Facade.IndexDelegate((int index) => {
                _view.GetShape(index).boundingRect = _appModel.GetShape(index).boundingRect;
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.ShapeRemoveEvent += new AppModel.Facade.IndexDelegate((int index) => {
                _view.RemoveShape(index);
                _view.ViewHandlers.InvalidateLayout();
            });

            _appModel.SelectionChangeEvent += new AppModel.Facade.IndexDelegate((int index) => {
                _view.SetSelectionIndex(index);
                _view.ViewHandlers.InvalidateLayout();
            });
        }
    }
}
