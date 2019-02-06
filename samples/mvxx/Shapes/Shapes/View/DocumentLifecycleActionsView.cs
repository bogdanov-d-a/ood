using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.View
{
    public class DocumentLifecycleActionsView
    {
        public delegate void VoidDelegate();

        public VoidDelegate OnNew;
        public VoidDelegate OnOpen;
        public VoidDelegate OnSave;
        public VoidDelegate OnSaveAs;
    }
}
