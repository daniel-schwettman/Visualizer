using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Telerik.Windows.Controls;

namespace Visualizer.Util
{
	public class ColumnSetting
	{
		public string UniqueName { get; set; }
		public int DisplayOrder { get; set; }
		public string Header { get; set; }
		public GridViewLength Width { get; set; }
		public bool IsVisible { get; set; }
	}

	public class SortDescriptorSetting
	{
		public string ColumnUniqueName { get; set; }
		public ListSortDirection SortDirection { get; set; }
	}

	public class GroupDescriptorSetting
	{
		public string ColumnUniqueName { get; set; }
		public ListSortDirection? SortDirection { get; set; }
	}

	public class FilterDescriptorSetting
	{
		public Telerik.Windows.Data.FilterOperator Operator { get; set; }
		public object Value { get; set; }
		public bool IsCaseSensitive { get; set; }
	}

	public class FilterSetting
	{
		public string ColumnUniqueName { get; set; }

		private List<object> selectedDistinctValue;
		public List<object> SelectedDistinctValues
		{
			get
			{
				if (this.selectedDistinctValue == null)
				{
					this.selectedDistinctValue = new List<object>();
				}

				return this.selectedDistinctValue;
			}
		}

		public FilterDescriptorSetting Filter1 { get; set; }
		public Telerik.Windows.Data.FilterCompositionLogicalOperator FieldFilterLogicalOperator { get; set; }
		public FilterDescriptorSetting Filter2 { get; set; }
	}
}
