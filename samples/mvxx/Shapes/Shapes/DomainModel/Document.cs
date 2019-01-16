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
            private class ShapeEvents : CanvasCommandCreator.IShapeEvents
            {
                private readonly Document _document;

                public ShapeEvents(Document document)
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
            private readonly ShapeEvents _shapeEvents;

            public CanvasCommandCreatorEvents(Document document)
            {
                _document = document;
                _shapeEvents = new ShapeEvents(_document);
            }

            public void AddCommand(Command.ICommand command)
            {
                _document._history.AddAndExecuteCommand(command);
            }

            public CanvasCommandCreator.IShapeEvents GetShapeEvents()
            {
                return _shapeEvents;
            }
        }

        private readonly Canvas _canvas;
        private readonly History _history;
        private readonly CanvasCommandCreator _canvasCommandCreator;

        public Document(Canvas canvas)
        {
            _canvas = canvas;
            _history = new History();
            _canvasCommandCreator = new CanvasCommandCreator(_canvas, new CanvasCommandCreatorEvents(this));
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

        public Command.ICommand GetLastExecutedCommand()
        {
            return _history.GetLastExecuted();
        }

        public delegate void IndexDelegate(int index);
        public event IndexDelegate ShapeInsertEvent;
        public event IndexDelegate ShapeModifyEvent;
        public event IndexDelegate ShapeRemoveEvent;
    }
}
