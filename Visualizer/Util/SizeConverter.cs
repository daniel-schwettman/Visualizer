using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using Visualizer.Settings;

namespace Visualizer.Util
{
    public class SizeConverter : IValueConverter
    {
        private double originalScreenWidth;
        private double originalScreenHeight;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Size size && parameter is FrameworkElement element)
            {
                double factorX = element.ActualWidth / originalScreenWidth;
                double factorY = element.ActualHeight / originalScreenHeight;

                double scaledWidth = size.Width * factorX;
                double scaledHeight = size.Height * factorY;

                return new Size(scaledWidth, scaledHeight);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Size size && parameter is FrameworkElement element)
            {
                double factorX = element.ActualWidth / originalScreenWidth;
                double factorY = element.ActualHeight / originalScreenHeight;

                double unscaledWidth = size.Width / factorX;
                double unscaledHeight = size.Height / factorY;

                return new Size(unscaledWidth, unscaledHeight);
            }

            return value;
        }

        public void SetOriginalScreenSize(double width, double height)
        {
            originalScreenWidth = width;
            originalScreenHeight = height;
        }
    }
}
