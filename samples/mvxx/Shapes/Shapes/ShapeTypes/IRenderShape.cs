using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shapes.ShapeTypes
{
    public interface IRenderShape : IShape
    {
        void Draw(IRenderTarget target);
    }
}
