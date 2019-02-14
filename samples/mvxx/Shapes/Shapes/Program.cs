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

            DomainModel.DocumentKeeper documentKeeper = new DomainModel.DocumentKeeper();
            documentKeeper.ResetData();

            AppModel.AppModel appModel = new AppModel.AppModel(documentKeeper);

            View.CanvasView canvasView = new View.CanvasView();
            View.ShapeActionsView shapeActionsView = new View.ShapeActionsView();
            View.MouseEventsView mouseEventsView = new View.MouseEventsView();
            View.UndoRedoActionsView undoRedoActionsView = new View.UndoRedoActionsView();
            View.DocumentLifecycleActionsView documentLifecycleActionsView = new View.DocumentLifecycleActionsView();

            new Presenter.CanvasPresenter(documentKeeper, appModel, canvasView);
            new Presenter.ShapeActionsPresenter(documentKeeper, appModel, shapeActionsView);
            new Presenter.MouseEventsPresenter(appModel.Pointer, mouseEventsView);
            new Presenter.UndoRedoActionsPresenter(appModel.History, undoRedoActionsView);
            new Presenter.DocumentLifecycleActionsPresenter(documentKeeper.DocumentLifecycle, documentLifecycleActionsView);

            Shapes shapesForm = new Shapes(canvasView, shapeActionsView, mouseEventsView, undoRedoActionsView, documentLifecycleActionsView);

            documentKeeper.SetLifecycleDecisionEvents(new Presenter.LifecycleDecisionPresenter(shapesForm.DialogsView).EventHandlers);
            shapesForm.SetFormClosingHandler(() => documentKeeper.DocumentLifecycle.New());

            Application.Run(shapesForm);
        }
    }
}
