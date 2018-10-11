using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public interface IShape
    {
        Common.Rectangle GetBoundingRect();
        void SetBoundingRect(Common.Rectangle rect);
        bool HasPointInside(Common.Position pos);
        IShape Clone();
    }
}
