using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Visualizer.Model;
using Visualizer.Responses;

namespace Visualizer.ViewModel
{
    public class AssetManagementPaneViewModel : ViewModelBase
    {
        private List<TagResult> _tags;
        private List<TagResult> _selectedTags;
        private List<Asset> _selectedAssets;
        private List<TagResult> _filteredTagList;
        private List<Asset> _assets;
        private DataManager _dataManager;
        public EventHandler CollectionChanged;
        private int tagFilteredCount;
        private int assetFilteredCount;

        public DelegateCommand AddAssetCommand { get; set; }
        public DelegateCommand UnpairAssetCommand { get; set; }
        public DelegateCommand EditAssetCommand { get; set; }
        public DelegateCommand GenerateReportCommand { get; set; }

        public AssetManagementPaneViewModel()
        {
            this._dataManager = DataManager.Instance;
            this._dataManager.DataUpdated += this._dataManager_DataUpdated;
            this._tags = new List<TagResult>();
            this._selectedTags = new List<TagResult>();
            this._selectedAssets = new List<Asset>();
            this._filteredTagList = new List<TagResult>();
            this._assets = new List<Asset>();

            this.AddAssetCommand = new DelegateCommand(AddAsset);
            this.UnpairAssetCommand = new DelegateCommand(UnpairAsset);
            this.EditAssetCommand = new DelegateCommand(EditAsset);
            this.GenerateReportCommand = new DelegateCommand(GenerateReport);

            SyncFeed(new object());
        }

        public void GenerateReport(object action)
        {
            StringBuilder logBuilder = new StringBuilder();

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Tags");
                stringBuilder.AppendLine();
                stringBuilder.Append("Tag Name,");
                stringBuilder.Append("Tag Id,");
                stringBuilder.Append("Updated On Server,");
                stringBuilder.Append("Latitude,");
                stringBuilder.Append("Longitude,");
                stringBuilder.Append("Operation,");
                stringBuilder.Append("Status Code,");
                stringBuilder.Append("Battery,");
                stringBuilder.Append("Rssi,");
                stringBuilder.Append("Current Mzone,");
                stringBuilder.Append("Previous Mzone,");
                stringBuilder.Append("Sequence Number,");
                stringBuilder.Append("Reader Id,");
                stringBuilder.Append("Asset Name,");
                stringBuilder.Append("Asset Description,");
                stringBuilder.Append("Asset IsActive");
                stringBuilder.AppendLine();

                //sort the tag by cartID
                List<TagResult> sortedList = this.FilteredTagList.OrderBy(x => x.Id).ToList();
                sortedList.Sort((tagA, tagB) => tagB.Id.CompareTo(tagA.Id));
                sortedList.Reverse();

                foreach (TagResult tag in sortedList)
                {
                    stringBuilder.Append($"{tag.Name},");
                    stringBuilder.Append($"{tag.Id.ToUpper()},");
                    stringBuilder.Append($"{tag.LastUpdatedOnServer.ToString("u", DateTimeFormatInfo.InvariantInfo)},");
                    stringBuilder.Append($"{tag.Latitude},");
                    stringBuilder.Append($"{tag.Longitude},");
                    stringBuilder.Append($"{tag.Operation},");
                    stringBuilder.Append($"{tag.StatusCode},");
                    stringBuilder.Append($"{tag.Battery},");
                    stringBuilder.Append($"{tag.Rssi},");
                    stringBuilder.Append($"{tag.MZone1},");
                    stringBuilder.Append($"{tag.MZone2},");
                    stringBuilder.Append($"{tag.SequenceNumber},");
                    stringBuilder.Append($"{tag.ReaderId},");
                    stringBuilder.AppendLine();

                    foreach (Asset asset in tag.AssociatedAssets)
                    {
                        stringBuilder.Append($",");
                        stringBuilder.Append($","); 
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($",");
                        stringBuilder.Append($"{asset.Name},");
                        stringBuilder.Append($"{asset.AssetIdentifier},");
                        stringBuilder.Append($"{asset.IsActive},");
                        stringBuilder.AppendLine();
                    }

                    stringBuilder.AppendLine();
                }

                string fileName = $"\\FilteredReport_{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.csv";
                string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fullpath = mydocsPath + fileName;

                FileInfo file = new FileInfo(fullpath);
                if (file.Exists)
                {
                    if (!IsFileLocked(file))
                        File.WriteAllText(fullpath, stringBuilder.ToString());
                }
                else
                {
                    File.WriteAllText(fullpath, stringBuilder.ToString());
                }

            }
            catch (Exception e)
            {
                string fileName = "\\logs.txt";
                string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fullpath = mydocsPath + fileName;

                // Get stack trace for the exception with source file information
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                logBuilder.AppendLine($"Error: {e.Message} on line {line.ToString()}");
                logBuilder.AppendLine("");

                File.WriteAllText(fullpath, logBuilder.ToString());
            }
        }

