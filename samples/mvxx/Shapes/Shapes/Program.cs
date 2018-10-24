using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Shapes
{
    static class Program
    {
        private class Drawable : Shapes.IDrawable
        {
            private readonly ShapeTypes.AbstractShape _shape;
            private readonly Common.Rectangle _rect;

            public Drawable(ShapeTypes.AbstractShape shape, Common.Rectangle rect)
            {
                _shape = shape;
                _rect = rect;
            }

            public void Draw(Shapes.IRenderTarget target)
            {
                _shape.Draw(target, _rect);
            }
        }

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
            DomainModel.Document document = new DomainModel.Document(canvas);
            AppModel.AppModel appModel = new AppModel.AppModel(
                document,
                (int index, Common.Position pos) => {
                    return shapeList.GetAt(index).HasPointInside(pos);
                });

            Shapes view = new Shapes(new Shapes.RequestPaintingDelegate((Shapes.DrawableDelegate drawableDelegate, Shapes.SelectionDelegate selectionDelegate) => {
                for (int i = 0; i < appModel.ShapeCount; ++i)
                {
                    drawableDelegate(new Drawable(shapeList.GetAt(i), appModel.GetShapeBoundingRect(i)));
                }

                int selIndex = appModel.GetSelectedIndex();
                if (selIndex != -1)
                {
                    selectionDelegate(appModel.GetShapeBoundingRect(selIndex));
                }
            }));
            Presenter presenter = new Presenter(document, appModel, view);

            Application.Run(view);
        }
    }
}
