using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Visualizer.Util
{
    public interface IDesignerElement
    {
        void PreviewRender();
        Rect TransformedBounds { get; }
        bool IsDpiDependent { get; }
        int BitBltThreshold { get; }
        double Rotation { get; }
        double CenterX { get; }
        double CenterY { get; }
        bool IsRGBToMonochromeEnabled { get; }
        void RenderCore(DrawingContext dc);
        void RenderDispose();
        FilterInfo GetPostProcessingFilter();
        byte[] RenderDpiDependent(int rotation, ImageType imageType, out int pdfWidth, out int pdfHeight, out int stride);
        BitmapSource RenderDpiDependent();
        BitmapSource RenderDpiDependent(ImageData targetImage, int offsetX, int offsetY, int rotation, bool isVerticalSwaths);
        bool IsWhiteMask { get; }
    }
}
