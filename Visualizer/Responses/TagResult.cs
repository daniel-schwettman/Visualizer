using System;
using System.Collections.Generic;
using System.Text;
using Visualizer.Model;

namespace Visualizer.Responses
{
    public class TagResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdatedOnServer { get; set; }
        public DateTime ReceviedByGatewayOn { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Rssi { get; set; }
        public string MZone1Rssi { get; set; }
        public string MZone1 { get; set; }
        public string MZone2 { get; set; }
        public string Category { get; set; }
        public string AssignedType { get; set; }
        public string ReaderId { get; set; }
        public string Battery { get; set; }
        public string StatusCode { get; set; }
        public string TagType { get; set; }
        public List<Asset> AssociatedAssets { get; set; }
        public string MZone1Name { get; set; }
        public int MZone1Number { get; set; }
        public string MZone2Name { get; set; }
        public int MZone2Number { get; set; }
        public bool IsSelected { get; set; }
        public string CartId { get; set; }
        public string SequenceNumber { get; set; }
        public string Operation { get; set; }
        public int DatabaseId { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
    }
}
