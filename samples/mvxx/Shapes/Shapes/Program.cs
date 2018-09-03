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
            AppModel.AppModel appModel = new AppModel.AppModel(canvas);

            Shapes view = new Shapes();
            Presenter presenter = new Presenter(appModel, view);

            Application.Run(view);
        }
    }
}
