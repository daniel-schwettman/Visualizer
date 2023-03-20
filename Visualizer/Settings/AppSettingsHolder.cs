using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Settings
{
	public class AppSettingsHolder
	{
		public int SyncIntervalSeconds { get; set; }
		public string ServerUrl { get; set; }
		public bool IsDarkMode { get; set; }
		public bool ShowToolTips { get; set; }
		public bool ShowMapping { get; set; }
		public bool ShowInventory { get; set; }
		public bool ShowReporting { get; set; }
		public bool ShowAsset { get; set; }
	}
}
