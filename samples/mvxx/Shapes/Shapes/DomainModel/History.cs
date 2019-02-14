using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    class History
    {
        private readonly LinkedList<Command.ICommand> _list = new LinkedList<Command.ICommand>();
        private LinkedListNode<Command.ICommand> _lastExecuted = null;

        private void RemoveTail()
        {
            if (_lastExecuted == null)
            {
                _list.Clear();
                return;
            }
            while (_lastExecuted != _list.Last)
            {
                _list.RemoveLast();
            }
        }

        public void AddAndExecuteCommand(Command.ICommand command)
        {
            command.Execute();
            RemoveTail();
            _list.AddLast(command);
            _lastExecuted = _list.Last;
        }

        public bool Undo()
        {
            if (_lastExecuted == null)
            {
                return false;
            }
            _lastExecuted.Value.Unexecute();
            if (_lastExecuted == _list.First)
            {
                _lastExecuted = null;
            }
            else
            {
                _lastExecuted = _lastExecuted.Previous;
            }
            return true;
        }

        public bool Redo()
        {
            if (_lastExecuted == null)
            {
                if (_list.Count > 0)
                {
                    _list.First.Value.Execute();
                    _lastExecuted = _list.First;
                    return true;
                }
                return false;
            }
            if (_lastExecuted == _list.Last)
            {
                return false;
            }
            _lastExecuted.Next.Value.Execute();
            _lastExecuted = _lastExecuted.Next;
            return true;
        }

        public Command.ICommand GetLastExecuted()
        {
            return _lastExecuted == null
                ? null
                : _lastExecuted.Value;
        }
    }
}
