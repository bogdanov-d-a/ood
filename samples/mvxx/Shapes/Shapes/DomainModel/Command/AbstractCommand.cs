using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel.Command
{
    public abstract class AbstractCommand : ICommand
    {
        private bool _isExecuted = false;

        public void Execute()
        {
            if (_isExecuted)
            {
                throw new Exception();
            }
            ExecuteImpl();
            _isExecuted = true;
        }

        public void Unexecute()
        {
            if (!_isExecuted)
            {
                throw new Exception();
            }
            UnexecuteImpl();
            _isExecuted = false;
        }

        protected abstract void ExecuteImpl();
        protected abstract void UnexecuteImpl();
    }
}
