using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Visualizer.Model;
using Visualizer.Responses;
using Visualizer.Util;

namespace Visualizer.ViewModel
{
    public class SystemReportingPaneViewModel : ViewModelBase
    {
        public event EventHandler SaveColumnLayoutEvent;
        private List<TagResult> _tags;
        private List<TagResult> _filteredTagList;
        private List<MicroZoneResult> _microZones;
        private List<MicroZoneResult> _filteredMicroZoneList;
        private ListCollectionView _view;
        private DataManager _dataManager;
        private int filteredCount;
        private int microZoneFilteredCount;

        public EventHandler CollectionChanged;
        public ICollectionView View => this._view;
        public ICommand GenerateSelectedReportCommand { get; set; }

        public DelegateCommand DeleteDataCommand { get; set; }
        public DelegateCommand SyncFeedCommand { get; set; }
        public List<TagResult> SelectedTags { get; set; }

        public SystemReportingPaneViewModel()
        {
            this._dataManager = DataManager.Instance;
            this._dataManager.DataUpdated += this._dataManager_DataUpdated;

            this.DeleteDataCommand = new DelegateCommand(DeleteData);
            this.SyncFeedCommand = new DelegateCommand(SyncFeed);
            this.GenerateSelectedReportCommand = new DelegateCommand(GenerateSelectedReport);

            this.FilteredTagList = new List<TagResult>();
            this.SelectedTags = new List<TagResult>();
            SyncFeed(new object());
        }

        public void SaveColumnLayouts()
        {
            SaveColumnLayoutEvent.Invoke(this, EventArgs.Empty);
        }

