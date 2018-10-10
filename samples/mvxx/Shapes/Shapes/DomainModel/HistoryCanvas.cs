using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class HistoryCanvas
    {
        private readonly Canvas canvas;
        private readonly History history;

        public HistoryCanvas(Canvas canvas)
        {
            this.canvas = canvas;
            canvas.LayoutUpdatedEvent += new Canvas.LayoutUpdatedDelegate(() => {
                LayoutUpdatedEvent();
            });
            history = new History(canvas);
        }

        public void AddShape(ShapeTypes.Type type, Common.Rectangle boundingRect)
        {
            history.AddAndExecuteCommand(new InsertShapeCommand(canvas, type, boundingRect));
        }

        public void Undo()
        {
            history.Undo();
        }

        public void Redo()
        {
            history.Redo();
        }

        public ShapeTypes.IShape GetShape(int index)
        {
            return canvas.GetShape(index);
        }

        public void ResetShapeRectangle(int index, Common.Rectangle rectangle)
        {
            history.AddAndExecuteCommand(new MoveShapeCommand(canvas, index, rectangle));
        }

        public void RemoveShape(int index)
        {
            history.AddAndExecuteCommand(new RemoveShapeCommand(canvas, index));
        }

        public Common.Size CanvasSize
        {
            get
            {
                return canvas.CanvasSize;
            }
        }

        public int ShapeCount
        {
            get
            {
                return canvas.ShapeCount;
            }
        }

        public delegate void LayoutUpdatedDelegate();
        public event LayoutUpdatedDelegate LayoutUpdatedEvent;
    }
}
