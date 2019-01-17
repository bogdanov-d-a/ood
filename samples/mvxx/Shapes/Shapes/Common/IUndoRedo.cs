using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.Common
{
    public interface IUndoRedo
    {
        void Undo();
        void Redo();
    }
}
