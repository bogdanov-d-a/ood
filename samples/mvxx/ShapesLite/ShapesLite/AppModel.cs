using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optional;

namespace ShapesLite
{
    using RectangleD = Common.Rectangle<double>;
    using SignallingInt = Common.SignallingValue<int>;

    public class AppModel
    {
        public readonly SignallingInt SelectedShapeIndex = new SignallingInt(-1);
        public Option<RectangleD> ActualSelectedShape = Option.None<RectangleD>();

        private readonly DomainModel _domainModel;

        public AppModel(DomainModel domainModel)
        {
            _domainModel = domainModel;

            _domainModel.ShapeList.AfterInsertEvent += (int index, RectangleD value) => AfterShapeInsertEvent(index);
            _domainModel.ShapeList.AfterSetEvent += (int index, RectangleD value) => AfterShapeSetEvent(index);

            _domainModel.ShapeList.BeforeRemoveEvent += (int index, RectangleD value) =>
            {
                if (SelectedShapeIndex.Value > index)
                {
                    SelectedShapeIndex.Value = SelectedShapeIndex.Value - 1;
                }
                else if (SelectedShapeIndex.Value == index)
                {
                    SelectedShapeIndex.Value = -1;
                }
                BeforeShapeRemoveEvent(index);
            };
        }

        public void InsertShape(int index, RectangleD shape)
        {
            _domainModel.ShapeList.Insert(index, shape);
        }

        public RectangleD GetShapeAt(int index)
        {
            return _domainModel.ShapeList.GetAt(index);
        }

        public void SetShapeAt(int index, RectangleD shape)
        {
            _domainModel.ShapeList.SetAt(index, shape);
        }

        public void RemoveShapeAt(int index)
        {
            _domainModel.ShapeList.RemoveAt(index);
        }

        public int ShapeCount
        {
            get => _domainModel.ShapeList.Count;
        }

        public delegate void IndexDelegate(int index);
        public event IndexDelegate AfterShapeInsertEvent = delegate {};
        public event IndexDelegate AfterShapeSetEvent = delegate {};
        public event IndexDelegate BeforeShapeRemoveEvent = delegate {};
    }
}
