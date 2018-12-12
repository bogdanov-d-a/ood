using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel.Command
{
    public interface ICommand
    {
        void Execute();
        void Unexecute();
    }
}
