using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Controls;
using Visualizer.Model;
using Visualizer.Settings;
using Visualizer.Threading;

namespace Visualizer.ViewModel
{
    public class ApplicationSettingsPaneViewModel : ViewModelBase
    {
        private const string ApplicationFolder = "Visualizer";
        private const string SettingsFileName = "ApplicationSettings.json";

        public ObservableCollection<string> AvailableTagTypes { get; set; }
        public ObservableCollection<string> AvailableTagCategories { get; set; }
        public MainScreenViewModel MainScreenVM { get; set; }

        public DelegateCommand SaveSettingsCommand { get; set; }
        public DelegateCommand AddTypeCommand { get; set; }
        public DelegateCommand AddCategoryCommand { get; set; }
        public DelegateCommand DeleteTypeCommand { get; set; }
        public DelegateCommand DeleteCategoryCommand { get; set; }

        private DataManager _dataManager;
        private DelegateMarshaler _delegateMarshaler;
        private string _newCategory;
        private string _newType;
        private string _selectedTagType;
        private string _selectedTagCategory;

        public ApplicationSettingsPaneViewModel()
        {
            this._dataManager = DataManager.Instance;

            this.SaveSettingsCommand = new DelegateCommand(SaveSettings);
            this.AddTypeCommand = new DelegateCommand(AddType);
            this.AddCategoryCommand = new DelegateCommand(AddCategory);
            this.DeleteTypeCommand = new DelegateCommand(DeleteType);
            this.DeleteCategoryCommand = new DelegateCommand(DeleteCategory);

            this.AvailableTagTypes = new ObservableCollection<string>();
            this.AvailableTagCategories = new ObservableCollection<string>();

            this._delegateMarshaler = DelegateMarshaler.Create();
            LoadApplicationSettings();
        }

        private void DeleteCategory(object action)
        {
            var tagCategory = this.SelectedTagCategory;

            if (String.IsNullOrEmpty(tagCategory) == true)
            {
                return;
            }

            this._dataManager.DeleteTagCategory(tagCategory);
            LoadExternalApplicationSettings();
        }

        private void DeleteType(object action)
        {
            var tagType = this.SelectedTagType;

            if (String.IsNullOrEmpty(tagType) == true)
            {
                return;
            }

            this._dataManager.DeleteTagType(tagType);
            LoadExternalApplicationSettings();
        }

        private void LoadApplicationSettings()
        {
            LoadExternalApplicationSettings();
        }

        private async void LoadExternalApplicationSettings()
        {
            var tagTypes = await this._dataManager.GetTagTypes();
            this.AvailableTagTypes = new ObservableCollection<string>(tagTypes.Select(tagType => tagType.Type).ToArray());

            var tagCategories = await this._dataManager.GetTagCategories();
            this.AvailableTagCategories = new ObservableCollection<string>(tagCategories.Select(tagCategory => tagCategory.Category).ToArray());

            OnPropertyChanged(nameof(AvailableTagTypes));
            OnPropertyChanged(nameof(AvailableTagCategories));
        }

        private void SaveSettings(object action)
        {
            if (Directory.Exists(this.SettingsFolderPath) == false)
            {
                Directory.CreateDirectory(this.SettingsFolderPath);
            }

            ApplicationSettings.SyncIntervalSeconds = this.SyncInterval;

            var filePath = Path.Combine(this.SettingsFolderPath, SettingsFileName);
            WriteApplicationSettings(filePath);
        }

        private void WriteApplicationSettings(string settingsFile)
        {
            AppSettingsHolder settings = new AppSettingsHolder();

            settings.IsDarkMode = ApplicationSettings.IsDarkMode;
            settings.ShowMapping = ApplicationSettings.ShowMapping;
            settings.ShowReporting = ApplicationSettings.ShowReporting;
            settings.ShowInventory = ApplicationSettings.ShowInventory;
            settings.ShowToolTips = ApplicationSettings.ShowToolTips;
            settings.SyncIntervalSeconds = ApplicationSettings.SyncIntervalSeconds;
            settings.ServerUrl = ApplicationSettings.ServerUrl;
            settings.ShowAsset = ApplicationSettings.ShowAsset;

            var settingsJSON = JsonConvert.SerializeObject(settings);
            File.WriteAllText(settingsFile, settingsJSON);

            MessageBox.Show("You must restart the application for the new settings to take effect");
        }

