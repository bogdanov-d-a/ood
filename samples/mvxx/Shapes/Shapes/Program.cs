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
            View.MouseEventsView mouseEventsView = new View.MouseEventsView();
            View.UndoRedoActionsView undoRedoActionsView = new View.UndoRedoActionsView();
            View.DocumentLifecycleActionsView documentLifecycleActionsView = new View.DocumentLifecycleActionsView();

            new Presenter.Presenter(model, view);
            new Presenter.ShapeActionsPresenter(model, shapeActionsView);
            new Presenter.MouseEventsPresenter(model.Pointer, mouseEventsView);
            new Presenter.UndoRedoActionsPresenter(model.History, undoRedoActionsView);
            new Presenter.DocumentLifecycleActionsPresenter(model.DocumentLifecycle, documentLifecycleActionsView);

            Application.Run(new Shapes(view, shapeActionsView, mouseEventsView, undoRedoActionsView, documentLifecycleActionsView));
        }
    }
}
