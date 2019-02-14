using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Presenter
{
    public class UndoRedoActionsPresenter
    {
        public UndoRedoActionsPresenter(Common.IUndoRedo model, View.UndoRedoActionsView view)
        {
            view.OnUndo += () => model.Undo();
            view.OnRedo += () => model.Redo();
        }
    }
}
