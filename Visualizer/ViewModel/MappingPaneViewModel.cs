using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Visualizer.Model;
using Visualizer.Responses;

namespace Visualizer.ViewModel
{
    public class MappingPaneViewModel : ViewModelBase
    {
        public event EventHandler SaveColumnLayoutEvent;

        private bool updating;

        private const string API_KEY = "AsSN9EZQPu2jmToMf1e-nu691OHhxEzcdG7zaVEB8tQthLtvOTpXV-MNYUwprjWQ";

        private const int DefaultZoomLevel = 0x12;

        public MapCore MapView { get; set; }

        public DelegateCommand LockMapCommand { get; set; }
        public DelegateCommand ClearMapCommand { get; set; }

        public ObservableCollection<TagResult> Tags { get; set; }
        public ObservableCollection<TagResult> GeoMappedTags { get; set; }
        public bool IsCurrentTags { get; set; }
        public bool IsAssets { get; set; }

        public ObservableCollection<TagResult> HistoryTags { get; set; }
        public ObservableCollection<TagResult> GeoMappedHistoryTags { get; set; }
        public ObservableCollection<Asset> Assets { get; set; }
        public ObservableCollection<Asset> GeoMappedAssets { get; set; }
        public ICommand GenerateTagHistoryReportCommand { get; set; }
        public ICommand GenerateCurrentTagReportCommand { get; set; }

        public EventHandler CollectionChanged;
        private DataManager _dataManager;
        private ListCollectionView _view;
        private TagResult _selectedTag;
        private List<TagResult> _selectedTags;
        private Asset _selectedAsset;
        private List<Asset> _selectedAssets;
        private TagResult _selectedHistoryTag;
        private List<TagResult> _selectedHistoryTags;
        private TagResult _foundTag;
        private string _latitude;
        private string _longitude;
        private bool _isMapLocked;
        private bool _isGeoMappingEnabled;
        private bool _isLoading;
        private string _selectedGeoMappingDistance;
        private string[] _geoMappingDistances;
        private int _selectedTabIndex;
        private double _zoomLevel;
        private List<TagResult> _tagHistoryfilteredTagList;
        private List<TagResult> _currentTagsfilteredTagList;

        public MappingPaneViewModel()
        {
            this.IsMapLocked = false;

            this._dataManager = DataManager.Instance;
            this._dataManager.DataUpdated += new EventHandler(this._dataManager_DataUpdated);

            this.Tags = new ObservableCollection<TagResult>(_dataManager.MostRecentTags);
            this.HistoryTags = new ObservableCollection<TagResult>(_dataManager.AllTags);

            //make a list of assets for all the tags
            UpdateAssetsList();

            this.GeoMappedTags = new ObservableCollection<TagResult>();
            this._selectedTags = new List<TagResult>();
            this._tagHistoryfilteredTagList = new List<TagResult>();
            this._currentTagsfilteredTagList = new List<TagResult>();
            this._selectedHistoryTags = new List<TagResult>();
            this._selectedAssets = new List<Asset>();

            this.LockMapCommand = new DelegateCommand(LockMap);
            this.ClearMapCommand = new DelegateCommand(ClearMap);
            this.GenerateTagHistoryReportCommand = new DelegateCommand(GenerateTagHistoryReport);
            this.GenerateCurrentTagReportCommand = new DelegateCommand(GenerateCurrentTagReport);

            this._geoMappingDistances = new string[] { "1000 ft", "5 mi", "10 mi", "15 mi", "25 mi", "75 mi", "100 mi", "125 mi", "150 mi" };
            this._isLoading = true;
            this.SelectedTabIndex = 0;
            this.ZoomLevel = 18.0;
            OnPropertyChanged("SelectedTabIndex"); 
            OnPropertyChanged(() => Assets);
        }

        public void SaveColumnLayouts()
        {
            SaveColumnLayoutEvent.Invoke(this, EventArgs.Empty);
        }

