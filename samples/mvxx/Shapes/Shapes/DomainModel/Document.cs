using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.DomainModel
{
    class Document
    {
        private class Shape : Facade.IShape
        {
            private readonly Document _parent;
            private readonly int _index;

            public Shape(Document parent, int index)
            {
                _parent = parent;
                _index = index;
            }

            private Common.Shape GetShape()
            {
                return _parent._canvas.GetShape(_index);
            }

            public Common.ShapeType GetShapeType()
            {
                return GetShape().type;
            }

            public Common.Rectangle GetBoundingRect()
            {
                return GetShape().boundingRect;
            }

            public void SetBoundingRect(Common.Rectangle rect)
            {
                if (!GetBoundingRect().Equals(rect))
                {
                    _parent._historyCanvas.MoveShape(_index, rect);
                }
            }
        }

        private class HistoryCanvasDelegate : HistoryCanvas.IDelegate
        {
            private readonly Document _parent;

            public HistoryCanvasDelegate(Document parent)
            {
                _parent = parent;
            }

            void HistoryCanvas.IDelegate.AddCommand(Command.ICommand command)
            {
                _parent._history.AddAndExecuteCommand(command);
            }

            void HistoryCanvas.IDelegate.OnInsertShape(int index)
            {
                _parent.ShapeInsertEvent(index);
            }

            void HistoryCanvas.IDelegate.OnRemoveShape(int index)
            {
                _parent.ShapeRemoveEvent(index);
            }

            void HistoryCanvas.IDelegate.OnMoveShape(int index)
            {
                _parent.ShapeModifyEvent(index);
            }
        }

        private readonly Canvas _canvas;
        private readonly History _history;
        private readonly HistoryCanvas _historyCanvas;

        public Document(Canvas canvas)
        {
            _canvas = canvas;
            _history = new History();
            _historyCanvas = new HistoryCanvas(_canvas, new HistoryCanvasDelegate(this));
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _historyCanvas.AddShape(type, rect);
        }

        public Facade.IShape GetShape(int index)
        {
            return new Shape(this, index);
        }

        public void RemoveShape(int index)
        {
            _historyCanvas.RemoveShape(index);
        }

        public void Undo()
        {
            _history.Undo();
        }

        public void Redo()
        {
            _history.Redo();
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        public delegate void IndexDelegate(int index);
        public event IndexDelegate ShapeInsertEvent;
        public event IndexDelegate ShapeModifyEvent;
        public event IndexDelegate ShapeRemoveEvent;
    }
}
