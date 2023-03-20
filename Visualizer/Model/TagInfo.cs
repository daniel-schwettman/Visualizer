using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Model
{
    public class TagInfo
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public TagType Type { get; set; }

        public TagCategory Category { get; set; }
    }
}
