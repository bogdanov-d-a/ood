using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class HistoryCanvas
    {
        public delegate void AddCommandHandler(ICommand command);

        private class InsertionCanvas : InsertShapeCommand.ICanvas
        {
            private readonly Canvas _canvas;

            public InsertionCanvas(Canvas canvas)
            {
                _canvas = canvas;
            }

            public void Add(Common.ShapeType type, Common.Rectangle rect)
            {
                _canvas.AddShape(type, rect);
            }

            public void Remove()
            {
                _canvas.RemoveShape(_canvas.ShapeCount - 1);
            }
        }

        private class DeletionCanvas : RemoveShapeCommand.ICanvas
        {
            private readonly Canvas _canvas;

            public DeletionCanvas(Canvas canvas)
            {
                _canvas = canvas;
            }

            private class Shape : RemoveShapeCommand.IShape
            {
                private readonly Canvas.IShape _shape;

                public Shape(Canvas.IShape shape)
                {
                    _shape = shape;
                }

                public Common.Rectangle GetBoundingRect()
                {
                    return _shape.GetBoundingRect();
                }

                public Common.ShapeType GetShapeType()
                {
                    return _shape.GetShapeType();
                }
            }

            public RemoveShapeCommand.IShape GetAt(int index)
            {
                return new Shape(_canvas.GetShape(index));
            }

            public void Insert(int index, Common.ShapeType type, Common.Rectangle rect)
            {
                _canvas.InsertShape(index, type, rect);
            }

            public void RemoveAt(int index)
            {
                _canvas.RemoveShape(index);
            }
        }

        private readonly AddCommandHandler _addCommandHandler;
        private readonly InsertionCanvas _insertionCanvas;
        private readonly DeletionCanvas _deletionCanvas;

        public HistoryCanvas(Canvas canvas, AddCommandHandler addCommandHandler)
        {
            _addCommandHandler = addCommandHandler;
            _insertionCanvas = new InsertionCanvas(canvas);
            _deletionCanvas = new DeletionCanvas(canvas);
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _addCommandHandler(new InsertShapeCommand(_insertionCanvas, type, rect));
        }

        public void RemoveShape(int index)
        {
            _addCommandHandler(new RemoveShapeCommand(_deletionCanvas, index));
        }
    }
}
