using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Util
{
	public static class DataTranformUtility
	{
		public const int TAG_ID_CHARACTER_COUNT = 12;
		public const int MICROZONE_CHARACTER_COUNT = 6;

		public static string SwapBytes(string bytes, int characterCount)
		{
			string convertedHexValue = String.Empty;

			if (String.IsNullOrEmpty(bytes) == true)
			{
				return String.Empty;
			}

			// Remove all leading and trailing whitespace
			bytes = bytes.Trim(new char[] { ' ' });

			try
			{
				ulong value = Convert.ToUInt64(bytes, 16);

				ulong convertedValue = (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
									   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
									   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
									   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;

				convertedHexValue = String.Format("{0:X6}", convertedValue).Substring(0, characterCount);
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.TraceError(String.Format("SwapBytes Exception:{0}", e.ToString()));
			}

			return convertedHexValue;
		}
	}
}
