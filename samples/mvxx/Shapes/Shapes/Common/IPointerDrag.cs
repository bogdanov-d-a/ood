using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public interface IPointerDrag
    {
        void BeginMove(Position pos);
        void EndMove(Position pos);
        void Move(Position pos);
    }
}
