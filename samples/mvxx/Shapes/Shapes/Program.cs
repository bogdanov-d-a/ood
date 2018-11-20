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

            ShapeTypes.AbstractShapeList shapeList = new ShapeTypes.AbstractShapeList();
            DomainModel.Canvas canvas = new DomainModel.Canvas(new Common.Size(640, 480), new ShapeTypes.CanvasShapeList(shapeList));
            Presenter.DocumentDelegateProxy documentDelegateProxy = new Presenter.DocumentDelegateProxy();
            DomainModel.Document document = new DomainModel.Document(documentDelegateProxy, canvas);
            AppModel.AppModel appModel = new AppModel.AppModel(
                document,
                (int index, Common.Position pos) => {
                    return shapeList.GetAt(index).HasPointInside(pos);
                });

            CanvasView canvasView = new CanvasView();
            Presenter presenter = new Presenter(document, documentDelegateProxy, appModel, shapeList, canvasView);

            Application.Run(new Shapes(canvasView));
        }
    }
}
