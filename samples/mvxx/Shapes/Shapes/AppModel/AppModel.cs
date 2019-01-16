using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.AppModel
{
    class AppModel
    {
        private readonly DomainModel.Facade _domainModel;
        private readonly CursorHandler _cursorHandler;
        private int _selectedIndex = -1;

        private class CursorHandlerModel : CursorHandler.IModel
        {
            private readonly AppModel _appModel;

            private class Shape : CursorHandler.IShape
            {
                private readonly DomainModel.Facade _domainModel;
                private readonly int _index;

                public Shape(DomainModel.Facade domainModel, int index)
                {
                    _domainModel = domainModel;
                    _index = index;
                }

                public DomainModel.IShape GetShape()
                {
                    return _domainModel.GetShape(_index);
                }

                public Common.Rectangle BoundingRect
                {
                    get => GetShape().BoundingRect;
                    set => _domainModel.GetShape(_index).BoundingRect = value;
                }

                public bool HasPointInside(Common.Position pos)
                {
                    return Utils.ShapeBoundsChecker.IsInsideShape(new Common.Shape(GetShape().ShapeType, BoundingRect), pos);
                }
            }

            public CursorHandlerModel(AppModel appModel)
            {
                _appModel = appModel;
            }

            public Common.Size CanvasSize
            {
                get => _appModel._domainModel.CanvasSize;
            }

            public int SelectionIndex
            {
                get => _appModel.SelectedIndex;
                set => _appModel.SelectedIndex = value;
            }

            public CursorHandler.IShape GetShape(int index)
            {
                return new Shape(_appModel._domainModel, index);
            }

            public int ShapeCount
            {
                get => _appModel._domainModel.ShapeCount;
            }

            public void OnShapeTransform()
            {
                int index = _appModel.SelectedIndex;
                if (index != -1)
                {
                    _appModel.ShapeModifyEvent(index);
                }
            }
        }

        public AppModel(DomainModel.Facade domainModel)
        {
            _domainModel = domainModel;
            _domainModel.CompleteLayoutUpdateEvent += new DomainModel.Facade.VoidDelegate(() => {
                _selectedIndex = -1;
                CompleteLayoutUpdateEvent();
            });
            _domainModel.ShapeModifyEvent += new DomainModel.Facade.IndexDelegate((int index) => {
                ShapeModifyEvent(index);
            });
            _domainModel.ShapeRemoveEvent += new DomainModel.Facade.IndexDelegate((int index) => {
                if (_selectedIndex >= _domainModel.ShapeCount)
                {
                    _selectedIndex = -1;
                }
                ShapeRemoveEvent(index);
            });
            _cursorHandler = new CursorHandler(new CursorHandlerModel(this));
        }

        public Common.Shape GetShape(int index)
        {
            return new Common.Shape(_domainModel.GetShape(index).ShapeType, GetShapeBoundingRect(index));
        }

        private Common.Rectangle GetShapeBoundingRect(int index)
        {
            if (index == _selectedIndex)
            {
                var rect = _cursorHandler.GetTransformingRect();
                if (rect.HasValue)
                {
                    return rect.ValueOrFailure();
                }
            }
            return _domainModel.GetShape(index).BoundingRect;
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set {
                _selectedIndex = value;
                SelectionChangeEvent(_selectedIndex);
            }
        }

        public void RemoveSelectedShape()
        {
            if (_selectedIndex != -1)
            {
                int tmpSelectedIndex = _selectedIndex;
                _selectedIndex = -1;
                SelectionChangeEvent(_selectedIndex);
                _domainModel.RemoveShape(tmpSelectedIndex);
            }
        }

        public void ResetSelection()
        {
            SelectedIndex = -1;
        }

        public void BeginMove(Common.Position pos)
        {
            _cursorHandler.BeginMove(pos);
        }

        public void Move(Common.Position pos)
        {
            _cursorHandler.Move(pos);
        }

        public void EndMove(Common.Position pos)
        {
            _cursorHandler.EndMove(pos);
        }

        public delegate void VoidDelegate();
        public event VoidDelegate CompleteLayoutUpdateEvent;

        public delegate void IndexDelegate(int index);
        public event IndexDelegate ShapeModifyEvent;
        public event IndexDelegate ShapeRemoveEvent;
        public event IndexDelegate SelectionChangeEvent;
    }
}
