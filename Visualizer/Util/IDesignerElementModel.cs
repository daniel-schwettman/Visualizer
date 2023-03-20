using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Util
{
    public interface IDesignerElementModel
    {
        //Matrix RenderTransform { get; }
        //double CenterX { get; }
        //double CenterY { get; }
        //Rect Bounds { get; }
        double Width { get; set; }
        double Height { get; set; }
        double OffsetX { get; set; }
        double OffsetY { get; set; }
        double Rotation { get; set; }
        bool LockAspectRatio { get; set; }
    }
}
