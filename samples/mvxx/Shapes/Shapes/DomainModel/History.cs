using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shapes.DomainModel
{
    class History
    {
        private readonly LinkedList<ICommand> list = new LinkedList<ICommand>();
        private LinkedListNode<ICommand> lastExecuted = null;

        private void RemoveTail()
        {
            if (lastExecuted == null)
            {
                list.Clear();
                return;
            }
            while (lastExecuted != list.Last)
            {
                list.RemoveLast();
            }
        }

        public void AddAndExecuteCommand(ICommand command)
        {
            command.Execute();
            RemoveTail();
            list.AddLast(command);
            lastExecuted = list.Last;
        }

        public bool Undo()
        {
            if (lastExecuted == null)
            {
                return false;
            }
            lastExecuted.Value.Unexecute();
            if (lastExecuted == list.First)
            {
                lastExecuted = null;
            }
            else
            {
                lastExecuted = lastExecuted.Previous;
            }
            return true;
        }

        public bool Redo()
        {
            if (lastExecuted == null)
            {
                if (list.Count > 0)
                {
                    list.First.Value.Execute();
                    lastExecuted = list.First;
                    return true;
                }
                return false;
            }
            if (lastExecuted == list.Last)
            {
                return false;
            }
            lastExecuted.Next.Value.Execute();
            lastExecuted = lastExecuted.Next;
            return true;
        }
    }
}
