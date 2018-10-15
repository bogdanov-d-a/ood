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
        }

        public void AddShape(ShapeTypes.Type type, Common.Rectangle boundingRect)
        {
            _addCommandHandler(new InsertShapeCommand(_canvas, type, boundingRect));
        }

        public void ResetShapeRectangle(int index, Common.Rectangle rectangle)
        {
            if (_canvas.GetShape(index).GetBoundingRect().Equals(rectangle))
            {
                return;
            }
            _addCommandHandler(new MoveShapeCommand(_canvas, index, rectangle));
        }

        public void RemoveShape(int index)
        {
            _addCommandHandler(new RemoveShapeCommand(_canvas, index));
        }
    }
}
