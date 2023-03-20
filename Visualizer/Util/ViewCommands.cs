using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public static class ViewCommands
    {
        public static readonly RoutedCommand ToggleSourceEndpointVisibility = new RoutedCommand("ToggleSourceEndpointVisibility", typeof(ViewCommands));
        public static readonly RoutedCommand ElementMoved = new RoutedCommand("ElementMoved", typeof(ViewCommands));
        public static readonly RoutedCommand ElementSizeChanged = new RoutedCommand("ElementSizeChanged", typeof(ViewCommands));
        public static readonly RoutedCommand PreviewElementShowProperties = new RoutedCommand("PreviewElementShowProperties", typeof(ViewCommands));
    }
}
