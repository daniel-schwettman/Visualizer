using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class Resizer : Control
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ViewModelElement), typeof(Resizer));

        public Resizer()
            : base()
        {
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
