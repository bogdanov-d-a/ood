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
            Views.ControlView controlView = new Views.ControlView();
            new Presenters.CanvasPresenter(appModel, canvasView);
            new Presenters.ControlPresenter(appModel, controlView);

            domainModel.InsertShape(0, new Common.RectangleDouble(0.25, 0.25, 0.5, 0.5));
            domainModel.InsertShape(1, new Common.RectangleDouble(0.75, 0.75, 0.25, 0.25));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ShapesLiteForm(canvasView, controlView));
        }
    }
}
