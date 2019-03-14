using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Presenters
{
    public class InfoPresenter
    {
        public InfoPresenter(AppModel appModel, Views.InfoView infoView)
        {
            appModel.DomainShapeBoundingRect.Event += (Common.Rectangle<double> rect) => {
                infoView.DomainModelRect.Value = rect;
            };

            appModel.ShapeBoundingRect.Event += (Common.Rectangle<double> rect) => {
                infoView.AppModelRect.Value = rect;
            };
        }
    }
}
