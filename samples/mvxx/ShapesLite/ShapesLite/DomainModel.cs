using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    using RectangleD = Common.Rectangle<double>;

    public class DomainModel
    {
        public readonly Common.SignallingList<RectangleD> ShapeList
            = new Common.SignallingList<RectangleD>();
    }
}
