using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MicroZoneUserControl.xaml
    /// </summary>
    public partial class MicroZoneUserControl : UserControl
    {
        private Cursor _cursor;
        private bool _isRotateEnabled;
        private double _originalAngle;
        private Point _originalPoint;
        private double _originalRotation;
        private bool _isDragging;
        private double _originalX;
        private double _originalY;

        public MicroZoneUserControl()
        {
            InitializeComponent();

            this.MicroZoneButton.Visibility = Visibility.Hidden;

            this.MouseDown += MicroZoneUserControl_MouseDown;
            this.MouseMove += MicroZoneUserControl_MouseMove;
            //HoldGestureTrigger hold = new HoldGestureTrigger();
            //hold.HandlesTouches = true;
            //hold.Hold += hold_Hold;

            //Interaction.GetTriggers(this).Add(hold);
        }

        private void MicroZoneUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isRotateEnabled)
            {
                MicroZoneViewModel microZoneViewModel = this.DataContext as MicroZoneViewModel;
                Point newPosition = e.GetPosition((UIElement)this.Border);

                double x = newPosition.X - microZoneViewModel.CenterX;
                double y = newPosition.Y - microZoneViewModel.CenterY;

                double angle = Math.Atan2(x, y);
                double angleDegrees = angle * (180 / Math.PI);

                if (angleDegrees == 0)
                {
                    return;
                }

                double rotateDegrees = this._originalAngle - angleDegrees;

                microZoneViewModel.Rotation = this._originalRotation + rotateDegrees;
                microZoneViewModel.CenterX = _originalX;
                microZoneViewModel.CenterY = _originalY;
                Debug.WriteLine($"zoneCenterX={microZoneViewModel.CenterX}, zoneCenterY={microZoneViewModel.CenterY}");
            }
        }

        private void MicroZoneUserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MicroZoneViewModel microZoneViewModel = this.DataContext as MicroZoneViewModel;
            microZoneViewModel.IsSelected = true;
            this._originalPoint = e.GetPosition((UIElement)this.Border);

            double x = this._originalPoint.X - microZoneViewModel.CenterX;
            double y = this._originalPoint.Y - microZoneViewModel.CenterY;
            double angle = Math.Atan2(x, y);
            this._originalAngle = angle * (180 / Math.PI);

            this._originalRotation = microZoneViewModel.Rotation;
            this._originalX = microZoneViewModel.CenterX;
            this._originalY = microZoneViewModel.CenterY;
            Debug.WriteLine($"OriginalX={_originalX}, OriginalY={_originalY}");
        }

        void hold_Hold(object sender, EventArgs e)
        {
            this.ContextMenu.IsOpen = true;
        }

        private void OnResizeThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            this._cursor = Cursor;
            Cursor = Cursors.SizeNWSE;
        }

        private void OnResizeThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            Cursor = this._cursor;
            MicroZoneViewModel microZoneViewModel = this.DataContext as MicroZoneViewModel;

            microZoneViewModel.Width = this.MicroZone.Width;
            microZoneViewModel.Height = this.MicroZone.Height;
            microZoneViewModel.UpdateMicroZoneLayout();
        }

        private void OnResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            MicroZoneViewModel microZoneViewModel = this.DataContext as MicroZoneViewModel;

            if (microZoneViewModel.IsLocked)
                return;

                double yAdjust = this.MicroZone.Height + e.VerticalChange;
                double xAdjust = this.MicroZone.Width + e.HorizontalChange;

                //make sure not to resize to negative width or heigth            
                xAdjust = (this.MicroZone.ActualWidth + xAdjust) > this.MicroZone.MinWidth ? xAdjust : this.MicroZone.MinWidth;
                yAdjust = (this.MicroZone.ActualHeight + yAdjust) > this.MicroZone.MinHeight ? yAdjust : this.MicroZone.MinHeight;

                this.MicroZone.Width = xAdjust < 0 ? 0 : xAdjust;
                this.MicroZone.Height = yAdjust < 0 ? 0 : yAdjust;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MicroZoneButton.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            this.MicroZoneButton.Visibility = Visibility.Hidden;
        }
    }
}
