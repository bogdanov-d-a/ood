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
            DomainModel.Canvas canvas = new DomainModel.Canvas(new Common.Size(640, 480));
            AppModel.AppModel presenter = new AppModel.AppModel(canvas);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Shapes(presenter));
        }
    }
}
