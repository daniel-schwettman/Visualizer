using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Responses
{
    public class DepartmentResult
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool IsLastLoaded { get; set; }
        public double ScreenHeight { get; set; }
        public double ScreenWidth { get; set; }
    }
}
