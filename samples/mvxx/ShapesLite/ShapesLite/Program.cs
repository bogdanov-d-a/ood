﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapesLite
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DomainModel domainModel = new DomainModel();
            AppModel appModel = new AppModel();
            View view = new View();
            InfoView infoView = new InfoView();
            new Presenter(domainModel, appModel, view);
            new InfoPresenter(domainModel, appModel, infoView);

            domainModel.ShapeBoundingRect.Value = new Common.RectangleDouble(0.25, 0.25, 0.5, 0.5);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ShapesLiteForm(view, infoView));
        }
    }
}
