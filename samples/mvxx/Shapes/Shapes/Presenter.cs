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
        private class LifecycleDecisionEvents : Common.ILifecycleDecisionEvents
        {
            private readonly View.CanvasView _view;

            public LifecycleDecisionEvents(View.CanvasView view)
            {
                _view = view;
            }

            private View.CanvasView.IDialogHandlers GetDialogHandlers()
            {
                return _view.ViewHandlers.DialogHandlers;
            }

            public Option<string> RequestOpenPath()
            {
                return GetDialogHandlers().ShowOpenFileDialog();
            }

            public Option<string> RequestSavePath()
            {
                return GetDialogHandlers().ShowSaveFileDialog();
            }

            public Common.ClosingAction RequestUnsavedClosing()
            {
                return GetDialogHandlers().ShowUnsavedDocumentClosePrompt();
            }
        }

        private class ViewEvents : View.CanvasView.IViewEvents
        {
            private class ShapeOperationEventsHandler : View.CanvasView.IShapeOperationEvents
            {
                private readonly AppModel.Facade _appModel;

                public ShapeOperationEventsHandler(AppModel.Facade appModel)
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

            private class MouseEventsHandler : View.CanvasView.IMouseEvents
            {
                private readonly Common.IPointerDrag _pointerDrag;

                public MouseEventsHandler(Common.IPointerDrag pointerDrag)
                {
                    _pointerDrag = pointerDrag;
                }

                public void Down(Common.Position pos)
                {
                    _pointerDrag.BeginMove(pos);
                }

                public void Move(Common.Position pos)
                {
                    _pointerDrag.Move(pos);
                }

                public void Up(Common.Position pos)
                {
                    _pointerDrag.EndMove(pos);
                }
            }

            private readonly AppModel.Facade _appModel;
            private readonly ShapeOperationEventsHandler _shapeOperationEvents;
            private readonly MouseEventsHandler _mouseEvents;

            public ViewEvents(AppModel.Facade appModel)
            {
                _appModel = appModel;
                _shapeOperationEvents = new ShapeOperationEventsHandler(_appModel);
                _mouseEvents = new MouseEventsHandler(_appModel.Pointer);
            }

            public Common.IDocumentLifecycle DocumentLifecycleEvents
            {
                get => _appModel.DocumentLifecycle;
            }

            public View.CanvasView.IShapeOperationEvents ShapeOperationEvents
            {
                get => _shapeOperationEvents;
            }

            public Common.IUndoRedo HistoryEvents
            {
                get => _appModel.History;
            }

            public View.CanvasView.IMouseEvents MouseEvents
            {
                get => _mouseEvents;
            }

            public bool FormClosing()
            {
                return _appModel.DocumentLifecycle.New();
            }

            public Common.Size CanvasSize
            {
                get => _appModel.CanvasSize;
            }
        }

        private readonly AppModel.Facade _appModel;
        private readonly View.CanvasView _view;

        public Presenter(AppModel.Facade appModel, View.CanvasView view)
        {
            _appModel = appModel;
            _view = view;

            _appModel.SetLifecycleDecisionEvents(new LifecycleDecisionEvents(_view));
            _view.ViewEvents = new ViewEvents(_appModel);

            Initialize();
        }

        private void Initialize()
        {
            _appModel.CompleteLayoutUpdateEvent += () => {
                _view.SetSelectionIndex(-1);

                while (_view.ShapeCount > 0)
                {
                    _view.RemoveShape(0);
                }

                for (int i = 0; i < _appModel.ShapeCount; ++i)
                {
                    _view.AddShape(i, _appModel.GetShape(i));
                }

                _view.SetSelectionIndex(_appModel.SelectedIndex);

                _view.ViewHandlers.InvalidateLayout();
            };

            _appModel.ShapeInsertEvent += (int index) => {
                _view.AddShape(index, _appModel.GetShape(index));
                _view.ViewHandlers.InvalidateLayout();
            };

            _appModel.ShapeModifyEvent += (int index) => {
                _view.GetShape(index).boundingRect = _appModel.GetShape(index).boundingRect;
                _view.ViewHandlers.InvalidateLayout();
            };

            _appModel.ShapeRemoveEvent += (int index) => {
                _view.RemoveShape(index);
                _view.ViewHandlers.InvalidateLayout();
            };

            _appModel.SelectionChangeEvent += (int index) => {
                _view.SetSelectionIndex(index);
                _view.ViewHandlers.InvalidateLayout();
            };
        }
    }
}
