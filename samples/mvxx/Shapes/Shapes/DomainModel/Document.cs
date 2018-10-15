using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class Document
    {
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

        public void AddShape(ShapeTypes.Type type, Common.Rectangle boundingRect)
        {
            _historyCanvas.AddShape(type, boundingRect);
        }

        public ShapeTypes.IShape GetShape(int index)
        {
            return _canvas.GetShape(index);
        }

        public void ResetShapeRectangle(int index, Common.Rectangle rectangle)
        {
            _historyCanvas.ResetShapeRectangle(index, rectangle);
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
        }

        public void Redo()
        {
            _history.Redo();
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
