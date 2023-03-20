using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Visualizer.Util
{
    public static class TransformHelper
    {
        public static TranslateTransform CreateTranslateTransform(double offsetX, double offsetY)
        {
            TranslateTransform transform = new TranslateTransform(offsetX, offsetY);
            transform.Freeze();
            return transform;
        }

        public static RotateTransform CreateRotateTransform(double rotation)
        {
            RotateTransform transform = new RotateTransform(rotation);
            transform.Freeze();
            return transform;
        }

        public static RotateTransform CreateRotateTransform(double rotation, double centerX, double centerY)
        {
            RotateTransform transform = new RotateTransform(rotation, centerX, centerY);
            transform.Freeze();
            return transform;
        }

        public static MatrixTransform CreateMatrixTransform(Matrix matrix)
        {
            MatrixTransform transform = new MatrixTransform(matrix);
            transform.Freeze();
            return transform;
        }
    }
}
