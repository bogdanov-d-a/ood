using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public class AbstractShapeList
    {
        private readonly List<AbstractShape> _shapeList = new List<AbstractShape>();
        private readonly AbstractShapeFactory _factory = new AbstractShapeFactory();

        public void InsertShape(int index, Common.ShapeType type, Common.Rectangle rect)
        {
            _shapeList.Insert(index, _factory.CreateShape(type, rect));
        }

        public void RemoveAt(int index)
        {
            _shapeList.RemoveAt(index);
        }

        public int GetCount()
        {
            return _shapeList.Count;
        }

        public AbstractShape GetAt(int index)
        {
            return _shapeList.ElementAt(index);
        }
    }
}
