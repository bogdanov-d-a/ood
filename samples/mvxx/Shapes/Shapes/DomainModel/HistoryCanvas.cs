using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class HistoryCanvas
    {
        public interface IDelegate
        {
            void AddCommand(Command.ICommand command);

            void OnInsertShape(int index);
            void OnRemoveShape(int index);
        }

        private class InsertionCanvas : Command.InsertShapeCommand.ICanvas
        {
            private readonly Canvas _canvas;
            private readonly IDelegate _delegate;

            public InsertionCanvas(Canvas canvas, IDelegate delegate_)
            {
                _canvas = canvas;
                _delegate = delegate_;
            }

            public void Add(Common.Shape shape)
            {
                int index = _canvas.ShapeCount;
                _canvas.InsertShape(index, shape);
                _delegate.OnInsertShape(index);
            }

            public Common.Shape Get()
            {
                return _canvas.GetShape(_canvas.ShapeCount - 1);
            }

            public void Remove()
            {
                int index = _canvas.ShapeCount - 1;
                _canvas.RemoveShape(index);
                _delegate.OnRemoveShape(index);
            }
        }

        private class DeletionCanvas : Command.RemoveShapeCommand.ICanvas
        {
            private readonly Canvas _canvas;
            private readonly IDelegate _delegate;

            public DeletionCanvas(Canvas canvas, IDelegate delegate_)
            {
                _canvas = canvas;
                _delegate = delegate_;
            }

            public Common.Shape GetAt(int index)
            {
                return _canvas.GetShape(index);
            }

            public void Insert(int index, Common.Shape shape)
            {
                _canvas.InsertShape(index, shape);
                _delegate.OnInsertShape(index);
            }

            public void RemoveAt(int index)
            {
                _canvas.RemoveShape(index);
                _delegate.OnRemoveShape(index);
            }
        }

        private readonly IDelegate _delegate;
        private readonly InsertionCanvas _insertionCanvas;
        private readonly DeletionCanvas _deletionCanvas;

        public HistoryCanvas(Canvas canvas, IDelegate delegate_)
        {
            _delegate = delegate_;
            _insertionCanvas = new InsertionCanvas(canvas, _delegate);
            _deletionCanvas = new DeletionCanvas(canvas, _delegate);
        }

        public void AddShape(Common.ShapeType type, Common.Rectangle rect)
        {
            _delegate.AddCommand(new Command.InsertShapeCommand(_insertionCanvas, type, rect));
        }

        public void RemoveShape(int index)
        {
            _delegate.AddCommand(new Command.RemoveShapeCommand(_deletionCanvas, index));
        }
    }
}