        private void AddAsset(object obj)
        {
            if(this.SelectedTags != null && this.SelectedTags.Count == 1)
            {
            RadWindow window = new RadWindow();
            window.Owner = System.Windows.Application.Current.MainWindow;
            window.HideMaximizeButton = true;
            window.HideMinimizeButton = true;
            window.Header = "Add Asset";
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Margin = new Thickness(20);
            StackPanel innerPanel = new StackPanel();
            innerPanel.Orientation = Orientation.Horizontal;
            TextBlock txtBlock = new TextBlock();
            txtBlock.Text = "Asset Name: ";
            txtBlock.Margin = new Thickness(10, 10, 0, 0);
            TextBox txtBox = new TextBox();
            txtBox.Width = 200;
            txtBox.Margin = new Thickness(37, 6, 10, 0);
            innerPanel.Children.Add(txtBlock);
            innerPanel.Children.Add(txtBox);
            panel.Children.Add(innerPanel);
            StackPanel innerPanel2 = new StackPanel();
            innerPanel2.Orientation = Orientation.Horizontal;
            TextBlock txtBlock2 = new TextBlock();
            txtBlock2.Text = "Asset Identifier: ";
            txtBlock2.Margin = new Thickness(10, 10, 0, 0);
            TextBox txtBox2 = new TextBox();
            txtBox2.Width = 200;
            txtBox2.Margin = new Thickness(20, 6, 10, 0);
            innerPanel2.Children.Add(txtBlock2);
            innerPanel2.Children.Add(txtBox2);
            panel.Children.Add(innerPanel2);
            StackPanel innerPanel3 = new StackPanel();
            innerPanel3.Orientation = Orientation.Horizontal;
            TextBlock txtBlock3 = new TextBlock();
            txtBlock3.Text = "Is Active: ";
            txtBlock3.Margin = new Thickness(10, 10, 0, 0);
            CheckBox checkBox = new CheckBox();
            checkBox.Margin = new Thickness(58, 10, 10, 0);
            innerPanel3.Children.Add(txtBlock3);
            innerPanel3.Children.Add(checkBox);
            panel.Children.Add(innerPanel3);
            StackPanel innerPanel4 = new StackPanel();
            innerPanel4.Orientation = Orientation.Horizontal;
            innerPanel4.HorizontalAlignment = HorizontalAlignment.Center;
            innerPanel4.Margin = new Thickness(0,20,0,0);
            RadButton btn = new RadButton();
            btn.Content = "Save";
            btn.Margin = new Thickness(10, 10, 0, 0);
            btn.Click += ButtonSave_Click;
            RadButton btn2 = new RadButton();
            btn2.Margin = new Thickness(20, 10, 0, 0);
            btn2.Content = "Cancel";
            btn2.Click += ButtonCancel_Click;
            innerPanel4.Children.Add(btn);
            innerPanel4.Children.Add(btn2);
            panel.Children.Add(innerPanel4);
            window.Content = panel;
            window.ShowDialog();
            }
            else
            {
                RadWindow.Alert("Please select a single tag to associate an asset to.");
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            RadButton button = sender as RadButton;
            RadWindow window = button.ParentOfType<RadWindow>();

            List<TextBox> textBoxes = window.ChildrenOfType<TextBox>().ToList();
            List<CheckBox> checkBoxes = window.ChildrenOfType<CheckBox>().ToList();

            Asset newAsset = new Asset()
            {
                TagId = this.SelectedTags[0].DatabaseId,
                Name = textBoxes[0].Text,
                AssetIdentifier = textBoxes[1].Text,
                IsActive = checkBoxes[0].IsChecked.HasValue ? checkBoxes[0].IsChecked.Value : false
            };

            this._dataManager.AddAsset(newAsset);
            RefreshData();

            window.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            RadButton button = sender as RadButton;
            RadWindow window = button.ParentOfType<RadWindow>();

            window.Close();
        }

        private void EditAsset(object obj)
        {
            if (this.SelectedAssets != null && this.SelectedAssets.Count == 1)
            {
                RadWindow window = new RadWindow();
                window.Owner = System.Windows.Application.Current.MainWindow;
                window.HideMaximizeButton = true;
                window.HideMinimizeButton = true;
                window.Header = "Edit Asset";
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;
                panel.Margin = new Thickness(20);
                StackPanel innerPanel = new StackPanel();
                innerPanel.Orientation = Orientation.Horizontal;
                TextBlock txtBlock = new TextBlock();
                txtBlock.Text = "Asset Name: ";
                txtBlock.Margin = new Thickness(10, 10, 0, 0);
                TextBox txtBox = new TextBox();
                txtBox.Width = 200;
                txtBox.Text = this.SelectedAssets[0].Name;
                txtBox.Margin = new Thickness(37, 6, 10, 0);
                innerPanel.Children.Add(txtBlock);
                innerPanel.Children.Add(txtBox);
                panel.Children.Add(innerPanel);
                StackPanel innerPanel2 = new StackPanel();
                innerPanel2.Orientation = Orientation.Horizontal;
                TextBlock txtBlock2 = new TextBlock();
                txtBlock2.Text = "Asset Identifier: ";
                txtBlock2.Margin = new Thickness(10, 10, 0, 0);
                TextBox txtBox2 = new TextBox();
                txtBox2.Width = 200;
                txtBox2.Text = this.SelectedAssets[0].AssetIdentifier;
                txtBox2.Margin = new Thickness(20, 6, 10, 0);
                innerPanel2.Children.Add(txtBlock2);
                innerPanel2.Children.Add(txtBox2);
                panel.Children.Add(innerPanel2);
                StackPanel innerPanel3 = new StackPanel();
                innerPanel3.Orientation = Orientation.Horizontal;
                TextBlock txtBlock3 = new TextBlock();
                txtBlock3.Text = "Is Active: ";
                txtBlock3.Margin = new Thickness(10, 10, 0, 0);
                CheckBox checkBox = new CheckBox();
                checkBox.Margin = new Thickness(58, 10, 10, 0);
                checkBox.IsChecked = this.SelectedAssets[0].IsActive;
                innerPanel3.Children.Add(txtBlock3);
                innerPanel3.Children.Add(checkBox);
                panel.Children.Add(innerPanel3);
                StackPanel innerPanel4 = new StackPanel();
                innerPanel4.Orientation = Orientation.Horizontal;
                innerPanel4.HorizontalAlignment = HorizontalAlignment.Center;
                innerPanel4.Margin = new Thickness(0, 20, 0, 0);
                RadButton btn = new RadButton();
                btn.Content = "Save";
                btn.Margin = new Thickness(10, 10, 0, 0);
                btn.Click += ButtonSave2_Click;
                RadButton btn2 = new RadButton();
                btn2.Margin = new Thickness(20, 10, 0, 0);
                btn2.Content = "Cancel";
                btn2.Click += ButtonCancel2_Click;
                innerPanel4.Children.Add(btn);
                innerPanel4.Children.Add(btn2);
                panel.Children.Add(innerPanel4);
                window.Content = panel;
                window.ShowDialog();
            }
            else
            {
                RadWindow.Alert("Please select a single asset to edit.");
            }
        }

        private void ButtonSave2_Click(object sender, RoutedEventArgs e)
        {
            RadButton button = sender as RadButton;
            RadWindow window = button.ParentOfType<RadWindow>();

            List<TextBox> textBoxes = window.ChildrenOfType<TextBox>().ToList();
            List<CheckBox> checkBoxes = window.ChildrenOfType<CheckBox>().ToList();

            if (this.SelectedAssets != null && this.SelectedAssets.Count > 0)
            {
                this.SelectedAssets[0].Name = textBoxes[0].Text;
                this.SelectedAssets[0].AssetIdentifier = textBoxes[1].Text;
                this.SelectedAssets[0].IsActive = checkBoxes[0].IsChecked.HasValue ? checkBoxes[0].IsChecked.Value : false;

                this._dataManager.EditAsset(this.SelectedAssets[0]);
            }

            RefreshData();
            window.Close();
        }

        private void ButtonCancel2_Click(object sender, RoutedEventArgs e)
        {
            RadButton button = sender as RadButton;
            RadWindow window = button.ParentOfType<RadWindow>();

            window.Close();
        }

        private void UnpairAsset(object obj)
        {
            if (this.SelectedAssets != null && this.SelectedAssets.Count > 0)
            {
                this._dataManager.DeleteAsset(this.SelectedAssets);
                RefreshData();
            }
            else
            {
                RadWindow.Alert("Select an asset(s) to unpair.");
            }
        }

        private void _dataManager_DataUpdated(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        public void SyncFeed(object action)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                this._tags.Clear();

                this._dataManager.UpdateData();

                foreach (TagResult result in this._dataManager.MostRecentTags)
                {
                    if (result.AssociatedAssets != null)
                    {
                        if (result.AssociatedAssets.Count > 0)
                        {
                            this._tags.Add(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string fileName = "\\logs.txt";
                string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fullpath = mydocsPath + fileName;
                File.WriteAllText(fullpath, e.Message);
            }

            OnCollectionChanged(EventArgs.Empty);
            OnPropertyChanged(() => Tags); ;
            OnPropertyChanged(() => AssociatedAssets);
            UpdateUI();
        }

        public List<TagResult> Tags
        {
            get => this._tags;
            set
            {
                this._tags = value;
                OnCollectionChanged(EventArgs.Empty);
                OnPropertyChanged(() => Tags);
            }
        }

        public List<Asset> AssociatedAssets
        {
            get => this._assets;
            set
            {
                this._assets = value;
                OnCollectionChanged(EventArgs.Empty);
                OnPropertyChanged(() => AssociatedAssets);
            }
        }

        public List<TagResult> SelectedTags
        {
            get
            {
                return this._selectedTags;
            }
            set
            {
                this._selectedTags = value;
                OnPropertyChanged(() => SelectedTags);
            }
        }

        public List<Asset> SelectedAssets
        {
            get
            {
                return this._selectedAssets;
            }
            set
            {
                this._selectedAssets = value;
                OnPropertyChanged(() => SelectedAssets);
            }
        }

        public int TagFilteredCount
        {
            get
            {
                return this.tagFilteredCount;
            }
            set
            {
                this.tagFilteredCount = value;
                OnPropertyChanged("TagsFilteredCount");
            }
        }

        public List<TagResult> FilteredTagList
        {
            get
            {
                return this._filteredTagList;
            }
            set
            {
                this._filteredTagList = value;
            }
        }

        public int TagTotalCount
        {
            get
            {
                if (this.Tags == null)
                {
                    return 0;
                }
                else
                {
                    return this.Tags.Count;
                }
            }
        }

        public int AssetFilteredCount
        {
            get
            {
                return this.assetFilteredCount;
            }
            set
            {
                this.assetFilteredCount = value;
                OnPropertyChanged("AssetFilteredCount");
            }
        }

        public int AssetTotalCount
        {
            get
            {
                if (this._assets == null)
                {
                    return 0;
                }
                else
                {
                    return this._assets.Count;
                }
            }
        }

        public ObservableCollection<TagResult> TagCollection
        {
            get
            {
                return new ObservableCollection<TagResult>(this.Tags);
            }
        }

        public ObservableCollection<Asset> AssetCollection
        {
            get
            {
                return new ObservableCollection<Asset>(this.AssociatedAssets);
            }
        }

        public void UpdateUI()
        {
            OnPropertyChanged("TagTotalCount");
            OnPropertyChanged("TagFilteredCount");
            OnPropertyChanged("AssetTotalCount");
            OnPropertyChanged("AssetFilteredCount");
            OnPropertyChanged("AssociatedAssets");
            OnPropertyChanged(() => AssetCollection);
            OnPropertyChanged(() => TagCollection);
        }

        public void FireCollectionChanged()
        {
            OnCollectionChanged(EventArgs.Empty);
        }

        protected virtual void OnCollectionChanged(EventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, e);
            }
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                {
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                return true;
            }

            return false;
        }
    }
}
