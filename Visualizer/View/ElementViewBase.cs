using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Visualizer.Util;

namespace Visualizer.View
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public abstract class ElementViewBase : Control
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ViewModelElement), typeof(ElementViewBase));

        public ElementViewBase()
        {
            KeyBinding deleteKeyBinding = new KeyBinding(ApplicationCommands.Delete, Key.Delete, ModifierKeys.None);
            this.InputBindings.Add(deleteKeyBinding);
        }

        public ViewModelElement ViewModel
        {
            get
            {
                return (ViewModelElement)base.GetValue(ViewModelProperty);
            }
            set
            {
                base.SetValue(ViewModelProperty, value);
            }
        }
    }
}
