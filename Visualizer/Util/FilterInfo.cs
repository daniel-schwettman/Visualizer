using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Util
{
    public class FilterInfo
    {
        public static readonly FilterInfo None = new FilterInfo();

        private int brightness;
        private int red;
        private int green;
        private int blue;
        private FilterType filterType;
        private int diffusion;
        private int threshold;

        public FilterInfo()
        {
            this.filterType = FilterType.None;
        }

        public FilterInfo(FilterInfo source)
        {
            this.brightness = source.brightness;
            this.red = source.red;
            this.green = source.green;
            this.blue = source.blue;
            this.filterType = source.filterType;
            this.diffusion = source.diffusion;
            this.threshold = source.threshold;
        }

        public FilterInfo Clone()
        {
            return new FilterInfo(this);
        }

        public int Brightness
        {
            get { return brightness; }
            set { brightness = value; }
        }

        public int Red
        {
            get { return red; }
            set { red = value; }
        }

        public int Green
        {
            get { return green; }
            set { green = value; }
        }

        public int Blue
        {
            get { return blue; }
            set { blue = value; }
        }


        public FilterType FilterType
        {
            get { return filterType; }
            set { filterType = value; }
        }

        public int Diffusion
        {
            get { return diffusion; }
            set { diffusion = value; }
        }

        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }
    }
}
