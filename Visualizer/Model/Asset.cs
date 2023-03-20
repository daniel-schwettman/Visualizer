using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Model
{
    public class Asset
    {
        public int AssetId { get; set; }
        public string Name { get; set; }
        public int TagId { get; set; }
        public bool IsActive { get; set; }
        public string AssetIdentifier { get; set; }
    }
}
