using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class InfoPresenter
    {
        public InfoPresenter(DomainModel domainModel, AppModel appModel, InfoView infoView)
        {
            domainModel.ShapeBoundingRect.Event += (Common.Rectangle<double> rect) => {
                infoView.DomainModelRect.Value = rect;
            };

            appModel.ShapeBoundingRect.Event += (Common.Rectangle<double> rect) => {
                infoView.AppModelRect.Value = rect;
            };
        }
    }
}
