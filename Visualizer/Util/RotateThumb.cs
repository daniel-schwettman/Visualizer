using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Input;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class RotateThumb : Thumb
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ViewModelElement), typeof(RotateThumb));
        public static readonly DependencyProperty ViewContextProperty = DependencyProperty.Register("ViewContext", typeof(ViewContext), typeof(RotateThumb), new UIPropertyMetadata(null));
        TransformCommand _transformCommand = new TransformCommand();

        static RotateThumb()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RotateThumb), new FrameworkPropertyMetadata(typeof(RotateThumb)));
        }

        private double _originalAngle;
        private Point _originalPoint;
        private double _originalRotation;
        private ViewCanvas _viewCanvas;
        private bool _isDragging;

        public RotateThumb()
            : base()
        {
            this.PreviewMouseDown += new MouseButtonEventHandler(RotateThumb_PreviewMouseDown);
            this.PreviewMouseMove += new MouseEventHandler(RotateThumb_PreviewMouseMove);
            this.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(RotateThumb_PreviewMouseLeftButtonDown);
            this.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(RotateThumb_PreviewMouseLeftButtonUp);
        }

        void RotateThumb_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //undo/redo command execution
            if (_transformCommand != null)
            {
                _transformCommand.Execute(new List<ViewModelElement>() { this.ViewModel });
                _transformCommand = new TransformCommand();
                //unsubscribe from the view canvas event
                this._viewCanvas.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(RotateThumb_PreviewMouseLeftButtonUp);
            }

        }

        void RotateThumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //undo/redo command execution
            _transformCommand.Execute(new List<ViewModelElement>() { this.ViewModel });
        }

        void RotateThumb_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                this._isDragging = false;
            }

            if (this._isDragging == false)
            {
                if (this._viewCanvas != null)
                {
                    //detatch handler
                    this._viewCanvas.PreviewMouseMove -= RotateThumb_PreviewMouseMove;
                    this._viewCanvas = null;
                }

                return;
            }

            Point newPosition = e.GetPosition(this._viewCanvas);

            double x = newPosition.X - this.ViewModel.CenterX;
            double y = newPosition.Y - this.ViewModel.CenterY;

            double angle = Math.Atan2(x, y);
            double angleDegrees = angle * (180 / Math.PI);

            if (angleDegrees == 0)
            {
                return;
            }

            double rotateDegrees = this._originalAngle - angleDegrees;

            //this.ViewModel.Rotation = this._originalRotation + rotateDegrees;

            if (this.ViewContext != null)
            {
                //this.ViewContext.RaiseSelectionBoundsChanged();
            }

            e.Handled = true;
        }

        void RotateThumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this._viewCanvas = VisualTreeHelperEx.FindVisualAncestorByType<ViewCanvas>(this);
            this._viewCanvas.PreviewMouseMove += RotateThumb_PreviewMouseMove;

            if (this._viewCanvas != null)
            {
                //We need to subscribe to the view canvas mouse up event because we may not release the button on the rotate element
                this._viewCanvas.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(RotateThumb_PreviewMouseLeftButtonUp);

            }

            this._originalPoint = e.GetPosition(this._viewCanvas);

            double x = this._originalPoint.X - this.ViewModel.CenterX;
            double y = this._originalPoint.Y - this.ViewModel.CenterY;
            double angle = Math.Atan2(x, y);
            this._originalAngle = angle * (180 / Math.PI);

            this._originalRotation = 0;//this.ViewModel.Rotation;

            this._isDragging = true;

            e.Handled = true;
        }

        public ViewModelElement ViewModel
        {
            get
            {
                return (ViewModelElement)base.GetValue(ViewModelProperty);
            }
            set
            {
                base.SetValue(ViewModelProperty, value);
            }
        }

        public ViewContext ViewContext
        {
            get { return (ViewContext)GetValue(ViewContextProperty); }
            set { SetValue(ViewContextProperty, value); }
        }

        public static bool IsZero(double value)
        {
            return (Math.Abs(value) < 2.2204460492503131E-15);
        }
    }
}