        public void GenerateCurrentTagReport(object action)
        {
            StringBuilder logBuilder = new StringBuilder();

            List<TagResult> reportList = this.SelectedTags;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Tags");
                stringBuilder.AppendLine();
                stringBuilder.Append("Tag Id,");
                stringBuilder.Append("Tag Name,");
                stringBuilder.Append("Updated On Server,");
                stringBuilder.Append("Latitude,");
                stringBuilder.Append("Longitude,");
                stringBuilder.Append("Rssi,");
                stringBuilder.Append("Reader Id,");
                stringBuilder.Append("Current Mzone,");
                stringBuilder.Append("Previous Mzone,");
                stringBuilder.AppendLine();

                //sort the tag by cartID
                List<TagResult> sortedList = reportList.OrderBy(x => x.Id).ToList();
                sortedList.Sort((tagA, tagB) => tagB.Id.CompareTo(tagA.Id));
                sortedList.Reverse();

                foreach (TagResult tag in sortedList)
                {
                    stringBuilder.Append($"{tag.Id.ToUpper()},");
                    stringBuilder.Append($"{tag.Name},");
                    stringBuilder.Append($"{tag.LastUpdatedOnServer.ToString("u", DateTimeFormatInfo.InvariantInfo)},");
                    stringBuilder.Append($"{tag.Latitude},");
                    stringBuilder.Append($"{tag.Longitude},");
                    stringBuilder.Append($"{tag.Rssi},");
                    stringBuilder.Append($"{tag.ReaderId},");
                    stringBuilder.Append($"{tag.MZone1},");
                    stringBuilder.Append($"{tag.MZone2},");

                    stringBuilder.AppendLine();
                }

                RadSaveFileDialog saveFileDialog = new RadSaveFileDialog();
                saveFileDialog.ExpandToCurrentDirectory = true;
                saveFileDialog.InitialSelectedLayout = Telerik.Windows.Controls.FileDialogs.LayoutType.Details;
                saveFileDialog.InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory);
                saveFileDialog.CustomPlaces.Add(Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory), "Users", Environment.UserName, "Desktop"));
                saveFileDialog.Filter = "CSV files | *.csv";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.BringToFront();
                saveFileDialog.BringIntoView();
                saveFileDialog.ShowDialog();

                if (saveFileDialog.DialogResult == true)
                {
                    FileInfo file = new FileInfo(saveFileDialog.FileName);

                    if (file.Exists)
                    {
                        if (!IsFileLocked(file))
                            File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
                    }
                    else
                    {
                        File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
                    }
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

        public void GenerateTagHistoryReport(object action)
        {
            StringBuilder logBuilder = new StringBuilder();

            List<TagResult> reportList = this.SelectedHistoryTags;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Tags");
                stringBuilder.AppendLine();
                stringBuilder.Append("Tag Id,");
                stringBuilder.Append("Tag Name,");
                stringBuilder.Append("Updated On Server,");
                stringBuilder.Append("Latitude,");
                stringBuilder.Append("Longitude,");
                stringBuilder.Append("Rssi,");
                stringBuilder.Append("Reader Id,");
                stringBuilder.Append("Current Mzone,");
                stringBuilder.Append("Previous Mzone,");
                stringBuilder.AppendLine();

                //sort the tag by cartID
                List<TagResult> sortedList = reportList.OrderBy(x => x.Id).ToList();
                sortedList.Sort((tagA, tagB) => tagB.Id.CompareTo(tagA.Id));
                sortedList.Reverse();

                foreach (TagResult tag in sortedList)
                {
                    stringBuilder.Append($"{tag.Id.ToUpper()},");
                    stringBuilder.Append($"{tag.Name},");
                    stringBuilder.Append($"{tag.LastUpdatedOnServer.ToString("u", DateTimeFormatInfo.InvariantInfo)},");
                    stringBuilder.Append($"{tag.Latitude},");
                    stringBuilder.Append($"{tag.Longitude},");
                    stringBuilder.Append($"{tag.Rssi},");
                    stringBuilder.Append($"{tag.ReaderId},");
                    stringBuilder.Append($"{tag.MZone1},");
                    stringBuilder.Append($"{tag.MZone2},");

                    stringBuilder.AppendLine();
                }

                RadSaveFileDialog saveFileDialog = new RadSaveFileDialog();
                saveFileDialog.ExpandToCurrentDirectory = true;
                saveFileDialog.InitialSelectedLayout = Telerik.Windows.Controls.FileDialogs.LayoutType.Details;
                saveFileDialog.InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory);
                saveFileDialog.CustomPlaces.Add(Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory), "Users", Environment.UserName, "Desktop"));
                saveFileDialog.Filter = "CSV files | *.csv";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.BringToFront();
                saveFileDialog.BringIntoView();
                saveFileDialog.ShowDialog();

                if (saveFileDialog.DialogResult == true)
                {
                    FileInfo file = new FileInfo(saveFileDialog.FileName);

                    if (file.Exists)
                    {
                        if (!IsFileLocked(file))
                            File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
                    }
                    else
                    {
                        File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
                    }
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

        public void GenerateTagHistoryFilteredReport(object action)
        {
            StringBuilder logBuilder = new StringBuilder();

            List<TagResult> reportList = action as List<TagResult>;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Tags");
                stringBuilder.AppendLine();
                stringBuilder.Append("Tag Id,");
                stringBuilder.Append("Tag Name,");
                stringBuilder.Append("Updated On Server,");
                stringBuilder.Append("Latitude,");
                stringBuilder.Append("Longitude,");
                stringBuilder.Append("Rssi,");
                stringBuilder.Append("Reader Id,");
                stringBuilder.Append("Current Mzone,");
                stringBuilder.Append("Previous Mzone,");
                stringBuilder.AppendLine();

                //sort the tag by cartID
                List<TagResult> sortedList = reportList.OrderBy(x => x.Id).ToList();
                sortedList.Sort((tagA, tagB) => tagB.Id.CompareTo(tagA.Id));
                sortedList.Reverse();

                foreach (TagResult tag in sortedList)
                {
                    stringBuilder.Append($"{tag.Id.ToUpper()},");
                    stringBuilder.Append($"{tag.Name},");
                    stringBuilder.Append($"{tag.LastUpdatedOnServer.ToString("u", DateTimeFormatInfo.InvariantInfo)},");
                    stringBuilder.Append($"{tag.Latitude},");
                    stringBuilder.Append($"{tag.Longitude},");
                    stringBuilder.Append($"{tag.Rssi},");
                    stringBuilder.Append($"{tag.ReaderId},");
                    stringBuilder.Append($"{tag.MZone1},");
                    stringBuilder.Append($"{tag.MZone2},");

                    stringBuilder.AppendLine();
                }

                RadSaveFileDialog saveFileDialog = new RadSaveFileDialog();
                saveFileDialog.ExpandToCurrentDirectory = true;
                saveFileDialog.InitialSelectedLayout = Telerik.Windows.Controls.FileDialogs.LayoutType.Details;
                saveFileDialog.InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory);
                saveFileDialog.CustomPlaces.Add(Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory), "Users", Environment.UserName, "Desktop"));
                saveFileDialog.Filter = "CSV files | *.csv";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.BringToFront();
                saveFileDialog.BringIntoView();
                saveFileDialog.ShowDialog();

                if (saveFileDialog.DialogResult == true)
                {
                    FileInfo file = new FileInfo(saveFileDialog.FileName);

                    if (file.Exists)
                    {
                        if (!IsFileLocked(file))
                            File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
                    }
                    else
                    {
                        File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
                    }
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

        public void GenerateCurrentTagFilteredReport(object action)
        {
            StringBuilder logBuilder = new StringBuilder();

            List<TagResult> reportList = action as List<TagResult>;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Tags");
                stringBuilder.AppendLine();
                stringBuilder.Append("Tag Id,");
                stringBuilder.Append("Tag Name,");
                stringBuilder.Append("Updated On Server,");
                stringBuilder.Append("Latitude,");
                stringBuilder.Append("Longitude,");
                stringBuilder.Append("Rssi,");
                stringBuilder.Append("Reader Id,");
                stringBuilder.Append("Current Mzone,");
                stringBuilder.Append("Previous Mzone,");
                stringBuilder.AppendLine();

                //sort the tag by cartID
                List<TagResult> sortedList = reportList.OrderBy(x => x.Id).ToList();
                sortedList.Sort((tagA, tagB) => tagB.Id.CompareTo(tagA.Id));
                sortedList.Reverse();

                foreach (TagResult tag in sortedList)
                {
                    stringBuilder.Append($"{tag.Id.ToUpper()},");
                    stringBuilder.Append($"{tag.Name},");
                    stringBuilder.Append($"{tag.LastUpdatedOnServer.ToString("u", DateTimeFormatInfo.InvariantInfo)},");
                    stringBuilder.Append($"{tag.Latitude},");
                    stringBuilder.Append($"{tag.Longitude},");
                    stringBuilder.Append($"{tag.Rssi},");
                    stringBuilder.Append($"{tag.ReaderId},");
                    stringBuilder.Append($"{tag.MZone1},");
                    stringBuilder.Append($"{tag.MZone2},");
                    stringBuilder.AppendLine();
                }

                RadSaveFileDialog saveFileDialog = new RadSaveFileDialog();
                saveFileDialog.ExpandToCurrentDirectory = true;
                saveFileDialog.InitialSelectedLayout = Telerik.Windows.Controls.FileDialogs.LayoutType.Details;
                saveFileDialog.InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory);
                saveFileDialog.CustomPlaces.Add(Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory), "Users", Environment.UserName, "Desktop"));
                saveFileDialog.Filter = "CSV files | *.csv";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.BringToFront();
                saveFileDialog.BringIntoView();
                saveFileDialog.ShowDialog();

                if (saveFileDialog.DialogResult == true)
                {
                    FileInfo file = new FileInfo(saveFileDialog.FileName);

                    if (file.Exists)
                    {
                        if (!IsFileLocked(file))
                            File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
                    }
                    else
                    {
                        File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
                    }
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

        public List<TagResult> TagHistoryFilteredList
        {
            get
            {
                return this._tagHistoryfilteredTagList;
            }
            set
            {
                this._tagHistoryfilteredTagList = value;
            }
        }

        public List<TagResult> CurrentTagsFilteredList
        {
            get
            {
                return this._currentTagsfilteredTagList;
            }
            set
            {
                this._currentTagsfilteredTagList = value;
            }
        }

        private void UpdateAssetsList()
        {

            this.Assets = new ObservableCollection<Asset>();
            foreach (TagResult tag in this.Tags)
            {
                if (tag.AssociatedAssets.Count > 0)
                {
                    foreach (Asset asset in tag.AssociatedAssets)
                    {
                        this.Assets.Add(asset);
                    }
                }
            }
        }

        private void _dataManager_DataUpdated(object sender, EventArgs e)
        {
            UpdateData();
        }

        private Uri BuildMapLocationUri(string mapLocation)
        {
            string str = Uri.EscapeDataString(mapLocation);
            return new Uri("http://dev.virtualearth.net/REST/v1/Locations?q=" + str + $"&key={API_KEY}");
        }

        protected virtual void OnCollectionChanged(EventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, e);
            }
        }

        private string BuildToolTipContent(TagResult tag)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Name: {tag.Name}, ID: {tag.Id}");
            builder.AppendLine();
            builder.Append($"Latitude: {tag.Latitude}, Longitude: {tag.Longitude}");
            return builder.ToString();
        }

        private void ClearMap(object action)
        {
        }

        private void LockMap(object action)
        {
            this.IsMapLocked = !this.IsMapLocked;
        }

        private void MapLocation(TagResult tag)
        {
            if (ReferenceEquals(tag, null))
            {
                MessageBox.Show("No tag was selected or found", "No Tag Selected");
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(tag.Latitude) || string.IsNullOrEmpty(tag.Longitude))
                    {
                        //MessageBox.Show("Latitude/Longitude is not set", "GPS Points Not Valid");
                    }
                    else
                    {
                        if (this.IsGeoMappingEnabled)
                        {
                            TagResult selectedTag = new TagResult();

                            if (IsCurrentTags)
                            {
                                selectedTag = this.SelectedTag;
                            }
                            else
                            {
                                selectedTag = this.SelectedHistoryTag;
                            }

                            if (this.SelectedGeoMappingDistance != null && selectedTag != null)
                            {
                                List<TagResult> mappedTags = new List<TagResult>();
                                Location location = new Location(Convert.ToDouble(tag.Latitude), Convert.ToDouble(tag.Longitude));
                                this.MapView.Center = location;
                                this.MapView.ZoomLevel = this.ZoomLevel;
                                Pushpin element = new Pushpin
                                {
                                    Location = location
                                };
                                ToolTip tip1 = new ToolTip();
                                tip1.Content = this.BuildToolTipContent(tag);
                                element.ToolTip = tip1;
                                this.MapView.Children.Add(element);

                                List<TagResult> tagsToSearch = new List<TagResult>();
                                if (this.IsCurrentTags)
                                    tagsToSearch = this.Tags.ToList();
                                else
                                    tagsToSearch = this.HistoryTags.ToList();

                                foreach (TagResult t in tagsToSearch)
                                {
                                    //ugly way of making sure these are empty
                                    if (t.Latitude == "")
                                        t.Latitude = "0";
                                    if (t.Longitude == "")
                                        t.Longitude = "0";

                                    if (TagIsWithinSetDistance(Convert.ToDouble(selectedTag.Latitude), Convert.ToDouble(selectedTag.Longitude), Convert.ToDouble(t.Latitude), Convert.ToDouble(t.Longitude)))
                                    {
                                        Location locationInZone = new Location(Convert.ToDouble(t.Latitude), Convert.ToDouble(t.Longitude));
                                        this.MapView.Center = locationInZone;
                                        this.MapView.ZoomLevel = this.ZoomLevel;
                                        Pushpin elementInZone = new Pushpin
                                        {
                                            Location = locationInZone
                                        };
                                        ToolTip tip2 = new ToolTip();
                                        tip2.Content = this.BuildToolTipContent(t);
                                        elementInZone.ToolTip = tip2;
                                        this.MapView.Children.Add(elementInZone);
                                        mappedTags.Add(t);
                                    }
                                }

                                if (IsCurrentTags)
                                {
                                    this.GeoMappedTags = new ObservableCollection<TagResult>(mappedTags);
                                    OnPropertyChanged(() => GeoMappedTags);
                                }
                                else
                                {
                                    this.GeoMappedHistoryTags = new ObservableCollection<TagResult>(mappedTags);
                                    OnPropertyChanged(() => GeoMappedHistoryTags);
                                }
                            }
                        }
                        else
                        {
                            Location location = new Location(Convert.ToDouble(tag.Latitude), Convert.ToDouble(tag.Longitude));
                            this.MapView.Center = location;
                            this.MapView.ZoomLevel = this.ZoomLevel;
                            Pushpin element = new Pushpin
                            {
                                Location = location
                            };
                            ToolTip tip1 = new ToolTip();
                            tip1.Content = this.BuildToolTipContent(tag);
                            element.ToolTip = tip1;
                            this.MapView.Children.Add(element);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error Mapping Location", "Mapping Error");
                }
            }
        }

        private void MapSelectedTag(TagResult tag)
        {
            this.MapView.Children.Clear();
            this.MapLocation(tag);
        }

        public void MapSelectedTags(List<TagResult> tags)
        {
            if (this.MapView != null)
            {
                this.MapView.Children.Clear();

                foreach (TagResult tag in tags)
                {
                    this.MapLocation(tag);
                }
            }
        }

        public void MapSelectedAssets(List<Asset> assets)
        {
            if (this.MapView != null)
            {
                this.MapView.Children.Clear();

                foreach (Asset asset in assets)
                {
                    string tagId = this.Tags.Where(x => x.AssociatedAssets.Where(x => x.AssetId == asset.AssetId) != null).FirstOrDefault().Id;
                    List<TagResult> tags = this.HistoryTags.Where(x => x.Id == tagId).ToList();
                    this.MapSelectedTags(tags);
                }
            }
        }

        private void UpdateData()
        {
            updating = true;

            List<TagResult> tagsSelected = new List<TagResult>();
            TagResult selectedResult = new TagResult();

            if (this._isLoading)
            {
                tagsSelected = this.SelectedTags;
                selectedResult = this.SelectedTag;

                List<TagResult> tags = this._dataManager.MostRecentTags;
                List<TagResult> tagList = new List<TagResult>();

                foreach (TagResult tag in tags)
                {
                    if (!tagList.Contains(tag) && tag.MZone1Rssi != "-127" && tag.Latitude != null && tag.Longitude != null && tag.Latitude != "" && tag.Longitude != "")
                        tagList.Add(tag);
                }

                this.Tags = new ObservableCollection<TagResult>(tagList);
                this.SelectedTags = tagsSelected;
                this.SelectedTag = selectedResult;

                if (tagsSelected != null && tagsSelected.Count > 0)
                {
                    MapSelectedTags(tagsSelected);
                }

                OnPropertyChanged(() => SelectedTag);
                OnPropertyChanged(() => SelectedTags);
                OnPropertyChanged(() => Tags);
                OnCollectionChanged(EventArgs.Empty);

                tagsSelected = this.SelectedHistoryTags;
                selectedResult = this.SelectedHistoryTag;

                tags = this._dataManager.AllTags;
                tagList = new List<TagResult>();

                foreach (TagResult tag in tags)
                {
                    if (!tagList.Contains(tag) && tag.MZone1Rssi != "-127" && tag.Latitude != null && tag.Longitude != null && tag.Latitude != "" && tag.Longitude != "")
                        tagList.Add(tag);
                }

                this.HistoryTags = new ObservableCollection<TagResult>(tagList);
                this.SelectedHistoryTags = tagsSelected;
                this.SelectedHistoryTag = selectedResult;

                if (tagsSelected != null && tagsSelected.Count > 0)
                {
                    MapSelectedTags(tagsSelected);
                }

                UpdateAssetsList();

                OnPropertyChanged(() => Assets);
                OnPropertyChanged(() => SelectedHistoryTag);
                OnPropertyChanged(() => SelectedHistoryTags);
                OnPropertyChanged(() => HistoryTags);
                OnPropertyChanged(() => Tags);
                OnCollectionChanged(EventArgs.Empty);

                this._isLoading = false;
            }
            else if (IsCurrentTags)
            {
                tagsSelected = this.SelectedTags;
                selectedResult = this.SelectedTag;

                List<TagResult> tags = this._dataManager.MostRecentTags;
                List<TagResult> tagList = new List<TagResult>();

                foreach (TagResult tag in tags)
                {
                    if (!tagList.Contains(tag) && tag.MZone1Rssi != "-127" && tag.Latitude != null && tag.Longitude != null && tag.Latitude != "" && tag.Longitude != "")
                        tagList.Add(tag);
                }

                this.Tags = new ObservableCollection<TagResult>(tagList);
                this.SelectedTags = tagsSelected;
                this.SelectedTag = selectedResult;

                if (tagsSelected != null)
                {
                    //MapSelectedTags(tagsSelected);
                }

                UpdateAssetsList();

                OnPropertyChanged(() => Assets);
                OnPropertyChanged(() => SelectedTag);
                OnPropertyChanged(() => SelectedTags);
                OnPropertyChanged(() => Tags);
                OnCollectionChanged(EventArgs.Empty);
            }
            else if(IsAssets)
            {
                List<TagResult> tags = this._dataManager.AllTags;
                this.Tags = new ObservableCollection<TagResult>(this._dataManager.MostRecentTags);
                UpdateAssetsList();

                List<TagResult> tagList = new List<TagResult>();

                foreach (TagResult tag in tags)
                {
                    if (!tagList.Contains(tag) && tag.MZone1Rssi != "-127" && tag.Latitude != null && tag.Longitude != null && tag.Latitude != "" && tag.Longitude != "")
                        tagList.Add(tag);
                }

                this.HistoryTags = new ObservableCollection<TagResult>(tagList);

                if (this.SelectedAssets != null)
                {
                    MapSelectedAssets(this.SelectedAssets);
                }

                OnPropertyChanged(() => Assets);
                OnPropertyChanged(() => SelectedHistoryTag);
                OnPropertyChanged(() => SelectedHistoryTags);
                OnPropertyChanged(() => HistoryTags);
                OnCollectionChanged(EventArgs.Empty);
            }
            else
            {
                tagsSelected = this.SelectedHistoryTags;
                selectedResult = this.SelectedHistoryTag;

                List<TagResult> tags = this._dataManager.AllTags;
                List<TagResult> tagList = new List<TagResult>();

                foreach (TagResult tag in tags)
                {
                    if (!tagList.Contains(tag) && tag.MZone1Rssi != "-127" && tag.Latitude != null && tag.Longitude != null && tag.Latitude != "" && tag.Longitude != "")
                        tagList.Add(tag);
                }

                this.HistoryTags = new ObservableCollection<TagResult>(tagList);
                this.SelectedHistoryTags = tagsSelected;
                this.SelectedHistoryTag = selectedResult;

                if (tagsSelected != null)
                {
                    //MapSelectedTags(tagsSelected);
                }

                UpdateAssetsList();

                OnPropertyChanged(() => Assets);
                OnPropertyChanged(() => SelectedHistoryTag);
                OnPropertyChanged(() => SelectedHistoryTags);
                OnPropertyChanged(() => HistoryTags);
                OnCollectionChanged(EventArgs.Empty);
            }
            updating = false;
        }

        public double ZoomLevel
        {
            get {  return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                OnPropertyChanged("ZoomLevel");
            }
        }

        public bool IsMapLocked
        {
            get =>
                this._isMapLocked;
            set
            {
                this._isMapLocked = value;
                OnPropertyChanged(() => IsMapLocked);
            }
        }

        public string Latitude
        {
            get =>
                this._latitude;
            set
            {
                this._latitude = value;
                OnPropertyChanged(() => Latitude);
            }
        }

        public string Longitude
        {
            get =>
                this._longitude;
            set
            {
                this._longitude = value;
                OnPropertyChanged(() => Longitude);
            }
        }

        public TagResult SelectedTag
        {
            get =>
                this._selectedTag;
            set
            {
                this._selectedTag = value;
                this.OnPropertyChanged(() => SelectedTag);
                if (this._selectedTag != null && !updating)
                {
                    this.MapSelectedTag(this._selectedTag);
                }
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

        public Asset SelectedAsset
        {
            get =>
                this._selectedAsset;
            set
            {
                this._selectedAsset = value;
                this.OnPropertyChanged(() => SelectedAsset);
                if (this._selectedAsset != null && !updating)
                {
                    string tagId = this.Tags.Where(x => x.AssociatedAssets.Where(x => x.AssetId == this._selectedAsset.AssetId) != null).FirstOrDefault().Id;
                    List<TagResult> tags = this.HistoryTags.Where(x => x.Id == tagId).ToList();

                    foreach (TagResult tag in tags)
                    {
                        this.MapSelectedTag(tag);
                    }
                }
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

        public TagResult SelectedHistoryTag
        {
            get =>
                this._selectedHistoryTag;
            set
            {
                this._selectedHistoryTag = value;
                this.OnPropertyChanged(() => SelectedHistoryTag);
                if (this._selectedHistoryTag != null && !updating)
                {
                    this.MapSelectedTag(this._selectedHistoryTag);
                }
            }
        }

        public List<TagResult> SelectedHistoryTags
        {
            get
            {
                return this._selectedHistoryTags;
            }
            set
            {
                this._selectedHistoryTags = value;
                OnPropertyChanged(() => SelectedHistoryTags);
            }
        }

        public TagResult FoundTag
        {
            get =>
                this._foundTag;
            set
            {
                this._foundTag = value;
                OnPropertyChanged(() => FoundTag);
            }
        }

        public int SelectedTabIndex
        {
            get { return this._selectedTabIndex; }
            set
            {
                this._selectedTabIndex = value;

                switch (value)
                {
                    case 0:
                        this.IsCurrentTags = true;
                        break;
                    case 1:
                        this.IsCurrentTags = false;
                        break;
                    case 2:
                        this.IsCurrentTags = false;
                        this.IsAssets = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public string SelectedGeoMappingDistance
        {
            get =>
                this._selectedGeoMappingDistance;
            set
            {
                this._selectedGeoMappingDistance = value;
                OnPropertyChanged(() => SelectedGeoMappingDistance);
            }
        }

        private bool TagIsWithinSetDistance(double lat1, double long1, double lat2, double long2)
        {
            if (lat1 == 0 || long1 == 0 || lat2 == 0 || long2 == 0)
            {
                return false;
            }

            int convertedDistance = 0;
            double longitude = (long2 - long1) * (Math.PI / 180);
            double latitude = (lat2 - lat1) * (Math.PI / 180);
            double a = Math.Pow(Math.Sin(latitude / 2.0), 2) + Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) * Math.Pow(Math.Sin(longitude / 2.0), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = 3956 * c;


            // Process for feet
            if (this.SelectedGeoMappingDistance.Contains("ft") == true)
            {
                // 0.189 represents the number of miles in 1000 ft (the user can select 1000 ft)
                distance = distance * 0.189;
                convertedDistance = Convert.ToInt32(this.SelectedGeoMappingDistance.Trim('f', 't'));
            }
            if (this.SelectedGeoMappingDistance.Contains("mi") == true)
            {
                convertedDistance = Convert.ToInt32(this.SelectedGeoMappingDistance.Trim('m', 'i'));
            }

            return distance <= convertedDistance;
        }

        public string[] GeoMappingDistances
        {
            get
            {
                return _geoMappingDistances;
            }
        }


        public bool IsGeoMappingEnabled
        {
            get
            {
                return this._isGeoMappingEnabled;
            }
            set
            {
                this._isGeoMappingEnabled = value;
                OnPropertyChanged(() => IsGeoMappingEnabled);
            }
        }
    }
}
