using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visualizer.ViewModel;

namespace Visualizer.View
{
    /// <summary>
    /// Interaction logic for InventoryManagementPaneView.xaml
    /// </summary>
    public partial class InventoryManagementPaneView : UserControl
    {
        private double _locationX;
        private double _locationY;
        private double originalCenterX;
        private double originalCenterY;
        private double _deltaX;
        private double _deltaY;
        private double _originalAngle;
        private Point _originalPoint;
        private double _originalRotation;
        private bool _isDragging;
        private Border border;
        private InventoryManagementPaneViewModel _inventoryManagementVM;

        private UIElement _element;

        private MicroZoneViewModel _selectedMicroZoneViewModel;

        public InventoryManagementPaneView()
        {
            InitializeComponent();

            this.PreviewMouseLeftButtonDown += MicroZoneUserControl_MouseLeftButtonDown;
            this.BackgroundCanvas.MouseLeftButtonDown += MicroZoneUserControlCanvas_MouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += MicroZoneUserControl_MouseLeftButtonUp;
            this.MouseMove += MicroZoneUserControl_MouseMove;

            //this.MapView.Focus();
            //this.MapView.AnimationLevel = AnimationLevel.Full;
            //this.MapView.Mode = new AerialMode(true);
            //this.MapView.ZoomLevel = 18.0;
            //this.MapView.Center = new Location(32.2177, -82.4135);
        }

        void MicroZoneUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition((UIElement)sender);

                this._deltaX = this._locationX - point.X;
                this._deltaY = this._locationY - point.Y;

                this._locationX = point.X;
                this._locationY = point.Y;

