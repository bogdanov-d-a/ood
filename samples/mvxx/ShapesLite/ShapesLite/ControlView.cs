using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite
{
    public class ControlView
    {
        public delegate void VoidDelegate();
        public VoidDelegate ResetPositionEvent;
    }
}
