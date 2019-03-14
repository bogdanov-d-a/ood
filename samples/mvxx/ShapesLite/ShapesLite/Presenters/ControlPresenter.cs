using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Presenters
{
    public class ControlPresenter
    {
        public ControlPresenter(AppModel appModel, Views.ControlView controlView)
        {
            controlView.AddShapeEvent += () => {
                appModel.InsertShape(appModel.ShapeCount, new Common.RectangleDouble(0.25, 0.25, 0.5, 0.5));
            };
        }
    }
}
