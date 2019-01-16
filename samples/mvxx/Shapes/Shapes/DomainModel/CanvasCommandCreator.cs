using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    class CanvasCommandCreator
    {
        public interface IShapeEvents
        {
            void OnInsert(int index);
            void OnRemove(int index);
            void OnMove(int index);
        }

        public interface IEvents
        {
            void AddCommand(Command.ICommand command);
            IShapeEvents GetShapeEvents();
        }

        private class CommandShapes : Command.IShapes
        {
            private readonly Canvas _canvas;
            private readonly IShapeEvents _shapeEvents;

            public CommandShapes(Canvas canvas, IShapeEvents shapeEvents)
            {
                _canvas = canvas;
                _shapeEvents = shapeEvents;
            }

            public int ShapeCount => _canvas.ShapeCount;

            public void InsertShape(int index, Common.Shape shape)
            {
                _canvas.InsertShape(index, shape);
                _shapeEvents.OnInsert(index);
            }

            public Common.Shape RemoveShapeAt(int index)
            {
                Common.Shape result = _canvas.GetShapeAt(index);
                _canvas.RemoveShapeAt(index);
                _shapeEvents.OnRemove(index);
                return result;
            }
        }

        private class Movable : Command.MoveShapeCommand.IMovable
        {
            private readonly Canvas _canvas;
            private readonly IShapeEvents _shapeEvents;
            private readonly int _index;

            public Common.Rectangle Rect
            {
                get => GetShape().boundingRect;
                set {
                    GetShape().boundingRect = value;
                    _shapeEvents.OnMove(_index);
                }
            }

            public Movable(Canvas canvas, IShapeEvents shapeEvents, int index)
            {
                _canvas = canvas;
                _shapeEvents = shapeEvents;
                _index = index;
            }

            private Common.Shape GetShape()
            {
                return _canvas.GetShapeAt(_index);
            }
        }

        private readonly Canvas _canvas;
        private readonly IEvents _events;
        private readonly CommandShapes _commandShapes;

        public CanvasCommandCreator(Canvas canvas, IEvents events)
        {
            _canvas = canvas;
            _events = events;
            _commandShapes = new CommandShapes(_canvas, _events.GetShapeEvents());
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _events.AddCommand(new Command.InsertShapeCommand(_commandShapes, new Common.Shape(type, rect)));
        }

        public void RemoveShape(int index)
        {
            _events.AddCommand(new Command.RemoveShapeCommand(_commandShapes, index));
        }

        public void MoveShape(int index, Common.Rectangle newRect)
        {
            _events.AddCommand(new Command.MoveShapeCommand(new Movable(_canvas, _events.GetShapeEvents(), index), newRect));
        }
    }
}
