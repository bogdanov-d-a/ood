using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Document
    {
        public interface IShape
        {
            Common.Rectangle GetBoundingRect();
            void SetBoundingRect(Common.Rectangle rect);
        }

        private class Shape : IShape
        {
            private class Movable : MoveShapeCommand.IMovable
            {
                private readonly Canvas _canvas;
                private readonly int _index;

                public Movable(Canvas canvas, int index)
                {
                    _canvas = canvas;
                    _index = index;
                }

                private Canvas.IShape GetShape()
                {
                    return _canvas.GetShape(_index);
                }

                public Common.Rectangle GetRect()
                {
                    return GetShape().GetBoundingRect();
                }

                public void SetRect(Common.Rectangle rect)
                {
                    GetShape().SetBoundingRect(rect);
                }
            }

            private readonly Movable _movable;
            private readonly History _history;

            public Shape(Canvas canvas, int index, History history)
            {
                _movable = new Movable(canvas, index);
                _history = history;
            }

            public Common.Rectangle GetBoundingRect()
            {
                return _movable.GetRect();
            }

            public void SetBoundingRect(Common.Rectangle rect)
            {
                if (!GetBoundingRect().Equals(rect))
                {
                    _history.AddAndExecuteCommand(new MoveShapeCommand(_movable, rect));
                }
            }
        }

        private readonly Canvas _canvas;
        private readonly History _history;
        private readonly HistoryCanvas _historyCanvas;

        public Document(Canvas canvas)
        {
            _canvas = canvas;
            _history = new History();
            _historyCanvas = new HistoryCanvas(_canvas, (ICommand command) => {
                _history.AddAndExecuteCommand(command);
            });
            _canvas.LayoutUpdatedEvent += new Canvas.LayoutUpdatedDelegate(() => {
                LayoutUpdatedEvent();
            });
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _historyCanvas.AddShape(type, rect);
        }

        public IShape GetShape(int index)
        {
            return new Shape(_canvas, index, _history);
        }

        public void RemoveShape(int index)
        {
            _historyCanvas.RemoveShape(index);
        }

        public Common.Size CanvasSize
        {
            get
            {
                return _canvas.CanvasSize;
            }
        }

        public int ShapeCount
        {
            get
            {
                return _canvas.ShapeCount;
            }
        }

        public void Undo()
        {
            _history.Undo();
            LayoutUpdatedEvent();
        }

        public void Redo()
        {
            _history.Redo();
            LayoutUpdatedEvent();
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
