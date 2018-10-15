using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class HistoryCanvas
    {
        private readonly Canvas _canvas;
        private readonly History _history;

        public HistoryCanvas(Canvas canvas)
        {
            _canvas = canvas;
            _canvas.LayoutUpdatedEvent += new Canvas.LayoutUpdatedDelegate(() => {
                LayoutUpdatedEvent();
            });
            _history = new History();
        }

        public void AddShape(ShapeTypes.Type type, Common.Rectangle boundingRect)
        {
            _history.AddAndExecuteCommand(new InsertShapeCommand(_canvas, type, boundingRect));
        }

        public void Undo()
        {
            _history.Undo();
        }

        public void Redo()
        {
            _history.Redo();
        }

        public ShapeTypes.IShape GetShape(int index)
        {
            return _canvas.GetShape(index);
        }

        public void ResetShapeRectangle(int index, Common.Rectangle rectangle)
        {
            if (GetShape(index).GetBoundingRect().Equals(rectangle))
            {
                return;
            }
            _history.AddAndExecuteCommand(new MoveShapeCommand(_canvas, index, rectangle));
        }

        public void RemoveShape(int index)
        {
            _history.AddAndExecuteCommand(new RemoveShapeCommand(_canvas, index));
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

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
