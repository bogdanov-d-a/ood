using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.View
{
    public class MouseEventsView
    {
        public delegate void PositionDelegate(Common.Position pos);

        public PositionDelegate OnDown;
        public PositionDelegate OnUp;
        public PositionDelegate OnMove;
    }
}
