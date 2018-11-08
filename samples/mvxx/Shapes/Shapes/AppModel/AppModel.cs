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
        public delegate bool HasPointInsideDelegate(int index, Common.Position pos);

        private static readonly Common.Rectangle defRect = new Common.Rectangle(new Common.Position(200, 100), new Common.Size(300, 200));

        private readonly DomainModel.Document _document;
        private readonly CursorHandler _cursorHandler;
        private readonly CursorHandlerModel _cursorHandlerModel;
        HasPointInsideDelegate _hasPointInside;
        private int _selectedIndex = -1;

        private class CursorHandlerModel : CursorHandler.IModel
        {
            private readonly AppModel _parent;

            private class Shape : CursorHandler.IShape
            {
                private readonly AppModel _parent;
                private readonly int _index;

                public Shape(AppModel parent, int index)
                {
                    _parent = parent;
                    _index = index;
                }

                public Common.Rectangle GetBoundingRect()
                {
                    return _parent._document.GetShape(_index).GetBoundingRect();
                }

                public bool HasPointInside(Common.Position pos)
                {
                    return _parent._hasPointInside(_index, pos);
                }

                public void SetBoundingRect(Common.Rectangle rect)
                {
                    _parent._document.GetShape(_index).SetBoundingRect(rect);
                }
            }

            public CursorHandlerModel(AppModel parent)
            {
                _parent = parent;
            }

            public Common.Size GetCanvasSize()
            {
                return _parent.CanvasSize;
            }

            public int GetSelectionIndex()
            {
                return _parent.GetSelectedIndex();
            }

            public CursorHandler.IShape GetShape(int index)
            {
                return new Shape(_parent, index);
            }

            public int GetShapeCount()
            {
                return _parent.ShapeCount;
            }

            public void OnLayoutUpdated()
            {
                _parent.LayoutUpdatedEvent();
            }

            public void SelectShape(int index)
            {
                _parent.SelectShape(index);
            }
        }

        public AppModel(DomainModel.Document document, HasPointInsideDelegate hasPointInside)
        {
            _document = document;
            _document.LayoutUpdatedEvent += new DomainModel.Document.LayoutUpdatedDelegate(() => {
                LayoutUpdatedEvent();
            });
            _hasPointInside = hasPointInside;
            _cursorHandlerModel = new CursorHandlerModel(this);
            _cursorHandler = new CursorHandler(_cursorHandlerModel);
        }

        private void AddShape(Common.ShapeType type)
        {
            _document.AddShape(type, defRect);
            LayoutUpdatedEvent();
        }

        public void AddRectangle()
        {
            AddShape(Common.ShapeType.Rectangle);
        }

        public void AddTriangle()
        {
            AddShape(Common.ShapeType.Triangle);
        }

        public void AddCircle()
        {
            AddShape(Common.ShapeType.Circle);
        }

        public Common.Rectangle GetShapeBoundingRect(int index)
        {
            if (index == _selectedIndex)
            {
                var rect = _cursorHandler.GetTransformingRect();
                if (rect.HasValue)
                {
                    return rect.ValueOrFailure();
                }
            }
            return _document.GetShape(index).GetBoundingRect();
        }

        public int GetSelectedIndex()
        {
            return _selectedIndex;
        }

        private void SelectShape(int index)
        {
            _selectedIndex = index;
            LayoutUpdatedEvent();
        }

        public void RemoveSelectedShape()
        {
            if (_selectedIndex != -1)
            {
                int tmpSelectedIndex = _selectedIndex;
                _selectedIndex = -1;
                _document.RemoveShape(tmpSelectedIndex);
                LayoutUpdatedEvent();
            }
        }

        public void ResetSelection()
        {
            SelectShape(-1);
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

        public int ShapeCount
        {
            get {
                return _document.ShapeCount;
            }
        }

        public Common.Size CanvasSize
        {
            get {
                return _document.CanvasSize;
            }
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
