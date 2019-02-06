using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.Presenter
{
    public class Presenter
    {
        private class ViewEvents : View.CanvasView.IViewEvents
        {
            private readonly AppModel.Facade _appModel;

            public ViewEvents(AppModel.Facade appModel)
            {
                _appModel = appModel;
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
