using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel.Command
{
    interface IShapes
    {
        void InsertShape(int index, Common.Shape shape);
        void RemoveShapeAt(int index);
        Common.Shape GetShapeAt(int index);
        int ShapeCount { get; }
    }
}
