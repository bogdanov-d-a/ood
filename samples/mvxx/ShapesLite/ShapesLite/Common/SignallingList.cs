using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Common
{
    public class SignallingList<T> : ISignallingList<T> where T : IEquatable<T>
    {
        private readonly List<T> _list = new List<T>();

        private IndexValueDelegate<T> _afterInsertEvent = delegate { };
        private IndexValueDelegate<T> _beforeRemoveEvent = delegate { };
        private IndexTwoValuesDelegate<T> _indexTwoValuesDelegate = delegate { };

        public int Count
        {
            get => _list.Count;
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            _afterInsertEvent(index, item);
        }

        public void RemoveAt(int index)
        {
            _beforeRemoveEvent(index, _list[index]);
            _list.RemoveAt(index);
        }

        public T GetAt(int index)
        {
            return _list[index];
        }

        public void SetAt(int index, T value)
        {
            T oldValue = GetAt(index);
            if (oldValue.Equals(value))
            {
                return;
            }
            _list[index] = value;
            _indexTwoValuesDelegate(index, oldValue, value);
        }

        public event IndexValueDelegate<T> AfterInsertEvent
        {
            add => _afterInsertEvent += value;
            remove => _afterInsertEvent -= value;
        }

        public event IndexValueDelegate<T> BeforeRemoveEvent
        {
            add => _beforeRemoveEvent += value;
            remove => _beforeRemoveEvent -= value;
        }

        public event IndexTwoValuesDelegate<T> AfterSetEvent
        {
            add => _indexTwoValuesDelegate += value;
            remove => _indexTwoValuesDelegate -= value;
        }
    }
}
