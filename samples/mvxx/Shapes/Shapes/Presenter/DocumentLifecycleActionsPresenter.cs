using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Presenter
{
    public class DocumentLifecycleActionsPresenter
    {
        public DocumentLifecycleActionsPresenter(Common.IDocumentLifecycle model, View.DocumentLifecycleActionsView view)
        {
            view.OnNew += () => {
                model.New();
            };
            view.OnOpen += () => {
                model.Open();
            };
            view.OnSave += () => {
                model.Save();
            };
            view.OnSaveAs += () => {
                model.SaveAs();
            };
        }
    }
}
