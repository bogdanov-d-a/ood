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
            DomainModel.Document document = new DomainModel.Document(canvas);
            AppModel.AppModel appModel = new AppModel.AppModel(document, new ShapeTypes.ShapeFactory());

            Shapes view = new Shapes(new Shapes.ShapeEnumeratorDelegate((Shapes.ShapeInfoDelegate shapeDelegate, Shapes.SelectionInfoDelegate selectionDelegate) => {
                for (int i = 0; i < appModel.ShapeCount; ++i)
                {
                    var shape = appModel.GetShape(i);
                    shapeDelegate((ShapeTypes.IRenderShape)shape);
                }

                int selIndex = appModel.GetSelectedIndex();
                if (selIndex != -1)
                {
                    selectionDelegate(appModel.GetShape(selIndex).GetBoundingRect());
                }
            }));
            Presenter presenter = new Presenter(document, appModel, view);

            Application.Run(view);
        }
    }
}
