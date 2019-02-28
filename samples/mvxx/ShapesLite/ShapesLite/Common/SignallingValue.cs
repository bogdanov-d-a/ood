using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Common
{
    public class SignallingValue<T> where T : IEquatable<T>
    {
        private T _value;

        public SignallingValue(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set {
                if (Value.Equals(value))
                {
                    return;
                }
                _value = value;
                FireEvent();
            }
        }

        public void FireEvent()
        {
            Event(_value);
        }

        public delegate void Delegate(T value);
        public event Delegate Event = delegate {};
    }
}
