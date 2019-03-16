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

            _domainModel.ShapeList.AfterInsertEvent += (int index, RectangleD value) =>
            {
                ActualSelectedShape = Option.None<RectangleD>();
            };
            _domainModel.ShapeList.AfterSetEvent += (int index, RectangleD value) =>
            {
                ActualSelectedShape = Option.None<RectangleD>();
            };
            _domainModel.ShapeList.BeforeRemoveEvent += (int index, RectangleD value) =>
            {
                ActualSelectedShape = Option.None<RectangleD>();
            };

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
            };
        }

        public Common.SignallingList<RectangleD> ShapeList
        {
            get => _domainModel.ShapeList;
        }
    }
}
