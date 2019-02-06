using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.View
{
    public class UndoRedoActionsView
    {
        public delegate void VoidDelegate();

        public VoidDelegate OnUndo;
        public VoidDelegate OnRedo;
    }
}
