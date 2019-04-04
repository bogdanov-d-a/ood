using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Common
{
    public delegate void IndexValueDelegate<T>(int index, T value);
    public delegate void IndexTwoValuesDelegate<T>(int index, T oldValue, T value);

    public interface ISignallingList<T> where T : IEquatable<T>
    {
        int Count { get; }
        void Insert(int index, T item);
        void RemoveAt(int index);
        T GetAt(int index);
        void SetAt(int index, T value);

        event IndexValueDelegate<T> AfterInsertEvent;
        event IndexValueDelegate<T> BeforeRemoveEvent;
        event IndexTwoValuesDelegate<T> AfterSetEvent;
    }
}
