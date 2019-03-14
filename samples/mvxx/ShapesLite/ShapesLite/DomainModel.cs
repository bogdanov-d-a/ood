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
        private readonly List<RectangleD> _shapeList = new List<RectangleD>();

        public void InsertShape(int index, RectangleD shape)
        {
            _shapeList.Insert(index, shape);
            AfterShapeInsertEvent(index);
        }

        public RectangleD GetShapeAt(int index)
        {
            return _shapeList.ElementAt(index);
        }

        public void SetShapeAt(int index, RectangleD shape)
        {
            if (GetShapeAt(index).Equals(shape))
            {
                return;
            }
            _shapeList[index] = shape;
            AfterShapeSetEvent(index);
        }

        public void RemoveShapeAt(int index)
        {
            BeforeShapeRemoveEvent(index);
            _shapeList.RemoveAt(index);
        }

        public int ShapeCount
        {
            get => _shapeList.Count;
        }

        public delegate void IndexDelegate(int index);
        public event IndexDelegate AfterShapeInsertEvent = delegate {};
        public event IndexDelegate AfterShapeSetEvent = delegate {};
        public event IndexDelegate BeforeShapeRemoveEvent = delegate {};
    }
}