        public void GenerateSelectedReport(object action)
        {
            StringBuilder logBuilder = new StringBuilder();

            List<TagResult> reportList = this.SelectedTags;

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
                stringBuilder.Append("Temperature,");
                stringBuilder.Append("Humidity,");
                stringBuilder.AppendLine();

                //sort the tag by cartID
                List<TagResult> sortedList = reportList.OrderBy(x => x.Id).ToList();
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
                    stringBuilder.Append($"{HexToInt(tag.Battery)},");
                    stringBuilder.Append($"{tag.Rssi},");
                    stringBuilder.Append($"{tag.MZone1},");
                    stringBuilder.Append($"{tag.MZone2},");
                    stringBuilder.Append($"{tag.SequenceNumber},");
                    stringBuilder.Append($"{tag.ReaderId},");
                    stringBuilder.Append($"{tag.Temperature},");
                    stringBuilder.Append($"{tag.Humidity},");

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

        protected virtual void OnCollectionChanged(EventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, e);
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

        private void DeleteData(object action)
        {
            if (MessageBox.Show($"Are you sure you want to delete all tag data from the system?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://lrniservice-test.azurewebsites.net/api/sync/DeleteTags");
                    request.ContentType = "text/json";
                    request.Method = "GET";
                    string str = string.Empty;
                    using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
                    {
                        str = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    int test = 0;
                }
            }
        }
       
        public void GenerateReport(object action)
        {
            StringBuilder logBuilder = new StringBuilder();

            List<TagResult> reportList = action as List<TagResult>;

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
                stringBuilder.Append("Temperature,");
                stringBuilder.Append("Humidity,");
                stringBuilder.AppendLine();

                //sort the tag by cartID
                List<TagResult> sortedList = reportList.OrderBy(x => x.Id).ToList();
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
                    stringBuilder.Append($"{HexToInt(tag.Battery)},");
                    stringBuilder.Append($"{tag.Rssi},");
                    stringBuilder.Append($"{tag.MZone1},");
                    stringBuilder.Append($"{tag.MZone2},");
                    stringBuilder.Append($"{tag.SequenceNumber},");
                    stringBuilder.Append($"{tag.ReaderId},");
                    stringBuilder.Append($"{tag.Temperature},");
                    stringBuilder.Append($"{tag.Humidity},");

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

        private void RefreshData()
        {
            try
            {
                List<TagResult> selected = this.SelectedTags;
                this.Tags = new List<TagResult>(this._dataManager.AllTags);
                List<TagResult> mostRecentTags = new List<TagResult>(this._dataManager.MostRecentTags);
                this.MicroZones = new List<MicroZoneResult>(this._dataManager.MicroZones);

                List<TagResult> tagResult = new List<TagResult>();

                foreach (MicroZoneResult result in this.MicroZones)
                {
                    result.TagsCurrentlyInZone.Clear();
                    result.TagsPreviouslyInZone.Clear();

                    tagResult = mostRecentTags.Where(x => x.MZone1 == result.TagAssociationNumber).ToList();

                    if (tagResult.Count > 0)
                    {
                        foreach (TagResult tagsInZoneCurrently in tagResult)
                        {
                            result.TagsCurrentlyInZone.Add(tagsInZoneCurrently);
                        }

                        tagResult.Clear();
                    }

                    tagResult = mostRecentTags.Where(x => x.MZone2 == result.TagAssociationNumber).ToList();

                    if (tagResult.Count > 0)
                    {
                        foreach (TagResult tagsInZonePreviously in tagResult)
                        {
                            result.TagsPreviouslyInZone.Add(tagsInZonePreviously);
                        }

                        tagResult.Clear();
                    }
                }


                this.SelectedTags = selected;
            }
            catch (Exception e)
            {
                string fileName = "\\logs.txt";
                string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fullpath = mydocsPath + fileName;
                File.WriteAllText(fullpath, e.Message);
            }

            OnCollectionChanged(EventArgs.Empty);
            OnPropertyChanged(() => Tags);
            OnPropertyChanged(() => MicroZones);
        }

        private int HexToInt(string hexData)
        {
            int convertedValue = 0;

            if (String.IsNullOrEmpty(hexData))
            {
                return 0;
            }

            try
            {
                convertedValue = Convert.ToSByte(hexData, 16);
                if (convertedValue >= 0x80)
                {
                    return -(convertedValue & 0x7F);
                }
            }
            catch (Exception e)
            {
                return 0;
            }

            return convertedValue;
        }

        public List<TagResult> Tags
        {
            get => this._tags;
            set
            {
                this._tags = value;
                this._view = new ListCollectionView(this._tags);
                OnCollectionChanged(EventArgs.Empty);
                OnPropertyChanged(() => Tags);
                OnPropertyChanged(() => View);
            }
        }

        public List<MicroZoneResult> MicroZones
        {
            get => this._microZones;
            set
            {
                this._microZones = value;
                OnCollectionChanged(EventArgs.Empty);
                OnPropertyChanged(() => MicroZones);
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

        public int FilteredCount
        {
            get
            {
                return this.filteredCount;
            }
            set
            {
                this.filteredCount = value;
                OnPropertyChanged("FilteredCount");
            }
        }

        public int TotalCount
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

        public List<MicroZoneResult> FilteredMicroZoneList
        {
            get
            {
                return this._filteredMicroZoneList;
            }
            set
            {
                this._filteredMicroZoneList = value;
            }
        }

        public int MicroZoneFilteredCount
        {
            get
            {
                return this.microZoneFilteredCount;
            }
            set
            {
                this.microZoneFilteredCount = value;
                OnPropertyChanged("MicroZoneFilteredCount");
            }
        }

        public int MicroZoneTotalCount
        {
            get
            {
                if (this.MicroZones == null)
                {
                    return 0;
                }
                else
                {
                    return this.MicroZones.Count;
                }
            }
        }

        public void UpdateUI()
        {
            OnPropertyChanged("TotalCount");
            OnPropertyChanged("FilteredCount");
            OnPropertyChanged("MicroZoneTotalCount");
            OnPropertyChanged("MicroZoneFilteredCount");
        }
    }
}

