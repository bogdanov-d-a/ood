using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public class HistoryCanvas
    {
        public delegate void AddCommandHandler(ICommand command);

        private readonly Canvas _canvas;
        private readonly AddCommandHandler _addCommandHandler;

        public HistoryCanvas(Canvas canvas, AddCommandHandler addCommandHandler)
        {
            _canvas = canvas;
            _addCommandHandler = addCommandHandler;
            _canvas.LayoutUpdatedEvent += new Canvas.LayoutUpdatedDelegate(() => {
                LayoutUpdatedEvent();
            });
        }

        public void AddShape(ShapeTypes.Type type, Common.Rectangle boundingRect)
        {
            _addCommandHandler(new InsertShapeCommand(_canvas, type, boundingRect));
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
            _addCommandHandler(new MoveShapeCommand(_canvas, index, rectangle));
        }

        public void RemoveShape(int index)
        {
            _addCommandHandler(new RemoveShapeCommand(_canvas, index));
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
