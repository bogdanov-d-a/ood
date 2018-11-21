using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.ShapeTypes
{
    public class CanvasShapeList : DomainModel.Canvas.IShapeList
    {
        private class Shape : DomainModel.Canvas.IShape
        {
            private readonly AbstractShape _shape;

            public Shape(AbstractShape shape)
            {
                _shape = shape;
            }

            public Common.Rectangle GetBoundingRect()
            {
                return _shape.GetBoundingRect();
            }

            public Common.ShapeType GetShapeType()
            {
                return _shape.GetShapeType();
            }

            public void SetBoundingRect(Common.Rectangle rect)
            {
                _shape.SetBoundingRect(rect);
            }

            public AbstractShape GetAbstractShape()
            {
                return _shape;
            }
        }

        private readonly AbstractShapeList _list;

        public CanvasShapeList(AbstractShapeList list)
        {
            _list = list;
        }

        public DomainModel.Canvas.IShape GetAt(int index)
        {
            return new Shape(_list.GetAt(index));
        }

        public int GetCount()
        {
            return _list.GetCount();
        }

        public void Insert(int index, Common.ShapeType type, Common.Rectangle rect)
        {
            _list.InsertShape(index, type, rect);
        }

        public void ReInsert(int index, DomainModel.Canvas.IShape shape)
        {
            _list.ReInsertShape(index, ((Shape)shape).GetAbstractShape());
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }
    }
}
