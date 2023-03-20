using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Util
{
    public class OffsetEventArgs : EventArgs
    {
        private readonly double _offset;

        public OffsetEventArgs(double offset)
        {
            this._offset = offset;
        }

        public double Offset
        {
            get
            {
                return this._offset;
            }
        }
    }
}
