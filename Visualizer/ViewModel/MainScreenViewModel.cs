using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
using Visualizer.Settings;
using ICommand = System.Windows.Input.ICommand;

namespace Visualizer.ViewModel
{
    public class MainScreenViewModel : ViewModelBase
    {
        public event EventHandler SaveColumnLayoutEvent;
        public ICommand ShowDailyPaneCommand { get; set; }
        public ICommand ShowAssetManagementPaneCommand { get; set; }
        public ICommand ShowInventoryManagementPaneCommand { get; set; }
        public ICommand ShowMappingPaneCommand { get; set; }
        public ICommand ShowSystemReportingPaneCommand { get; set; }
        public ICommand ShowApplicationSettingsPaneCommand { get; set; }

        public ObservableCollection<PaneViewModel> Panes { get; set; }

        public MainScreenViewModel()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomainExceptionHandler);

            this.ShowDailyPaneCommand = new DelegateCommand(ShowDailyPane);
            this.ShowInventoryManagementPaneCommand = new DelegateCommand(ShowInventoryManagementPane);
            this.ShowAssetManagementPaneCommand = new DelegateCommand(ShowAssetManagementPane);
            this.ShowMappingPaneCommand = new DelegateCommand(ShowMappingPane);
            this.ShowSystemReportingPaneCommand = new DelegateCommand(ShowSystemReportingPane);
            this.ShowApplicationSettingsPaneCommand = new DelegateCommand(ShowApplicationSettingsPane);
            this.Panes = new ObservableCollection<PaneViewModel>();
        }

        public void SaveColumnLayouts()
        {
            SaveColumnLayoutEvent.Invoke(this, EventArgs.Empty);
        }

        public void ShowDailyPane(object action)
        {
            var res = this.Panes.Where(x => x.Header == "Daily").FirstOrDefault();

            if (res == null)
                this.Panes.Add(new PaneViewModel(typeof(DailyPaneViewModel)) { Header = "Daily", InitialPosition = DockState.DockedLeft, IsHidden = false });
            else
                this.Panes.Where(x => x.Header == "Daily").FirstOrDefault().IsHidden = !this.Panes.Where(x => x.Header == "Daily").FirstOrDefault().IsHidden;

            OnPropertyChanged("Panes");
        }

        public void ShowInventoryManagementPane(object action)
        {
            var res = this.Panes.Where(x => x.Header == "Inventory Management").FirstOrDefault();

            if (res == null)
                this.Panes.Add(new PaneViewModel(typeof(InventoryManagementPaneViewModel)) { Header = "Inventory Management", InitialPosition = DockState.DockedLeft, IsHidden = false });
            else
                this.Panes.Where(x => x.Header == "Inventory Management").FirstOrDefault().IsHidden = !this.Panes.Where(x => x.Header == "Inventory Management").FirstOrDefault().IsHidden;

            OnPropertyChanged("Panes");
        }

        public void ShowAssetManagementPane(object action)
        {
            var res = this.Panes.Where(x => x.Header == "Asset Management").FirstOrDefault();

            if (res == null)
                this.Panes.Add(new PaneViewModel(typeof(AssetManagementPaneViewModel)) { Header = "Asset Management", InitialPosition = DockState.DockedLeft, IsHidden = false });
            else
                this.Panes.Where(x => x.Header == "Asset Management").FirstOrDefault().IsHidden = !this.Panes.Where(x => x.Header == "Asset Management").FirstOrDefault().IsHidden;

            OnPropertyChanged("Panes");
        }

        public void ShowMappingPane(object action)
        {
            var res = this.Panes.Where(x => x.Header == "Mapping").FirstOrDefault();

            if (res == null)
                this.Panes.Add(new PaneViewModel(typeof(MappingPaneViewModel)) { Header = "Mapping", InitialPosition = DockState.DockedLeft, IsHidden = false });
            else
                this.Panes.Where(x => x.Header == "Mapping").FirstOrDefault().IsHidden = !this.Panes.Where(x => x.Header == "Mapping").FirstOrDefault().IsHidden;

            OnPropertyChanged("Panes");
        }

        public void ShowSystemReportingPane(object action)
        {
            var res = this.Panes.Where(x => x.Header == "Dashboard").FirstOrDefault();

            if (res == null)
                this.Panes.Add(new PaneViewModel(typeof(SystemReportingPaneViewModel)) { Header = "Dashboard", InitialPosition = DockState.DockedLeft, IsHidden = false });
            else
                this.Panes.Where(x => x.Header == "Dashboard").FirstOrDefault().IsHidden = !this.Panes.Where(x => x.Header == "Dashboard").FirstOrDefault().IsHidden;

            OnPropertyChanged("Panes");
        }

        public void ShowApplicationSettingsPane(object action)
        {
            var res = this.Panes.Where(x => x.Header == "Application Settings").FirstOrDefault();

            if (res == null)
                this.Panes.Add(new PaneViewModel(typeof(ApplicationSettingsPaneViewModel)) { Header = "Application Settings", InitialPosition = DockState.DockedLeft, IsHidden = false });
            else
                this.Panes.Where(x => x.Header == "Application Settings").FirstOrDefault().IsHidden = !this.Panes.Where(x => x.Header == "Application Settings").FirstOrDefault().IsHidden;

            OnPropertyChanged("Panes");
        }

        public bool IsMappingEnabled
        {
            get { return ApplicationSettings.ShowMapping; }
        }

        public bool IsReportingEnabled
        {
            get { return ApplicationSettings.ShowReporting; }
        }

        public bool IsInventoryEnabled
        {
            get { return ApplicationSettings.ShowInventory; }
        }

        public bool IsAssetEnabled
        {
            get { return ApplicationSettings.ShowAsset; }
        }

        private void AppDomainExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            File.WriteAllText("UnhandlerException.txt", e.ToString());
        }

        public void UpdateUI()
        {
            OnPropertyChanged("IsReportingEnabled");
            OnPropertyChanged("IsMappingEnabled");
            OnPropertyChanged("IsAssetEnabled");
            OnPropertyChanged("IsInventoryEnabled");
        }
    }
}
