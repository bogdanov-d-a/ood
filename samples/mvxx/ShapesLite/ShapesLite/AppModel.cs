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

            _domainModel.AfterShapeInsertEvent += (int index) => AfterShapeInsertEvent(index);
            _domainModel.AfterShapeSetEvent += (int index) => AfterShapeSetEvent(index);

            _domainModel.BeforeShapeRemoveEvent += (int index) =>
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
            _domainModel.InsertShape(index, shape);
        }

        public RectangleD GetShapeAt(int index)
        {
            return _domainModel.GetShapeAt(index);
        }

        public void SetShapeAt(int index, RectangleD shape)
        {
            _domainModel.SetShapeAt(index, shape);
        }

        public void RemoveShapeAt(int index)
        {
            _domainModel.RemoveShapeAt(index);
        }

        public int ShapeCount
        {
            get => _domainModel.ShapeCount;
        }

        public delegate void IndexDelegate(int index);
        public event IndexDelegate AfterShapeInsertEvent = delegate {};
        public event IndexDelegate AfterShapeSetEvent = delegate {};
        public event IndexDelegate BeforeShapeRemoveEvent = delegate {};
    }
}
