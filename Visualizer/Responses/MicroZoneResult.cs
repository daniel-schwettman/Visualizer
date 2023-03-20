using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;

namespace Visualizer.Responses
{
    public class MicroZoneResult
    {
        public int MicroZoneId { get; set; }
        public string RawId { get; set; }
        public string MicroZoneName { get; set; }
        public string MicroZoneNumber { get; set; }
        public string TagAssociationNumber { get; set; }
        public int DepartmentId { get; set; }
        public float MicroZoneX { get; set; }
        public float MicroZoneY { get; set; }
        public float MicroZoneHeight { get; set; }
        public float MicroZoneWidth { get; set; }
        public bool IsLocked { get; set; }
        public List<TagResult> TagsCurrentlyInZone { get; set; }
        public List<TagResult> TagsPreviouslyInZone { get; set; }
    }
}
