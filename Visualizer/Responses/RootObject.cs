using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Responses
{
    public class RootObject
    {
        public List<TagResult> TagResults { get; set; }
        public List<CartResult> CartResults { get; set; }
        public List<MicroZoneResult> MicroZoneResults { get; set; }
        public List<TagAuditResult> TagAuditResults { get; set; }
        public List<DepartmentResult> DepartmentResults { get; set; }
    }
}
