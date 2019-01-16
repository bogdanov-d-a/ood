using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public interface IShape
    {
        Common.ShapeType ShapeType { get; }
        Common.Rectangle BoundingRect { get; set; }
    }
}
