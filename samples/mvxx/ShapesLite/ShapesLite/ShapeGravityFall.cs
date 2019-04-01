using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    class ShapeGravityFall
    {
        private readonly AppModel _model;

        public ShapeGravityFall(AppModel model)
        {
            _model = model;
        }

        public void Tick(double offset)
        {
            for (int i = 0; i < _model.ShapeList.Count; ++i)
            {
                if (i != _model.SelectedShapeIndex.Value)
                {
                    Common.Rectangle<double> rect = _model.ShapeList.GetAt(i);
                    Common.Rectangle<double> newRect = new Common.RectangleDouble(
                        rect.Left, rect.Top + offset, rect.Width, rect.Height);
                    _model.ShapeList.SetAt(i, newRect);
                }
            }
        }
    }
}
