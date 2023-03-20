using System;
using System.Collections.Generic;
using System.IO;
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
using Telerik.Windows.Persistence;
using Telerik.Windows.Persistence.Services;
using Telerik.Windows.Persistence.Storage;
using Visualizer.Responses;
using Visualizer.Util;
using Visualizer.ViewModel;

namespace Visualizer.View
{
    /// <summary>
    /// Interaction logic for SystemReportingPaneView.xaml
    /// </summary>
    public partial class SystemReportingPaneView : UserControl
    {
        private Stream stream;

        public SystemReportingPaneViewModel SystemReportingVM { get; set; }

        public SystemReportingPaneView()
        {
            InitializeComponent();
            ServiceProvider.RegisterPersistenceProvider<ICustomPropertyProvider>(typeof(RadGridView), new RadGridViewCustomPropertyProvider());
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            this.SystemReportingVM.FilteredTagList.Clear();

            foreach (var item in this.TagsGrid.Items)
            {
                this.SystemReportingVM.FilteredTagList.Add(item as TagResult);
            }

            this.SystemReportingVM.GenerateReport(this.SystemReportingVM.FilteredTagList);
        }

        private void MainGridView_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
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

            this.SystemReportingVM.FilteredCount = (sender as RadGridView).Items.Count;

            if (this.SystemReportingVM.FilteredCount == 0)
            {
                this.SystemReportingVM.FilteredCount = this.SystemReportingVM.TotalCount;
            }

            this.SystemReportingVM.UpdateUI();
        }

        private void MicroZoneGridView_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
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

            this.SystemReportingVM.MicroZoneFilteredCount = (sender as RadGridView).Items.Count;

            if (this.SystemReportingVM.MicroZoneFilteredCount == 0)
            {
                this.SystemReportingVM.MicroZoneFilteredCount = this.SystemReportingVM.MicroZoneTotalCount;
            }

            this.SystemReportingVM.UpdateUI();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            RadGridView grid = e.OriginalSource as RadGridView;

            this.SystemReportingVM.SelectedTags.Clear();

            foreach(object tag in grid.SelectedItems)
            {
                this.SystemReportingVM.SelectedTags.Add(tag as TagResult);
            }
        }

        public void mainScreenVM_CollectionChanged(object sender, EventArgs e)
        {
        }

        private void TagsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.SystemReportingVM = this.DataContext as SystemReportingPaneViewModel;
            this.SystemReportingVM.CollectionChanged += mainScreenVM_CollectionChanged;

            this.SystemReportingVM.UpdateUI();
            this.SystemReportingVM.SaveColumnLayoutEvent += SystemReportingVM_SaveColumnLayoutEvent;

            //load previous
            IsolatedStorageProvider isoProvider = new IsolatedStorageProvider();
            isoProvider.LoadFromStorage();

            foreach (GridViewDataColumn col in this.TagsGrid.Columns)
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

            this.SystemReportingVM.FilteredCount = this.TagsGrid.Items.Count;

            if (this.SystemReportingVM.FilteredCount == 0)
            {
                this.SystemReportingVM.FilteredCount = this.SystemReportingVM.TotalCount;
            }

            this.SystemReportingVM.UpdateUI();
        }

        private void SystemReportingVM_SaveColumnLayoutEvent(object sender, EventArgs e)
        {
            IsolatedStorageProvider isoProvider = new IsolatedStorageProvider();
            isoProvider.SaveToStorage();
        }

        private void MicroZonesGrid_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
