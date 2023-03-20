using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Visualizer.Model;
using Visualizer.Responses;
using Visualizer.Settings;
using Visualizer.Threading;
using Visualizer.Util;
using System.Drawing.Imaging;
using Microsoft.Office.Interop.Word;

namespace Visualizer.ViewModel
{
    public class InventoryManagementPaneViewModel : ViewModelBase
    {
        private DepartmentViewModel _currentDepartment;
        private bool _isRunning;
        private DelegateMarshaler _delegateMarshaler;
        private string _locationImage;
        private string _lastSyncedTime;
        private string _selectedShape;
        private int _syncTime;
        private int _rotation;
        private int _syncTimeUserSet;
        private DataManager _dataManager;
        private int _imageHeight;
        private int _imageWidth;
        private BitmapSource _imageSource;
        
        public double AccumulatedX { get; set; }
        public double AccumulatedY { get; set; }
        public event EventHandler SystemSynced;
        public ObservableCollection<MicroZoneViewModel> MicroZones { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
        public List<string> Shapes { get; set; }
        public DepartmentViewModel SelectedDepartment { get; set; }
        public DelegateCommand SyncSystemCommand { get; set; }
        public DelegateCommand SetSyncIntervalCommand { get; set; }
        public DelegateCommand AddMicroZoneCommand { get; set; }
        public DelegateCommand LoadDepartmentCommand { get; set; }

        public string SelectedShape
        {
            get
            {
                return _selectedShape;
            }
            set
            {
                _selectedShape = value;

                if (value == "Rectangle")
                {
                    foreach (MicroZoneViewModel zone in this.MicroZones)
                    {
                        if (zone.IsSelected)
                        {
                            zone.CornerRadius = 10;
                        }
                    }
                }

                if (value == "Circle")
                {
                    foreach (MicroZoneViewModel zone in this.MicroZones)
                    {
                        if (zone.IsSelected)
                        {
                            zone.CornerRadius = 360;
                        }
                    }
                }

                OnPropertyChanged("MicroZones");
            }
        }

        public int Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;

                foreach (MicroZoneViewModel zone in this.MicroZones)
                {
                    if (zone.IsSelected)
                    {
                        zone.Rotation = value;
                    }
                }
            }
        }


        public InventoryManagementPaneViewModel()
        {
            this.SyncTime = 120;
            this._syncTimeUserSet = this.SyncTime;

            this.SyncSystemCommand = new DelegateCommand(SyncSystem);
            this.SetSyncIntervalCommand = new DelegateCommand(SetSyncInterval);
            this.AddMicroZoneCommand = new DelegateCommand(AddMicroZone);
            this.LoadDepartmentCommand = new DelegateCommand(LoadDepartment);

            this.Departments = new List<DepartmentViewModel>();
            this.Shapes = new List<string>() { "Rectangle", "Circle" };
            this.MicroZones = new ObservableCollection<MicroZoneViewModel>();
            this.PropertyChanged += MicroZoneAssetLayoutViewModel_PropertyChanged;
            this._delegateMarshaler = DelegateMarshaler.Create();

            //create instance of dataManager
            this._dataManager = DataManager.Instance;

            //get all departments
            List<DepartmentResult> departmentResults = this._dataManager.GetDepartments();

            DepartmentViewModel blankVM = new DepartmentViewModel()
            {
                DepartmentId = 0,
                Name = "Add Department",
                FilePath = "",
                ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight,
                ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth

            };

            this.Departments.Add(blankVM);

            foreach (DepartmentResult result in departmentResults)
            {
                DepartmentViewModel vm = new DepartmentViewModel()
                {
                    DepartmentId = result.DepartmentId,
                    Name = result.Name,
                    FilePath = result.FilePath,
                    ScreenHeight = result.ScreenHeight,
                    ScreenWidth = result.ScreenWidth
                };

                this.Departments.Add(vm);

                if (result.IsLastLoaded)
                {
                    this.SelectedDepartment = vm;
                    LoadDepartment(null);
                }
            }

            this._dataManager.DataUpdated += new EventHandler(this._dataManager_DataUpdated);
        }

        private async void AddDepartment(DepartmentViewModel dept)
        {
            string response = await this._dataManager.AddDepartment(dept);

            if (response == "failure")
            {
                MessageBox.Show("Failed to add department. Check server connection.", "Error", MessageBoxButton.OK);
            }
        }

        public void AddMicroZone(object action)
        {
            RadWindow.Prompt("Enter the full Tag ID for this MicroZone:", this.OnAddMicroZoneClosed);
        }

