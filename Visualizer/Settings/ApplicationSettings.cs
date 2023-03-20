using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Settings
{
	public static class ApplicationSettings
	{
		public static EventHandler CollectionChanged;

		public static int SyncIntervalSeconds { get; set; }
		public static string ServerUrl { get; set; }
		public static bool IsDarkMode { get; set; }
		public static bool ShowToolTips { get; set; }
		public static bool ShowMapping { get; set; }
		public static bool ShowInventory { get; set; }
		public static bool ShowReporting { get; set; }
		public static bool ShowAsset { get; set; }
		public static Tuple<double, double> OriginalScreenSize { get; set; }
	}
}
