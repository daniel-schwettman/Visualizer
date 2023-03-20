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
    public class PositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the current screen resolution
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            // Get the saved coordinates
            double savedX = (double)values[0];
            double savedY = (double)values[1];

            // Traverse the visual tree to get the scale factor of the parent controls
            DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject)values[2]);
            while (parent != null && !(parent is Window))
            {
                if (parent is FrameworkElement element)
                {
                    savedX /= element.ActualWidth;
                    savedY /= element.ActualHeight;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }

            // Adjust the coordinates for the current screen resolution
            double adjustedX = savedX * (screenWidth / ApplicationSettings.OriginalScreenSize.Item1);
            double adjustedY = savedY * (screenHeight / ApplicationSettings.OriginalScreenSize.Item2);

            // Return a Thickness object with the adjusted coordinates
            return new Thickness(adjustedX, adjustedY, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
