using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public interface IRenderTarget
    {
        void DrawRectangle(Common.Rectangle rect);
        void DrawTriangle(Common.Rectangle rect);
        void DrawCircle(Common.Rectangle rect);
    }
}
