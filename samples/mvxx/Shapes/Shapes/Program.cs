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

            AppModel.Facade model = new AppModel.Facade();

            View.CanvasView view = new View.CanvasView();
            View.ShapeActionsView shapeActionsView = new View.ShapeActionsView();

            new Presenter.Presenter(model, view);
            new Presenter.ShapeActionsPresenter(model, shapeActionsView);

            Application.Run(new Shapes(view, shapeActionsView));
        }
    }
}
