using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class ControlPresenter
    {
        public ControlPresenter(DomainModel domainModel, AppModel appModel, ControlView controlView)
        {
            controlView.ResetPositionEvent += () => {
                domainModel.ShapeBoundingRect.Value = new Common.RectangleDouble(0.25, 0.25, 0.5, 0.5);
            };
        }
    }
}
