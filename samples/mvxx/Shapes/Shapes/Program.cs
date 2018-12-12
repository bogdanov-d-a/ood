using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Shapes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DomainModel.Canvas canvas = new DomainModel.Canvas(new Common.Size(640, 480));
            Presenter.DocumentDelegateProxy documentDelegateProxy = new Presenter.DocumentDelegateProxy();
            DomainModel.Document document = new DomainModel.Document(documentDelegateProxy, canvas);
            AppModel.AppModel appModel = new AppModel.AppModel(document);

            View.CanvasView canvasView = new View.CanvasView();
            View.CanvasViewData canvasViewData = new View.CanvasViewData();
            Presenter presenter = new Presenter(document, documentDelegateProxy, appModel, canvasView, canvasViewData);

            Application.Run(new Shapes(canvasView, canvasViewData));
        }
    }
}
