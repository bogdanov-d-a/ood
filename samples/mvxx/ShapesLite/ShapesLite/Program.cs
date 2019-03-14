using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapesLite
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DomainModel domainModel = new DomainModel();
            AppModel appModel = new AppModel(domainModel);
            Views.CanvasView canvasView = new Views.CanvasView();
            Views.InfoView infoView = new Views.InfoView();
            Views.ControlView controlView = new Views.ControlView();
            new Presenters.CanvasPresenter(domainModel, appModel, canvasView);
            new Presenters.InfoPresenter(appModel, infoView);
            new Presenters.ControlPresenter(domainModel, appModel, controlView);

            domainModel.ShapeBoundingRect.Value = new Common.RectangleDouble(0.25, 0.25, 0.5, 0.5);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ShapesLiteForm(canvasView, infoView, controlView));
        }
    }
}
