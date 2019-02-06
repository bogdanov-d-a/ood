using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Presenter
{
    public class MouseEventsPresenter
    {
        public MouseEventsPresenter(Common.IPointerDrag model, View.MouseEventsView view)
        {
            view.OnDown += (Common.Position pos) => {
                model.BeginMove(pos);
            };
            view.OnUp += (Common.Position pos) => {
                model.EndMove(pos);
            };
            view.OnMove += (Common.Position pos) => {
                model.Move(pos);
            };
        }
    }
}
