using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Util
{
    public class GenericEventArgs<T> : EventArgs
    {
        private readonly T _value;

        public GenericEventArgs(T value)
        {
            this._value = value;
        }

        public T Value
        {
            get
            {
                return this._value;
            }
        }
    }
}