        private async void OnAddMicroZoneClosed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                int deptId = this.CurrentDepartment == null ? 0 : this.CurrentDepartment.DepartmentId;

                if(e.PromptResult.Length < 6)
                {
                    AddMicroZone(null);
                }

                else
                {
                    string tagAssociationNumber = e.PromptResult.Substring(e.PromptResult.Length - 6, 6);

                    MicroZoneViewModel newMzone = new MicroZoneViewModel(null, 0, 0, 0, 0, false)
                    {
                        Name = "mZone " + tagAssociationNumber,
                        RawId = e.PromptResult,
                        DepartmentId = deptId,
                        TagAssociationNumber = tagAssociationNumber
                    };

                    newMzone.Width = 100;
                    newMzone.Height = 100;

                    string response = await this._dataManager.AddOrEditMicroZone(newMzone);

                    this.MicroZones.Add(newMzone);
                    OnPropertyChanged("MicroZones");
                }
               
            }
        }

        private void _dataManager_DataUpdated(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            foreach (MicroZoneViewModel mZoneVM in this.MicroZones)
            {
                mZoneVM.Assets.Clear();
                foreach (TagResult tag in this._dataManager.MostRecentTags)
                {
                    if (tag.MZone1 == mZoneVM.TagAssociationNumber)
                    {
                        AssetViewModel vm = new AssetViewModel()
                        {
                            Name = tag.Name,
                            AssetId = tag.Id,
                            DeptName = this.Departments.Where(x => x.DepartmentId == mZoneVM.DepartmentId).FirstOrDefault().Name,
                            LastUpdatedOn = tag.LastUpdatedOnServer,
                            LocationX = 0,
                            LocationY = 0,
                            MicroZoneCurrent = tag.MZone1,
                            MicroZonePrevious = tag.MZone2,
                            MicroZoneName = tag.MZone1Name,
                            IsSelected = false,
                            IsInMotion = true,
                            IsStationary = false,
                            IsAlarmEnabled = false,
                            IsButtonEvent = false,
                            IsDoubleTap = false,
                            IsEmployee = true,
                            IsMachine = false,
                            IsMissing = false
                        };

                        mZoneVM.Assets.Add(vm);
                    }
                    else if(tag.Id == mZoneVM.RawId)
                    {
                        //if these match, this tag is the mZone, update it's name in case in got changed
                        //this is a work around to having to update/merge tables when editting tags
                        //mZoneVM.Name = tag.Name;
                    }
                }
            }

            OnPropertyChanged("MicroZones");
        }

        void MicroZoneAssetLayoutViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            foreach (MicroZoneViewModel microZone in this.MicroZones)
            {
                microZone.UpdateMicroZoneProperties();
            }
        }

        // Updates x,y position of Microzone
        public async void microZone_LayoutUpdated(object sender, EventArgs e)
        { 
            MicroZoneViewModel microZone = (MicroZoneViewModel)sender;

            string response = await this._dataManager.AddOrEditMicroZone(microZone);
            this.MicroZones.Where(x => x.RawId == microZone.RawId).FirstOrDefault().Name = microZone.Name;
            OnPropertyChanged("MicroZones");
        }

        public void LoadDepartment(object action)
        {
            if (this.SelectedDepartment != null)
            {
                this.MicroZones.Clear();

                if (this.SelectedDepartment.Name == "Add Department")
                {
                    LoadDepartmentImage();
                }
                else
                {
                    this.CurrentDepartment = this.SelectedDepartment;

                    string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Departments");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string[] fileNames = Directory.GetFiles(path);
                    string foundFilePath = "";

                    foreach (string fileName in fileNames)
                    {
                        if (Path.GetFileName(fileName) == this.CurrentDepartment.FilePath)
                        {
                            foundFilePath = fileName;
                        }
                    }

                    if (foundFilePath != "")
                    {
                        this.LocationImage = foundFilePath;
                        Bitmap bmp = new Bitmap(this.LocationImage);
                        this.ImageHeight = bmp.Height;
                        this.ImageWidth = bmp.Width;

                        BitmapImage bitmapImage = new BitmapImage();
                        using (MemoryStream memory = new MemoryStream())
                        {
                            bmp.Save(memory, ImageFormat.Bmp);
                            memory.Position = 0;
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memory;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                        }

                        this.ImageSource = bitmapImage;

                        List<MicroZoneResult> mZoneResults = this._dataManager.GetMicroZones();

                        foreach (MicroZoneResult result in mZoneResults)
                        {
                            if (result.DepartmentId == this.CurrentDepartment.DepartmentId)
                            {
                                // Get the current screen resolution
                                double screenWidth = SystemParameters.PrimaryScreenWidth;
                                double screenHeight = SystemParameters.PrimaryScreenHeight;

                                MicroZoneViewModel mZoneVM = new MicroZoneViewModel(null, result.MicroZoneX, result.MicroZoneY, result.MicroZoneWidth, result.MicroZoneHeight, false)
                                {
                                    DepartmentId = result.DepartmentId,
                                    MicroZoneId = result.MicroZoneId.ToString(),
                                    Name = result.MicroZoneName,
                                    LocationX = result.MicroZoneX,
                                    LocationY = result.MicroZoneY,
                                    CenterX = result.MicroZoneX + (result.MicroZoneWidth / 2),
                                    CenterY = result.MicroZoneY + (result.MicroZoneHeight / 2),
                                    OriginalCenterX = result.MicroZoneX + (result.MicroZoneWidth / 2),
                                    OriginalCenterY = result.MicroZoneY + (result.MicroZoneHeight / 2),
                                    Height = result.MicroZoneHeight,
                                    Width = result.MicroZoneWidth,
                                    IsLocked = result.IsLocked,
                                    TagAssociationNumber = result.TagAssociationNumber,
                                    RawId = result.RawId
                                };

                                //if (screenWidth != this.CurrentDepartment.ScreenWidth  || screenHeight != this.CurrentDepartment.ScreenHeight)
                                //{
                                //    double originalX = mZoneVM.LocationX;
                                //    double originalY = mZoneVM.LocationY;

                                //    double originalWidth = mZoneVM.Width;
                                //    double originalHeight = mZoneVM.Height;

                                //    mZoneVM.LocationX = (originalX) * (screenWidth / this.CurrentDepartment.ScreenWidth);
                                //    mZoneVM.LocationY = (originalY) * (screenHeight / this.CurrentDepartment.ScreenHeight);

                                //    mZoneVM.Width = originalWidth * (screenWidth / this.CurrentDepartment.ScreenWidth);
                                //    mZoneVM.Height = originalHeight * (screenHeight / this.CurrentDepartment.ScreenHeight);
                                //}

                                if (mZoneVM.IsLocked)
                                {
                                    mZoneVM.LockUnlockText = "Unlock";
                                    mZoneVM.LockUnlockPath = "Red";
                                }
                                else
                                {
                                    mZoneVM.LockUnlockText = "Lock";
                                    mZoneVM.LockUnlockPath = "Green";
                                }

                                SubscribeToMicroZoneEvents(mZoneVM);
                                this.MicroZones.Add(mZoneVM);
                            }
                        }

                        UpdateData();
                        this.SelectedDepartment.IsLastLoaded = true;
                        this._dataManager.UpdateDepartment(this.SelectedDepartment);
                    }
                    else
                    {
                        MessageBox.Show($"Unable to load file: {this.CurrentDepartment.FilePath}", "Error", MessageBoxButton.OK);
                    }
                }
            }
        }

        public void AddorEditMicroZone(MicroZoneViewModel microZone)
        {
            try
            {
                if (microZone == null)
                {
                    return;
                }

                var existingMicroZone = this.MicroZones.FirstOrDefault(microZoneItem => microZoneItem.MicroZoneId == microZone.MicroZoneId);

                //This microzone exist
                if (existingMicroZone != null)
                {
                    microZone.LocationX = existingMicroZone.LocationX;
                    microZone.LocationY = existingMicroZone.LocationY;
                    microZone.ScaleX = existingMicroZone.ScaleX;
                    microZone.ScaleY = existingMicroZone.ScaleY;
                    //microZone.IsAlarmEnabled = existingMicroZone.IsAlarmEnabled;
                    //microZone.ZoneType = existingMicroZone.ZoneType;
                    microZone.IsLocked = existingMicroZone.IsLocked;

                    this.MicroZones.Remove(existingMicroZone);
                }

                //Add microzone to current department or edited microzone changed department so don't assign to current
                if (microZone.DepartmentId == this.CurrentDepartment.DepartmentId)
                {
                    SubscribeToMicroZoneEvents(microZone);
                    this.MicroZones.Add(microZone);
                }
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.AddMicroZone Failure");
            }
        }

        public void ClearSelectedAssets()
        {
            try
            {
                foreach (MicroZoneViewModel microZone in this.MicroZones)
                {
                    foreach (AssetViewModel asset in microZone.Assets)
                    {
                        asset.IsSelected = false;
                    }
                }

                OnPropertyChanged(() => MicroZones);
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.ClearSelectedAssets Failure");
            }
        }

        public void ShowSelectedAsset(AssetViewModel selectedAsset)
        {
            try
            {
                if (selectedAsset == null)
                {
                    return;
                }

                foreach (MicroZoneViewModel microZone in this.MicroZones)
                {
                    foreach (AssetViewModel asset in microZone.Assets)
                    {
                        if (selectedAsset.AssetId == asset.AssetId)
                        {
                            asset.IsSelected = true;
                        }
                        else
                        {
                            asset.IsSelected = false;
                        }
                    }
                }

                OnPropertyChanged(() => MicroZones);
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.ShowSelectedAsset Failure");
            }
        }

        public void DeleteDepartment(DepartmentViewModel department)
        {
            //The current department was deleted, 
            if (this.CurrentDepartment.Name == department.Name)
            {
                this.CurrentDepartment = null;
                this.LocationImage = String.Empty;
            }
        }

        private void SyncSystem(object action)
        {
            UpdateMicroZoneAssets();
            this.LastSyncedTime = FormatSyncDate(DateTime.Now);
        }

        private void SetSyncInterval(object action)
        {
            this._syncTimeUserSet = this.SyncTime;
        }

        public void UpdateMicroZoneAssets()
        {
            try
            {
                List<AssetViewModel> assets = this._dataManager.GetAssets();

                // Assets exist, assign to microzones
                ClearAssetsFromMicroZone();

                foreach (AssetViewModel assetViewModel in assets)
                {
                    if (DateTime.Now <= assetViewModel.LastUpdatedOn.AddMinutes(5))
                    {
                        assetViewModel.AssetDeleted += DeleteAsset;

                        foreach (MicroZoneViewModel microZone in this.MicroZones)
                        {
                            string microZoneId = assetViewModel.MicroZonePrevious;

                            bool isAssetInThisMicroZone = microZoneId == microZone.MicroZoneId;

                            // Asset is reporting as being in this microzone
                            if (isAssetInThisMicroZone)
                                microZone.Assets.Add(assetViewModel);

                            microZone.UpdateMicroZoneProperties();
                        }
                    }
                }
                OnPropertyChanged(() => MicroZones);
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.UpdateMicroZoneAssets Failure");
            }
        }

        private void ClearAssetsFromMicroZone()
        {
            foreach (MicroZoneViewModel microZone in this.MicroZones)
            {
                microZone.Assets.Clear();
            }
        }

        public async void UpdateEditedMicroZone(MicroZoneViewModel microzone)
        {
            try
            {
                string response = await this._dataManager.AddOrEditMicroZone(microzone);

                List<MicroZoneResult> microZones = this._dataManager.GetMicroZones();
                List<MicroZoneViewModel> microZoneVMs = new List<MicroZoneViewModel>();

                foreach (MicroZoneResult microZone in microZones)
                {
                    MicroZoneViewModel mZoneVM = new MicroZoneViewModel(null, microzone.LocationX, microzone.LocationY, microzone.Width, microzone.Height, false)
                    {
                        DepartmentId = microZone.DepartmentId,
                        MicroZoneId = microZone.MicroZoneId.ToString(),
                        Name = microZone.MicroZoneName,
                        LocationX = microZone.MicroZoneX,
                        LocationY = microZone.MicroZoneY,
                        Height = microZone.MicroZoneHeight,
                        Width = microZone.MicroZoneWidth,
                        IsLocked = microZone.IsLocked,
                        TagAssociationNumber = microZone.TagAssociationNumber,
                        RawId = microzone.RawId
                    };

                    microZoneVMs.Add(mZoneVM);
                }

                foreach (MicroZoneViewModel mZoneVM in microZoneVMs)
                {
                    SubscribeToMicroZoneEvents(mZoneVM);
                }

                this.MicroZones = new ObservableCollection<MicroZoneViewModel>(microZoneVMs);
                OnPropertyChanged(() => MicroZones);
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.UpdateEditedMicroZone Failure");
            }
        }

        public void DeleteMicroZone(MicroZoneViewModel microZone)
        {
            try
            {
                if (microZone == null)
                {
                    return;
                }

                var existingMicroZone = this.MicroZones.FirstOrDefault(microZoneItem => microZoneItem.MicroZoneId == microZone.MicroZoneId);

                //This microzone exist
                if (existingMicroZone != null)
                {
                    this.MicroZones.Remove(existingMicroZone);
                    OnPropertyChanged(() => MicroZones);
                }
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.DeleteMicroZone Failure");
            }
        }

        private void LoadDepartmentImage()
        {
            try
            {
                RadWindow.Prompt("Department Name: ", this.OnLoadDepartmentClosed);
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.LoadDepartmentImage Failure");
            }
        }

        private void OnLoadDepartmentClosed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                RadOpenFileDialog dlg = new RadOpenFileDialog();
                dlg.InitialDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Departments");
                dlg.ExpandToCurrentDirectory = false;
                this._isRunning = true;
                if (dlg.ShowDialog() == true)
                {
                    this.LocationImage = dlg.FileName;
                    Bitmap bmp = new Bitmap(this.LocationImage);

                    DepartmentViewModel vm = new DepartmentViewModel()
                    {
                        FilePath = Path.GetFileName(dlg.FileName),
                        Name = e.PromptResult,
                        IsLastLoaded = true,
                        ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth,
                        ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight
                    };

                    this.Departments.Add(vm);
                    this.SelectedDepartment = vm;
                    OnPropertyChanged("SelectedDepartment");
                    OnPropertyChanged("Departments");
                    AddDepartment(vm);
                    LoadDepartment(null);
                }
            }
        }

        private async void MicroZone_MicroZoneLocked(object sender, GenericEventArgs<MicroZoneViewModel> e)
        {
            try
            {
                MicroZoneViewModel microZone = (MicroZoneViewModel)sender;
                string response = await this._dataManager.AddOrEditMicroZone(microZone);
            }
            catch (Exception exception)
            {
                //Logger.LogException(exception, "MicroZoneAssetLayoutViewModel.MicroZone_MicroZoneLocked Failure");
                // a new comment
            }
        }

        private void DeleteAsset(object sender, GenericEventArgs<AssetViewModel> asset)
        {
            try
            {
                List<Asset> assetsToDelete = new List<Asset>();

                Asset assetToDelete = new Asset
                {
                    AssetId = Int32.Parse(asset.Value.AssetId),
                    Name = asset.Value.Name,
                };

                assetsToDelete.Add(assetToDelete);

                this._dataManager.DeleteAsset(assetsToDelete);
                UpdateMicroZoneAssets();
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.DeleteAsset Failure");
            }
        }

        private void SubscribeToMicroZoneEvents(MicroZoneViewModel microZone)
        {
            try
            {
                microZone.LayoutUpdated += microZone_LayoutUpdated;
                microZone.MicroZoneLocked += MicroZone_MicroZoneLocked;
                microZone.MicroZoneDeleted += MicroZone_MicroZoneDeleted;
            }
            catch (Exception e)
            {
                //Logger.LogException(e, "MicroZoneAssetLayoutViewModel.SubscribeToMicroZoneEvents Failure");
            }
        }

        private void MicroZone_MicroZoneDeleted(object sender, GenericEventArgs<MicroZoneViewModel> e)
        {
            this.MicroZones.Remove(e.Value);
            this._dataManager.DeleteMicroZone(e.Value);
            OnPropertyChanged("MicroZones");
        }

        private string FormatSyncDate(DateTime date)
        {
            return String.Format("{0:t}", date);
        }

        public string LocationImage
        {
            get
            {
                return this._locationImage;
            }
            set
            {
                this._locationImage = value;
                OnPropertyChanged(() => LocationImage);
            }
        }

        public BitmapSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                _imageSource = value;
                OnPropertyChanged(() => ImageSource);
            }
        }

        public int ImageHeight
        {
            get
            {
                return this._imageHeight;
            }
            set
            {
                this._imageHeight = value;
                OnPropertyChanged(() => ImageHeight);
            }
        }

        public int ImageWidth
        {
            get
            {
                return this._imageWidth;
            }
            set
            {
                this._imageWidth = value;
                OnPropertyChanged(() => ImageWidth);
            }
        }

        public string LastSyncedTime
        {
            get
            {
                return this._lastSyncedTime;
            }

            set
            {
                this._lastSyncedTime = value;
                OnPropertyChanged(() => LastSyncedTime);
            }
        }

        public int SyncTime
        {
            get
            {
                return this._syncTime;
            }

            set
            {
                this._syncTime = value;
                OnPropertyChanged(() => SyncTime);
            }
        }

        public DepartmentViewModel CurrentDepartment
        {
            get
            {
                return _currentDepartment;
            }

            set
            {
                _currentDepartment = value;
            }
        }

        protected virtual void OnSystemSynced()
        {
            if (this.SystemSynced != null)
            {
                this.SystemSynced(this, EventArgs.Empty);
            }
        }
    }
}
