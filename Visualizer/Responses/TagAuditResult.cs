using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Responses
{
    public class TagAuditResult
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public string CurrentMicroZone { get; set; }
        public string PreviousMicroZone { get; set; }
        public string MicroZoneName { get; set; }
        public string MicroZoneNumber { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string Rssi { get; set; }
    }
}
