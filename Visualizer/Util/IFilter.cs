using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Util
{
    public interface IFilter
    {
        ImageData ApplyFilter(ImageData sourceData);
        void ApplyFilterInPlace(ImageData sourceData);
    }
}
