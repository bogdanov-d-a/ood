using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.AppModel
{
    public class AppModel
    {
        private class UndoRedoHandlers : Common.IUndoRedo
        {
            private readonly AppModel _appModel;

            public UndoRedoHandlers(AppModel appModel)
            {
                _appModel = appModel;
            }

            public void Redo()
            {
                _appModel.ResetSelection();
                _appModel._documentKeeper.Document.History.Redo();
            }

            public void Undo()
            {
                _appModel.ResetSelection();
                _appModel._documentKeeper.Document.History.Undo();
            }
        }

        private readonly DomainModel.DocumentKeeper _documentKeeper;
        private readonly CursorHandler _cursorHandler;
        private readonly UndoRedoHandlers _undoRedoHandlers;
        private int _selectedIndex = -1;

        private class CursorHandlerModel : CursorHandler.IModel
        {
            private readonly AppModel _appModel;

            private class Shape : CursorHandler.IShape
            {
                private readonly DomainModel.DocumentKeeper _documentKeeper;
                private readonly int _index;

                public Shape(DomainModel.DocumentKeeper documentKeeper, int index)
                {
                    _documentKeeper = documentKeeper;
                    _index = index;
                }

                public DomainModel.IShape GetShape()
                {
                    return _documentKeeper.Document.GetShape(_index);
                }

                public Common.Rectangle BoundingRect
                {
                    get => GetShape().BoundingRect;
                    set => _documentKeeper.Document.GetShape(_index).BoundingRect = value;
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
                get => _appModel._documentKeeper.Canvas.CanvasSize;
            }

            public int SelectionIndex
            {
                get => _appModel.SelectedIndex;
                set => _appModel.SelectedIndex = value;
            }

            public CursorHandler.IShape GetShape(int index)
            {
                return new Shape(_appModel._documentKeeper, index);
            }

            public int ShapeCount
            {
                get => _appModel._documentKeeper.Canvas.ShapeCount;
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

        public AppModel(DomainModel.DocumentKeeper documentKeeper)
        {
            _documentKeeper = documentKeeper;

            _documentKeeper.CompleteLayoutUpdateEvent += () => {
                _selectedIndex = -1;
                CompleteLayoutUpdateEvent();
            };
            _documentKeeper.ShapeModifyEvent += (int index) => ShapeModifyEvent(index);
            _documentKeeper.ShapeRemoveEvent += (int index) => {
                if (_selectedIndex >= _documentKeeper.Canvas.ShapeCount)
                {
                    _selectedIndex = -1;
                }
                ShapeRemoveEvent(index);
            };

            _cursorHandler = new CursorHandler(new CursorHandlerModel(this));
            _undoRedoHandlers = new UndoRedoHandlers(this);
        }

        public Common.Shape GetShape(int index)
        {
            return new Common.Shape(_documentKeeper.Document.GetShape(index).ShapeType, GetShapeBoundingRect(index));
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
            return _documentKeeper.Document.GetShape(index).BoundingRect;
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
                _documentKeeper.Document.RemoveShape(tmpSelectedIndex);
            }
        }

        public void ResetSelection()
        {
            SelectedIndex = -1;
        }

        public Common.IPointerDrag Pointer
        {
            get => _cursorHandler;
        }

        public Common.IUndoRedo History
        {
            get => _undoRedoHandlers;
        }

        public event Common.DelegateTypes.Void CompleteLayoutUpdateEvent;

        public event Common.DelegateTypes.Int ShapeModifyEvent;
        public event Common.DelegateTypes.Int ShapeRemoveEvent;
        public event Common.DelegateTypes.Int SelectionChangeEvent;
    }
}