                MicroZoneMoveCallback();
            }
        }

        void MicroZoneUserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this._selectedMicroZoneViewModel != null)
            {
                this._selectedMicroZoneViewModel.UpdateMicroZoneLayout();
            }

            this._selectedMicroZoneViewModel = null;
        }

        void MicroZoneUserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            this._locationX = e.GetPosition(sender as Control).X;
            this._locationY = e.GetPosition(sender as Control).Y;

            Point point = e.GetPosition((UIElement)sender);

            _element = (UIElement)sender;

            VisualTreeHelper.HitTest(
                (UIElement)sender,
                null,
                new HitTestResultCallback(MicroZoneHitCallback),
                new PointHitTestParameters(point));
        }

        void MicroZoneUserControlCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            this._locationX = e.GetPosition(sender as Control).X;
            this._locationY = e.GetPosition(sender as Control).Y;

            Point point = e.GetPosition((UIElement)sender);

            _element = (UIElement)sender;

            VisualTreeHelper.HitTest(
                (UIElement)sender,
                null,
                new HitTestResultCallback(MicroZoneHitCallback),
                new PointHitTestParameters(point));
        }

        private HitTestResultBehavior MicroZoneHitCallback(HitTestResult result)
        {
            if (result.VisualHit is Border)
            {
                border = result.VisualHit as Border;
                this._selectedMicroZoneViewModel = border.DataContext as MicroZoneViewModel;

                if(this._selectedMicroZoneViewModel != null) 
                {
                    _inventoryManagementVM = this.DataContext as InventoryManagementPaneViewModel;

                    foreach (MicroZoneViewModel mz in _inventoryManagementVM.MicroZones)
                    {
                        if (mz == _selectedMicroZoneViewModel)
                            mz.IsSelected = true;
                        else
                            mz.IsSelected = false;
                    }

                    this._originalPoint = Mouse.GetPosition(border);

                    originalCenterX = this._selectedMicroZoneViewModel.OriginalCenterX - (this._selectedMicroZoneViewModel.Width / 2);
                    originalCenterY = this._selectedMicroZoneViewModel.OriginalCenterY - (this._selectedMicroZoneViewModel.Height / 2);

                    double x = this._originalPoint.X - this._selectedMicroZoneViewModel.CenterX;
                    double y = this._originalPoint.Y - this._selectedMicroZoneViewModel.CenterY;
                    double angle = Math.Atan2(x, y);
                    this._originalAngle = angle * (180 / Math.PI);

                    this._originalRotation = this._selectedMicroZoneViewModel.Rotation;

                    this._isDragging = true;
                    return HitTestResultBehavior.Continue;
                }
            }

            return HitTestResultBehavior.Stop;
        }

        private void MicroZoneMoveCallback()
        {
            if (this._selectedMicroZoneViewModel != null)
            {
                if (this._selectedMicroZoneViewModel.IsLocked == false)
                {
                    //if (this._selectedMicroZoneViewModel.Rotation != 0)
                    //{
                    //    Point newPosition = Mouse.GetPosition(border);

                    //    double x = newPosition.X + this._selectedMicroZoneViewModel.CenterX;
                    //    double y = newPosition.Y + this._selectedMicroZoneViewModel.CenterY;

                    //    double angle = Math.Atan2(x, y);
                    //    double angleDegrees = (angle * (180 / Math.PI)) * 1.5;

                    //    if (angleDegrees == 0)
                    //    {
                    //        return;
                    //    }

                    //    double rotateDegrees = this._originalAngle - angleDegrees;

                    //    //keep element on center point when rotated
                    //    if (IsZero(rotateDegrees) == false)
                    //    {
                    //        this._selectedMicroZoneViewModel.CenterX = originalCenterX;
                    //        this._selectedMicroZoneViewModel.CenterY = originalCenterY;
                    //    }

                    //    this._selectedMicroZoneViewModel.Rotation = -1 * (this._originalRotation + rotateDegrees);

                    //    //keep element on center point when rotated
                    //    if (IsZero(rotateDegrees) == false)
                    //    {
                    //        this._selectedMicroZoneViewModel.CenterX = originalCenterX;
                    //        this._selectedMicroZoneViewModel.CenterY = originalCenterY;
                    //    }
                    //}
                    //else
                    //{
                        if (this._selectedMicroZoneViewModel.LocationX - this._deltaX < 0)
                            return;
                        if (this._selectedMicroZoneViewModel.LocationY - this._deltaY < 0)
                            return;

                    if (this._deltaX > this._selectedMicroZoneViewModel.LocationX)
                    {
                        this._selectedMicroZoneViewModel.LocationX = this._selectedMicroZoneViewModel.LocationX + this._deltaX;

                        //if (this._selectedMicroZoneViewModel.LocationX > this.BackgroundCanvas.ActualWidth)
                        //    this._selectedMicroZoneViewModel.LocationX = this.BackgroundCanvas.ActualWidth - this._selectedMicroZoneViewModel.Width;
                    }

                    if (this._deltaX < this._selectedMicroZoneViewModel.LocationX)
                    {
                        this._selectedMicroZoneViewModel.LocationX = this._selectedMicroZoneViewModel.LocationX - this._deltaX;

                        //if (this._selectedMicroZoneViewModel.LocationX > this.BackgroundCanvas.ActualWidth - this._selectedMicroZoneViewModel.Width)
                        //    this._selectedMicroZoneViewModel.LocationX = this.BackgroundCanvas.ActualWidth - this._selectedMicroZoneViewModel.Width;
                    }

                    if (this._deltaY > this._selectedMicroZoneViewModel.LocationY)
                    {
                        this._selectedMicroZoneViewModel.LocationY = this._selectedMicroZoneViewModel.LocationY + this._deltaY;

                        //if (this._selectedMicroZoneViewModel.LocationY > this.BackgroundCanvas.ActualHeight)
                        //    this._selectedMicroZoneViewModel.LocationY = this.BackgroundCanvas.ActualHeight - this._selectedMicroZoneViewModel.Height;
                    }

                    if (this._deltaY < this._selectedMicroZoneViewModel.LocationY)
                    {
                        this._selectedMicroZoneViewModel.LocationY = this._selectedMicroZoneViewModel.LocationY - this._deltaY;

                        //if (this._selectedMicroZoneViewModel.LocationY > this.BackgroundCanvas.ActualHeight - this._selectedMicroZoneViewModel.Height)
                        //    this._selectedMicroZoneViewModel.LocationY = this.BackgroundCanvas.ActualHeight - this._selectedMicroZoneViewModel.Height;
                    }
                //}
            }

                return;
            }

            return;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.BackgroundCanvas.Width = this.InventoryGrid.ActualWidth;
            //this.BackgroundCanvas.Height = this.InventoryGrid.ActualHeight;
            _inventoryManagementVM = this.DataContext as InventoryManagementPaneViewModel;

            if(_inventoryManagementVM != null)
            {
                //// Traverse the visual tree to get the accumulated position of the parent controls
                //System.Windows.FrameworkElement parent = MZoneItemsControl.Parent as System.Windows.FrameworkElement;
                //double accumulatedX = 0;
                //double accumulatedY = 0;
                //while (parent != null)
                //{
                //    accumulatedX += parent.Margin.Left;
                //    accumulatedY += parent.Margin.Top;
                //    parent = parent.Parent as System.Windows.FrameworkElement;
                //}

                //_inventoryManagementVM.AccumulatedX = accumulatedX;
                //_inventoryManagementVM.AccumulatedY = accumulatedY;
            }
        }

        public static bool IsZero(double value)
        {
            return (Math.Abs(value) < 2.2204460492503131E-15);
        }
    }
}
