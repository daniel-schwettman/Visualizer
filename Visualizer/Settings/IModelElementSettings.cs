using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Settings
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public interface IModelElementSettings
    {
        IModelElementSettings Clone();
    }
}