        private async void AddType(object action)
        {
            if (String.IsNullOrEmpty(this.NewType))
            {
                MessageBox.Show("Please provide a type to add to the system.");
                return;
            }

            await this._dataManager.AddTagType(new TagType() { Type = this.NewType });
            this.NewType = string.Empty;

            LoadExternalApplicationSettings();
        }

        private async void AddCategory(object action)
        {
            if (string.IsNullOrEmpty(this.NewCategory))
            {
                MessageBox.Show("Please provide a category to add to the system.");
                return;
            }

            await this._dataManager.AddTagCategory(new TagCategory() { Category = this.NewCategory });
            this.NewCategory = string.Empty;

            LoadExternalApplicationSettings();
        }

        private string SettingsFolderPath
        {
            get
            {
                var settingsFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return Path.Combine(settingsFolder, ApplicationFolder);
            }
        }

        public string ServerUrl
        {
            get => ApplicationSettings.ServerUrl;

            set
            {
                ApplicationSettings.ServerUrl = value;
                OnPropertyChanged(nameof(ServerUrl));
            }
        }

        public int SyncInterval
        {
            get => ApplicationSettings.SyncIntervalSeconds;

            set
            {
                ApplicationSettings.SyncIntervalSeconds = value;
                OnPropertyChanged(nameof(SyncInterval));
            }
        }

        public bool IsDarkMode
        {
            get => ApplicationSettings.IsDarkMode;

            set
            {
                ApplicationSettings.IsDarkMode = value;

                if(value)
                {
                    CrystalPalette.LoadPreset(CrystalPalette.ColorVariation.Dark);
                    CrystalPalette.Palette.WindowButtonsAlignment = ButtonsAlignment.Right;
                    StyleManager.ApplicationTheme = new CrystalTheme();
                }
                else
                {
                    CrystalPalette.LoadPreset(CrystalPalette.ColorVariation.Light);
                    CrystalPalette.Palette.WindowButtonsAlignment = ButtonsAlignment.Right;
                    StyleManager.ApplicationTheme = new CrystalTheme();
                }

                OnPropertyChanged(nameof(IsDarkMode));
            }
        }

        public bool ShowToolTips
        {
            get => ApplicationSettings.ShowToolTips;

            set
            {
                ApplicationSettings.ShowToolTips = value;
                OnPropertyChanged(nameof(ShowToolTips));
            }
        }

        public bool ShowMapping
        {
            get => ApplicationSettings.ShowMapping;

            set
            {
                ApplicationSettings.ShowMapping = value;
                OnPropertyChanged(nameof(ShowMapping));
            }
        }

        public bool ShowReporting
        {
            get => ApplicationSettings.ShowReporting;

            set
            {
                ApplicationSettings.ShowReporting = value;
                OnPropertyChanged(nameof(ShowReporting));
            }
        }

        public bool ShowAsset
        {
            get => ApplicationSettings.ShowAsset;

            set
            {
                ApplicationSettings.ShowAsset = value;
                OnPropertyChanged(nameof(ShowAsset));
            }
        }

        public bool ShowInventory
        {
            get => ApplicationSettings.ShowInventory;

            set
            {
                ApplicationSettings.ShowInventory = value;
                OnPropertyChanged(nameof(ShowInventory));
            }
        }

        public string NewType
        {
            get
            {
                return this._newType;
            }
            set
            {
                this._newType = value;
                OnPropertyChanged(() => NewType);
            }
        }

        public string NewCategory
        {
            get
            {
                return this._newCategory;
            }
            set
            {
                this._newCategory = value;
                OnPropertyChanged(() => NewCategory);
            }
        }

        public string SelectedTagType
        {
            get
            {
                return this._selectedTagType;
            }
            set
            {
                this._selectedTagType = value;
                OnPropertyChanged(() => SelectedTagType);
            }
        }
        public string SelectedTagCategory
        {
            get
            {
                return this._selectedTagCategory;
            }
            set
            {
                this._selectedTagCategory = value;
                OnPropertyChanged(() => SelectedTagCategory);
            }
        }
    }
}
