using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Visualizer.Util
{
    public interface IDesignerElementRenderer : IDisposable
    {
        //Rect TransformedBounds { get; }
        //void Render(DrawingContext dc, Matrix matrix);
        Rect GetElementBounds(Matrix renderingMatrix);
        BitmapSource Render(ImageData targetImage, Matrix renderingMatrix, int bitmapWidth, int bitmapHeight, int offsetX, int offsetY, MaskType mask, bool isVerticalSwaths);
    }
}
