using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.View
{
    public class ShapeActionsView
    {
        public delegate void VoidDelegate();

        public VoidDelegate OnAddRectangle;
        public VoidDelegate OnAddTriangle;
        public VoidDelegate OnAddCircle;
        public VoidDelegate OnRemove;
    }
}
