using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shapes.ShapeTypes
{
    public interface IShape
    {
        int GetTypeId();
        Common.Rectangle GetBoundingRect();
        void SetBoundingRect(Common.Rectangle rect);
        IShape Clone();
    }
}
