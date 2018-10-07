using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    public abstract class AbstractCommand : ICommand
    {
        private bool isExecuted = false;

        public void Execute()
        {
            if (isExecuted)
            {
                throw new Exception();
            }
            ExecuteImpl();
            isExecuted = true;
        }

        public void Unexecute()
        {
            if (!isExecuted)
            {
                throw new Exception();
            }
            UnexecuteImpl();
            isExecuted = false;
        }

        public abstract void ExecuteImpl();
        public abstract void UnexecuteImpl();
    }
}
