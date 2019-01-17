using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;

namespace Shapes.DomainModel
{
    class Document
    {
        private class Shape : IShape
        {
            private readonly Document _document;
            private readonly int _index;

            public Shape(Document document, int index)
            {
                _document = document;
                _index = index;
            }

            private Common.Shape GetShape()
            {
                return _document._canvas.GetShapeAt(_index);
            }

            public Common.ShapeType ShapeType
            {
                get => GetShape().type;
            }

            public Common.Rectangle BoundingRect
            {
                get => GetShape().boundingRect;
                set {
                    if (!BoundingRect.Equals(value))
                    {
                        _document._canvasCommandCreator.MoveShape(_index, value);
                    }
                }
            }
        }

        private class CanvasCommandCreatorEvents : CanvasCommandCreator.IEvents
        {
            private class ShapeEventsHandler : CanvasCommandCreator.IShapeEvents
            {
                private readonly Document _document;

                public ShapeEventsHandler(Document document)
                {
                    _document = document;
                }

                public void OnInsert(int index)
                {
                    _document.ShapeInsertEvent(index);
                }

                public void OnMove(int index)
                {
                    _document.ShapeModifyEvent(index);
                }

                public void OnRemove(int index)
                {
                    _document.ShapeRemoveEvent(index);
                }
            }

            private readonly Document _document;
            private readonly ShapeEventsHandler _shapeEvents;

            public CanvasCommandCreatorEvents(Document document)
            {
                _document = document;
                _shapeEvents = new ShapeEventsHandler(_document);
            }

            public void AddCommand(Command.ICommand command)
            {
                _document._history.AddAndExecuteCommand(command);
            }

            public CanvasCommandCreator.IShapeEvents ShapeEvents
            {
                get => _shapeEvents;
            }
        }

        private class UndoRedoHandlers : Common.IUndoRedo
        {
            private readonly History _history;

            public UndoRedoHandlers(History history)
            {
                _history = history;
            }

            public void Redo()
            {
                _history.Redo();
            }

            public void Undo()
            {
                _history.Undo();
            }
        }

        private readonly Canvas _canvas;
        private readonly History _history;
        private readonly CanvasCommandCreator _canvasCommandCreator;
        private readonly UndoRedoHandlers _undoRedoHandlers;

        public Document(Canvas canvas)
        {
            _canvas = canvas;
            _history = new History();
            _canvasCommandCreator = new CanvasCommandCreator(_canvas, new CanvasCommandCreatorEvents(this));
            _undoRedoHandlers = new UndoRedoHandlers(_history);
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _canvasCommandCreator.AddShape(type, rect);
        }

        public IShape GetShape(int index)
        {
            return new Shape(this, index);
        }

        public void RemoveShape(int index)
        {
            _canvasCommandCreator.RemoveShape(index);
        }

        public Common.IUndoRedo History
        {
            get => _undoRedoHandlers;
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        public Command.ICommand GetLastExecutedCommand()
        {
            return _history.GetLastExecuted();
        }

        public event Common.DelegateTypes.Int ShapeInsertEvent;
        public event Common.DelegateTypes.Int ShapeModifyEvent;
        public event Common.DelegateTypes.Int ShapeRemoveEvent;
    }
}
