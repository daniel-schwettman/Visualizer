using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Visualizer.Util
{
    //http://joyfulwpf.blogspot.com/2007/10/extented-visualtreehelper-class.html
    public static class VisualTreeHelperEx
    {
        public static T FindVisualAncestorByType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null) return default(T);
            if (dependencyObject is T) return (T)dependencyObject;
            T parent = default(T);
            parent = FindVisualAncestorByType<T>(VisualTreeHelper.GetParent(dependencyObject));
            return parent;
        }
        public static T FindVisualChildByType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null) return default(T);
            if (dependencyObject is T) return (T)dependencyObject;
            T child = default(T);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                child = FindVisualChildByType<T>(VisualTreeHelper.GetChild(dependencyObject, i));
                if (child != null) return child;
            }
            return null;
        }
    }
}
