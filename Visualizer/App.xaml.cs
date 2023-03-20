using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Visualizer.Settings;
using Visualizer.ViewModel;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string ApplicationFolder = "Visualizer";
        private const string SettingsFileName = "ApplicationSettings.json";
        private MainWindow _mainWindow;
        private MainScreenViewModel _mainScreenVM;

        protected override void OnStartup(StartupEventArgs e)
        {
            LoadLocalApplicationSettings();

            if (ApplicationSettings.IsDarkMode)
            {
                CrystalPalette.LoadPreset(CrystalPalette.ColorVariation.Dark);
            }
            else
            {
                CrystalPalette.LoadPreset(CrystalPalette.ColorVariation.Light);
            }
            CrystalPalette.Palette.WindowButtonsAlignment = ButtonsAlignment.Right;
            StyleManager.ApplicationTheme = new CrystalTheme();

            _mainWindow = new MainWindow();
            _mainScreenVM = _mainWindow.DataContext as MainScreenViewModel;
            _mainWindow.Show();

            base.OnStartup(e);
            base.MainWindow.WindowState = WindowState.Maximized; 
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            File.WriteAllText("ApplicationError.txt", e.ToString());

            // Prevent default unhandled exception processing
            e.Handled = true;
        }

        private string SettingsFolderPath
        {
            get
            {
                var settingsFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return Path.Combine(settingsFolder, ApplicationFolder);
            }
        }

        private void LoadLocalApplicationSettings()
        {
            var filePath = Path.Combine(this.SettingsFolderPath, SettingsFileName);

            if (File.Exists(filePath))
            {
                var fileContents = File.ReadAllText(filePath);
                AppSettingsHolder settings = JsonConvert.DeserializeObject<AppSettingsHolder>(fileContents);

                ApplicationSettings.IsDarkMode = settings.IsDarkMode;
                ApplicationSettings.ShowMapping = settings.ShowMapping;
                ApplicationSettings.ShowReporting = settings.ShowReporting;
                ApplicationSettings.ShowInventory = settings.ShowInventory;
                ApplicationSettings.ShowToolTips = settings.ShowToolTips;
                ApplicationSettings.SyncIntervalSeconds = settings.SyncIntervalSeconds;
                ApplicationSettings.ServerUrl = settings.ServerUrl;
                ApplicationSettings.ShowAsset = settings.ShowAsset;
            }
            else
            {
                //if it doesn't exist, set some defaults
                ApplicationSettings.IsDarkMode = false;
                ApplicationSettings.ShowMapping = false;
                ApplicationSettings.ShowReporting = true;
                ApplicationSettings.ShowInventory = false;
                ApplicationSettings.ShowAsset = false;
                ApplicationSettings.ShowToolTips = true;
                ApplicationSettings.SyncIntervalSeconds = 30;
                ApplicationSettings.ServerUrl = "lrniservice-test.azurewebsites.net";
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _mainScreenVM.SaveColumnLayouts();
        }
    }
}
