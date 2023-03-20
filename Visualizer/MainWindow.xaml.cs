using System;
using System.Collections.Generic;
using System.Linq;
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
using Telerik.Windows.Controls.Docking;
using Telerik.Windows.Controls.SplashScreen;
using Visualizer.Model;
using Visualizer.ViewModel;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RadWindow
    {
        private MainScreenViewModel _mainScreenVM;
        private DataManager  _manager;

        public MainWindow()
        {
            InitializeComponent();
            _mainScreenVM = new MainScreenViewModel();
            this.DataContext = _mainScreenVM;
            _mainScreenVM.SaveColumnLayoutEvent += _mainScreenVM_SaveColumnLayoutEvent;

            var dataContext = (SplashScreenDataContext)RadSplashScreenManager.SplashScreenDataContext;
            dataContext.ImagePath = "/Images/focusSplashScreen.png";
            dataContext.Content = "Loading Application...";
            dataContext.Footer = "Copyright ©2022, RTLS";

            RadSplashScreenManager.Show();

            _manager = DataManager.Instance;
        }

        private void _mainScreenVM_SaveColumnLayoutEvent(object sender, EventArgs e)
        {
            var pane = this.radDocking.Panes.Where(x => x.Header.ToString() == "Dashboard").FirstOrDefault();

            if (pane != null)
            {
                SystemReportingPaneViewModel vm = pane.DataContext as SystemReportingPaneViewModel;
                vm.SaveColumnLayouts();
            }

            pane = this.radDocking.Panes.Where(x => x.Header.ToString() == "Mapping").FirstOrDefault();

            if (pane != null)
            {
                MappingPaneViewModel vm = pane.DataContext as MappingPaneViewModel;
                vm.SaveColumnLayouts();
            }
        }

        public MainScreenViewModel MainScreen
        {
            get
            {
                return this.DataContext as MainScreenViewModel;
            }
        }

        private void RadNavigationViewItem_Click(object sender, RoutedEventArgs e)
        {
            string viewItemContent = (e.OriginalSource as RadNavigationViewItem).Content.ToString();
            string viewItemHeader = (e.OriginalSource as RadNavigationViewItem).Name.ToString();

            if (viewItemContent != null && viewItemContent == "Daily")
            {
                var pane = this.radDocking.Panes.Where(x => x.Header.ToString() == "Daily").FirstOrDefault();

                if (pane != null)
                {
                    pane.IsHidden = !pane.IsHidden;
                }
                else
                {
                    MainScreen.Panes.Add(new PaneViewModel(typeof(DailyPaneViewModel)) { Header = "Daily", InitialPosition = DockState.DockedLeft, IsHidden = false });
                }
            }
            else if (viewItemContent != null && viewItemContent == "Inventory Management")
            {
                var pane = this.radDocking.Panes.Where(x => x.Header.ToString() == "Inventory Management").FirstOrDefault();

                if (pane != null)
                {
                    pane.IsHidden = !pane.IsHidden;
                }
                else
                {
                    MainScreen.Panes.Add(new PaneViewModel(typeof(InventoryManagementPaneViewModel)) { Header = "Inventory Management", InitialPosition = DockState.DockedLeft, IsHidden = false });
                }
            }
            else if (viewItemContent != null && viewItemContent == "Asset Management")
            {
                var pane = this.radDocking.Panes.Where(x => x.Header.ToString() == "Asset Management").FirstOrDefault();

                if (pane != null)
                {
                    pane.IsHidden = !pane.IsHidden;
                }
                else
                {
                    MainScreen.Panes.Add(new PaneViewModel(typeof(AssetManagementPaneViewModel)) { Header = "Asset Management", InitialPosition = DockState.DockedLeft, IsHidden = false });
                }
            }
            else if (viewItemContent != null && viewItemContent == "Mapping")
            {
                var pane = this.radDocking.Panes.Where(x => x.Header.ToString() == "Mapping").FirstOrDefault();

                if (pane != null)
                {
                    pane.IsHidden = !pane.IsHidden;
                }
                else
                {
                    MainScreen.Panes.Add(new PaneViewModel(typeof(MappingPaneViewModel)) { Header = "Mapping", InitialPosition = DockState.DockedLeft, IsHidden = false });
                }
            }
            else if (viewItemContent != null && viewItemContent == "Dashboard")
            {
                var pane = this.radDocking.Panes.Where(x => x.Header.ToString() == "Dashboard").FirstOrDefault();

                if (pane != null)
                {
                    pane.IsHidden = !pane.IsHidden;
                }
                else
                {
                    MainScreen.Panes.Add(new PaneViewModel(typeof(SystemReportingPaneViewModel)) { Header = "Dashboard", InitialPosition = DockState.DockedLeft, IsHidden = false });
                }
            }
            else if (viewItemContent != null && viewItemContent == "Application Settings")
            {
                var pane = this.radDocking.Panes.Where(x => x.Header.ToString() == "Application Settings").FirstOrDefault();

                if (pane != null)
                {
                    pane.IsHidden = !pane.IsHidden;
                }
                else
                {
                    MainScreen.Panes.Add(new PaneViewModel(typeof(ApplicationSettingsPaneViewModel)) { Header = "Application Settings", InitialPosition = DockState.DockedLeft, IsHidden = false });
                }
            }
        }

        private void radDocking_ElementLoaded(object sender, LayoutSerializationEventArgs e)
        {
            var pane = e.AffectedElement as RadPane;

            if (pane != null)
            {
                if (pane.Header.ToString() == "Daily")
                {
                    DailyPaneViewModel dailyPaneVM = new DailyPaneViewModel();
                    pane.DataContext = dailyPaneVM;
                    pane.Content = dailyPaneVM;
                }

                if (pane.Header.ToString() == "Inventory Management")
                {
                    InventoryManagementPaneViewModel inventoryPaneVM = new InventoryManagementPaneViewModel();
                    pane.DataContext = inventoryPaneVM;
                    pane.Content = inventoryPaneVM;
                }

                if (pane.Header.ToString() == "Asset Management")
                {
                    AssetManagementPaneViewModel assetPaneVM = new AssetManagementPaneViewModel();
                    pane.DataContext = assetPaneVM;
                    pane.Content = assetPaneVM;
                }

                if (pane.Header.ToString() == "Mapping")
                {
                    MappingPaneViewModel mappingPaneVM = new MappingPaneViewModel();
                    pane.DataContext = mappingPaneVM;
                    pane.Content = mappingPaneVM;
                }

                if (pane.Header.ToString() == "Dashboard")
                {
                    SystemReportingPaneViewModel systemPaneVM = new SystemReportingPaneViewModel();
                    pane.DataContext = systemPaneVM;
                    pane.Content = systemPaneVM;
                }

                if (pane.Header.ToString() == "Application Settings")
                {
                    ApplicationSettingsPaneViewModel settingsPaneVM = new ApplicationSettingsPaneViewModel();
                    settingsPaneVM.MainScreenVM = this.MainScreen;
                    pane.DataContext = settingsPaneVM;
                    pane.Content = settingsPaneVM;
                }
            }
        }
    }
}
