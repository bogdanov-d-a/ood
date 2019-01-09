using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Utils
{
    public delegate void CanvasBuilder(Common.ShapeType type, Common.Rectangle boundingRect);

    public interface ICanvasLoader
    {
        void LoadShapes(CanvasBuilder canvasBuilder);
    }
}
