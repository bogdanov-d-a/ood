using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel.Command
{
    interface IShapes
    {
        void InsertShape(int index, Common.Shape shape);
        Common.Shape RemoveShapeAt(int index);
        int ShapeCount { get; }
    }
}
