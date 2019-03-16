using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Common
{
    public class SignallingList<T> where T : IEquatable<T>
    {
        private readonly List<T> _list = new List<T>();

        public int Count
        {
            get => _list.Count;
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            AfterInsertEvent(index, item);
        }

        public void RemoveAt(int index)
        {
            BeforeRemoveEvent(index, _list[index]);
            _list.RemoveAt(index);
        }

        public T GetAt(int index)
        {
            return _list[index];
        }

        public void SetAt(int index, T value)
        {
            if (GetAt(index).Equals(value))
            {
                return;
            }
            _list[index] = value;
            AfterSetEvent(index, value);
        }

        public delegate void IndexValueDelegate(int index, T value);
        public event IndexValueDelegate AfterInsertEvent = delegate {};
        public event IndexValueDelegate BeforeRemoveEvent = delegate {};
        public event IndexValueDelegate AfterSetEvent = delegate {};
    }
}
