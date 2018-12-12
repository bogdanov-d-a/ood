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

            public void Add(Canvas.Shape shape)
            {
                _canvas.InsertShape(_canvas.ShapeCount, shape);
            }

            public Canvas.Shape Get()
            {
                return _canvas.GetShape(_canvas.ShapeCount - 1);
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

            public Canvas.Shape GetAt(int index)
            {
                return _canvas.GetShape(index);
            }

            public void Insert(int index, Canvas.Shape shape)
            {
                _canvas.InsertShape(index, shape);
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
