using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    class HistoryCanvas
    {
        public interface IDelegate
        {
            void AddCommand(Command.ICommand command);

            void OnInsertShape(int index);
            void OnRemoveShape(int index);
            void OnMoveShape(int index);
        }

        private class CommandShapes : Command.IShapes
        {
            private readonly Canvas _canvas;
            private readonly IDelegate _delegate;

            public CommandShapes(Canvas canvas, IDelegate delegate_)
            {
                _canvas = canvas;
                _delegate = delegate_;
            }

            public int ShapeCount => _canvas.ShapeCount;

            public void InsertShape(int index, Common.Shape shape)
            {
                _canvas.InsertShape(index, shape);
                _delegate.OnInsertShape(index);
            }

            public Common.Shape RemoveShapeAt(int index)
            {
                Common.Shape result = _canvas.GetShape(index);
                _canvas.RemoveShape(index);
                _delegate.OnRemoveShape(index);
                return result;
            }
        }

        private class Movable : Command.MoveShapeCommand.IMovable
        {
            private readonly Canvas _canvas;
            private readonly IDelegate _delegate;
            private readonly int _index;

            public Common.Rectangle Rect
            {
                get => GetShape().boundingRect;
                set {
                    GetShape().boundingRect = value;
                    _delegate.OnMoveShape(_index);
                }
            }

            public Movable(Canvas canvas, IDelegate delegate_, int index)
            {
                _canvas = canvas;
                _delegate = delegate_;
                _index = index;
            }

            private Common.Shape GetShape()
            {
                return _canvas.GetShape(_index);
            }
        }

        private readonly Canvas _canvas;
        private readonly IDelegate _delegate;
        private readonly CommandShapes _commandShapes;

        public HistoryCanvas(Canvas canvas, IDelegate delegate_)
        {
            _canvas = canvas;
            _delegate = delegate_;
            _commandShapes = new CommandShapes(_canvas, _delegate);
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _delegate.AddCommand(new Command.InsertShapeCommand(_commandShapes, new Common.Shape(type, rect)));
        }

        public void RemoveShape(int index)
        {
            _delegate.AddCommand(new Command.RemoveShapeCommand(_commandShapes, index));
        }

        public void MoveShape(int index, Common.Rectangle newRect)
        {
            _delegate.AddCommand(new Command.MoveShapeCommand(new Movable(_canvas, _delegate, index), newRect));
        }
    }
}
