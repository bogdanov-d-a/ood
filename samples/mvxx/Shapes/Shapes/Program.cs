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

            DomainModel.DocumentKeeper keeper = new DomainModel.DocumentKeeper();
            keeper.ResetData();

            AppModel.AppModel appModel = new AppModel.AppModel(keeper);

            View.CanvasView canvasView = new View.CanvasView();
            View.ShapeActionsView shapeActionsView = new View.ShapeActionsView();
            View.MouseEventsView mouseEventsView = new View.MouseEventsView();
            View.UndoRedoActionsView undoRedoActionsView = new View.UndoRedoActionsView();
            View.DocumentLifecycleActionsView documentLifecycleActionsView = new View.DocumentLifecycleActionsView();

            new Presenter.CanvasPresenter(keeper, appModel, canvasView);
            new Presenter.ShapeActionsPresenter(keeper, appModel, shapeActionsView);
            new Presenter.MouseEventsPresenter(appModel.Pointer, mouseEventsView);
            new Presenter.UndoRedoActionsPresenter(appModel.History, undoRedoActionsView);
            new Presenter.DocumentLifecycleActionsPresenter(keeper.DocumentLifecycle, documentLifecycleActionsView);

            Shapes shapes = new Shapes(canvasView, shapeActionsView, mouseEventsView, undoRedoActionsView, documentLifecycleActionsView);

            keeper.SetLifecycleDecisionEvents(new Presenter.LifecycleDecisionPresenter(shapes.DialogsView).EventHandlers);
            shapes.SetFormClosingHandler(() => keeper.DocumentLifecycle.New());

            Application.Run(shapes);
        }
    }
}
