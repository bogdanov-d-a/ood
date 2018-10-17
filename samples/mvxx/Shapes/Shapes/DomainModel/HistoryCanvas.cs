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

        public void AddShape(ShapeTypes.IShape shape)
        {
            _addCommandHandler(new InsertShapeCommand(_canvas, shape));
        }

        public void RemoveShape(int index)
        {
            _addCommandHandler(new RemoveShapeCommand(_canvas, index));
        }
    }
}
