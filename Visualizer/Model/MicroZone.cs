using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Model
{
    public class MicroZone
    {
        public int MicroZoneId { get; set; }
        public string RawId { get; set; }
        public string MicroZoneName { get; set; }
        public string MicroZoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public float MicroZoneHeight { get; set; }
        public float MicroZoneWidth { get; set; }
        public float MicroZoneX { get; set; }
        public float MicroZoneY { get; set; }
        public bool IsLocked { get; set; }
    }
}
