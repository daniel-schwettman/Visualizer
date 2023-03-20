using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Visualizer.Model;
using Visualizer.Responses;
using Visualizer.ViewModel;

namespace Visualizer.View
{
    /// <summary>
    /// Interaction logic for AssetManagementPaneView.xaml
    /// </summary>
    public partial class AssetManagementPaneView : UserControl
    {
        public AssetManagementPaneViewModel AssetManagementVM { get; set; }

        public AssetManagementPaneView()
        {
            InitializeComponent();
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

            this.AssetManagementVM.TagFilteredCount = (sender as RadGridView).Items.Count;

            if (this.AssetManagementVM.TagFilteredCount == 0)
            {
                this.AssetManagementVM.TagFilteredCount = this.AssetManagementVM.TagTotalCount;
            }

            this.AssetManagementVM.UpdateUI();
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

            this.AssetManagementVM.AssetFilteredCount = (sender as RadGridView).Items.Count;

            if (this.AssetManagementVM.AssetFilteredCount == 0)
            {
                this.AssetManagementVM.AssetFilteredCount = this.AssetManagementVM.AssetTotalCount;
            }

            this.AssetManagementVM.UpdateUI();
        }

        public void mainScreenVM_CollectionChanged(object sender, EventArgs e)
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

            this.AssetManagementVM.TagFilteredCount = this.TagGridView.Items.Count;

            if (this.AssetManagementVM.TagFilteredCount == 0)
            {
                this.AssetManagementVM.TagFilteredCount = this.AssetManagementVM.TagTotalCount;
            }

            foreach (GridViewDataColumn col in this.AssetGridView.Columns)
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

            this.AssetManagementVM.AssetFilteredCount = this.AssetGridView.Items.Count;

            if (this.AssetManagementVM.AssetFilteredCount == 0)
            {
                this.AssetManagementVM.AssetFilteredCount = this.AssetManagementVM.AssetTotalCount;
            }

            this.AssetManagementVM.UpdateUI();
        }

        private void TagsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.AssetManagementVM = this.DataContext as AssetManagementPaneViewModel;

            if(this.AssetManagementVM != null)
            {
                this.AssetManagementVM.CollectionChanged += mainScreenVM_CollectionChanged;

                this.AssetManagementVM.TagFilteredCount = this.TagGridView.Items.Count;

                if (this.AssetManagementVM.TagFilteredCount == 0)
                {
                    this.AssetManagementVM.TagFilteredCount = this.AssetManagementVM.TagTotalCount;
                }

                this.AssetManagementVM.UpdateUI();
            }
        }

        private void AssetGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.AssetManagementVM.AssetFilteredCount = this.AssetGridView.Items.Count;

            if (this.AssetManagementVM.AssetFilteredCount == 0)
            {
                this.AssetManagementVM.AssetFilteredCount = this.AssetManagementVM.AssetTotalCount;
            }

            this.AssetManagementVM.UpdateUI();
        }

        private void TagGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (this.TagGridView.SelectedItems.Count > 0)
            {
                this.AssetManagementVM.SelectedTags.Clear();
                this.AssetManagementVM.AssociatedAssets.Clear();

                foreach(object item in this.TagGridView.SelectedItems)
                {
                    TagResult tag = item as TagResult;
                    this.AssetManagementVM.SelectedTags.Add(tag);
                    AssetManagementVM.AssociatedAssets.AddRange(tag.AssociatedAssets);
                }

                this.AssetManagementVM.UpdateUI();
                this.AssetManagementVM.FireCollectionChanged();
            }
        }

        private void AssetGridView_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            if (this.AssetManagementVM != null)
            {
                this.AssetManagementVM.SelectedAssets.Clear();

                foreach (object obj in this.AssetGridView.SelectedItems)
                {
                    Asset result = obj as Asset;
                    this.AssetManagementVM.SelectedAssets.Add(result);
                }

                //this.AssetManagementVM.UpdateUI();
               // this.AssetManagementVM.FireCollectionChanged();
            }
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            this.AssetManagementVM.FilteredTagList.Clear();

            foreach (var item in this.TagGridView.Items)
            {
                this.AssetManagementVM.FilteredTagList.Add(item as TagResult);
            }
        }
    }
}
