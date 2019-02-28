using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Presenters
{
    public class ControlPresenter
    {
        public ControlPresenter(DomainModel domainModel, AppModel appModel, ControlView controlView)
        {
            controlView.ResetPositionEvent += () => {
                domainModel.ShapeBoundingRect.Value = new Common.RectangleDouble(0.25, 0.25, 0.5, 0.5);
            };
            controlView.FlipSelectionEvent += () => {
                appModel.IsShapeSelected.Value = !appModel.IsShapeSelected.Value;
            };
        }
    }
}
