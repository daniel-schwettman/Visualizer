using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Persistence.Services;
using Telerik.Windows.Persistence.Storage;
using Visualizer.Model;
using Visualizer.Responses;
using Visualizer.Util;
using Visualizer.ViewModel;

namespace Visualizer.View
{
    /// <summary>
    /// Interaction logic for MappingPaneView.xaml
    /// </summary>
    public partial class MappingPaneView : UserControl
    {
		private MappingPaneViewModel mappingPaneViewModel;
        private EventHandler mappingVM_SaveColumnLayoutEvent;

        public MappingPaneView()
		{
			InitializeComponent();
			this.MapView.Focus();
			this.MapView.AnimationLevel = AnimationLevel.Full;
			this.MapView.Mode = new AerialMode(true);
			this.MapView.ZoomLevel = 18.0;
			this.MapView.Center = new Location(32.2177, -82.4135);

			ServiceProvider.RegisterPersistenceProvider<ICustomPropertyProvider>(typeof(RadGridView), new RadGridViewCustomPropertyProvider());
		}

		private void TagHistoryReportButton_Click(object sender, RoutedEventArgs e)
		{
			mappingPaneViewModel.TagHistoryFilteredList.Clear();

			foreach (var item in this.TagHistoryGridView.Items)
			{
				mappingPaneViewModel.TagHistoryFilteredList.Add(item as TagResult);
			}

			mappingPaneViewModel.GenerateTagHistoryFilteredReport(mappingPaneViewModel.TagHistoryFilteredList);
		}

		private void CurrentTagsReportButton_Click(object sender, RoutedEventArgs e)
		{
			mappingPaneViewModel.CurrentTagsFilteredList.Clear();

			foreach (var item in this.TagGridView.Items)
			{
				mappingPaneViewModel.CurrentTagsFilteredList.Add(item as TagResult);
			}

			mappingPaneViewModel.GenerateCurrentTagFilteredReport(mappingPaneViewModel.CurrentTagsFilteredList);
		}

		private void Map_Loaded(object sender, RoutedEventArgs e)
		{
			mappingPaneViewModel = this.DataContext as MappingPaneViewModel;
			mappingPaneViewModel.MapView = this.MapView;
			mappingPaneViewModel.CollectionChanged += mappingPaneVM_CollectionChanged;
            mappingPaneViewModel.SaveColumnLayoutEvent += MappingPaneViewModel_SaveColumnLayoutEvent;

			//load previous
			IsolatedStorageProvider isoProvider = new IsolatedStorageProvider();
			isoProvider.LoadFromStorage();
		}

        private void MappingPaneViewModel_SaveColumnLayoutEvent(object sender, EventArgs e)
        {
			IsolatedStorageProvider isoProvider = new IsolatedStorageProvider();
			isoProvider.SaveToStorage();
		}

        private void mappingPaneVM_CollectionChanged(object sender, EventArgs e)
        {
			foreach (GridViewDataColumn col in this.TagGridView.Columns)
			{
				if (col.ColumnFilterDescriptor.IsActive)
				{
					col.Background = Brushes.Blue;
				}
				else
				{
					if (col.Background == Brushes.Blue)
					{
						col.Background = Brushes.Transparent;
					}
				}
			}

			foreach (GridViewDataColumn col in this.TagGridView2.Columns)
			{
				if (col.ColumnFilterDescriptor.IsActive)
				{
					col.Background = Brushes.Blue;
				}
				else
				{
					if (col.Background == Brushes.Blue)
					{
						col.Background = Brushes.Transparent;
					}
				}
			}
		}

        private void TagGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
		{
			if(mappingPaneViewModel != null)
            {
				mappingPaneViewModel.SelectedTags.Clear();

				foreach (object obj in this.TagGridView.SelectedItems)
				{
					mappingPaneViewModel.SelectedTags.Add(obj as TagResult);
				}

				mappingPaneViewModel.MapSelectedTags(mappingPaneViewModel.SelectedTags);
			}
		}

        private void TagGridView_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
			foreach (GridViewDataColumn col in (sender as RadGridView).Columns)
			{
				if (col.ColumnFilterDescriptor.IsActive)
				{
					col.Background = Brushes.Blue;
				}
				else
				{
					if (col.Background == Brushes.Blue)
					{
						col.Background = Brushes.Transparent;
					}
				}
			}
		}
		private void TagGridView2_SelectionChanged(object sender, SelectionChangeEventArgs e)
		{

		}

		private void TagGridView2_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
		{
			foreach (GridViewDataColumn col in (sender as RadGridView).Columns)
			{
				if (col.ColumnFilterDescriptor.IsActive)
				{
					col.Background = Brushes.Blue;
				}
				else
				{
					if (col.Background == Brushes.Blue)
					{
						col.Background = Brushes.Transparent;
					}
				}
			}
		}

		private void TagHistoryGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
		{
			if (mappingPaneViewModel != null)
			{
				mappingPaneViewModel.SelectedHistoryTags.Clear();

				foreach (object obj in this.TagHistoryGridView.SelectedItems)
				{
					mappingPaneViewModel.SelectedHistoryTags.Add(obj as TagResult);
				}

				mappingPaneViewModel.MapSelectedTags(mappingPaneViewModel.SelectedHistoryTags);
			}
		}

		private void TagHistoryGridView_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
		{
			foreach (GridViewDataColumn col in (sender as RadGridView).Columns)
			{
				if (col.ColumnFilterDescriptor.IsActive)
				{
					col.Background = Brushes.Blue;
				}
				else
				{
					if (col.Background == Brushes.Blue)
					{
						col.Background = Brushes.Transparent;
					}
				}
			}
		}

		private void TagHistoryGridView2_SelectionChanged(object sender, SelectionChangeEventArgs e)
		{

		}

		private void TagHistoryGridView2_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
		{
			foreach (GridViewDataColumn col in (sender as RadGridView).Columns)
			{
				if (col.ColumnFilterDescriptor.IsActive)
				{
					col.Background = Brushes.Blue;
				}
				else
				{
					if (col.Background == Brushes.Blue)
					{
						col.Background = Brushes.Transparent;
					}
				}
			}
		}

		private void AssetGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
		{
			if (mappingPaneViewModel != null)
			{
				mappingPaneViewModel.SelectedAssets.Clear();

				foreach (object obj in this.AssetGridView.SelectedItems)
				{
					mappingPaneViewModel.SelectedAssets.Add(obj as Asset);
				}

				mappingPaneViewModel.MapSelectedAssets(mappingPaneViewModel.SelectedAssets);
			}
		}

		private void AssetGridView_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
		{
			foreach (GridViewDataColumn col in (sender as RadGridView).Columns)
			{
				if (col.ColumnFilterDescriptor.IsActive)
				{
					col.Background = Brushes.Blue;
				}
				else
				{
					if (col.Background == Brushes.Blue)
					{
						col.Background = Brushes.Transparent;
					}
				}
			}
		}
		private void AssetGridView2_SelectionChanged(object sender, SelectionChangeEventArgs e)
		{

		}

		private void AssetGridView2_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
		{
			foreach (GridViewDataColumn col in (sender as RadGridView).Columns)
			{
				if (col.ColumnFilterDescriptor.IsActive)
				{
					col.Background = Brushes.Blue;
				}
				else
				{
					if (col.Background == Brushes.Blue)
					{
						col.Background = Brushes.Transparent;
					}
				}
			}
		}
	}
}
