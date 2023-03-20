using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class BoundsThumb : Thumb
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ViewModelElement), typeof(BoundsThumb));
        public static readonly DependencyProperty ViewContextProperty = DependencyProperty.Register("ViewContext", typeof(ViewContext), typeof(BoundsThumb), new UIPropertyMetadata(null));
        TransformCommand _transformCommand = new TransformCommand();
        static BoundsThumb()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BoundsThumb), new FrameworkPropertyMetadata(typeof(BoundsThumb)));
        }

        public BoundsThumb()
            : base()
        {
            this.DragDelta += new DragDeltaEventHandler(MyThumb_DragDelta);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            _transformCommand.Execute(new List<ViewModelElement>() { this.ViewModel });
            base.OnMouseLeftButtonDown(e);
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            _transformCommand.Execute(new List<ViewModelElement>() { this.ViewModel });
            _transformCommand = new TransformCommand();
        }

        void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double originalCenterX = this.ViewModel.CenterX;
            double originalCenterY = this.ViewModel.CenterY;

            double horizontalChange = e.HorizontalChange;
            double verticalChange = e.VerticalChange;

            double width = this.ViewModel.Width;
            double height = this.ViewModel.Height;

            double rotationAngle = 0;

            if (true)
            {
                if (true)
                {
                    double deltaX = horizontalChange;
                    double deltaY = verticalChange;
                    double h = horizontalChange * width / height;
                    double v = verticalChange * height / width;

                    if (this.HorizontalAlignment == HorizontalAlignment.Left)
                    {
                        //topleft corner
                        if (this.VerticalAlignment == VerticalAlignment.Top)
                        {
                            if (h > v)
                            {
                                deltaY = horizontalChange * height / width;
                            }
                            else
                            {
                                deltaX = verticalChange * width / height;
                            }
                            this.ViewModel.Width = Math.Max(1d, width - deltaX);
                            this.ViewModel.Height = Math.Max(1d, height - deltaY);
                            this.ViewModel.OffsetX += deltaX;
                            this.ViewModel.OffsetY += deltaY;
                        }
                        //left side and bottom left
                        if (this.VerticalAlignment == VerticalAlignment.Stretch || this.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            deltaY = horizontalChange * height / width;

                            this.ViewModel.Width = Math.Max(1d, width - deltaX);
                            this.ViewModel.Height = Math.Max(1d, height - deltaY);
                            this.ViewModel.OffsetX += deltaX;
                        }
                    }

                    if (this.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        //top
                        if (this.VerticalAlignment == VerticalAlignment.Top)
                        {
                            deltaX = verticalChange * width / height;

                            this.ViewModel.Width = Math.Max(1d, width - deltaX);
                            this.ViewModel.Height = Math.Max(1d, height - deltaY);
                            this.ViewModel.OffsetY += deltaY;
                        }

                        //bottom
                        if (this.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            deltaX = verticalChange * width / height;

                            this.ViewModel.Width = Math.Max(1d, width + deltaX);
                            this.ViewModel.Height = Math.Max(1d, height + deltaY);
                        }
                    }

                    if (this.HorizontalAlignment == HorizontalAlignment.Right)
                    {
                        //topright corner
                        if (this.VerticalAlignment == VerticalAlignment.Top)
                        {
                            //support only horizontal sizing
                            deltaY = horizontalChange * height / width;

                            this.ViewModel.Width = Math.Max(1d, width + deltaX);
                            this.ViewModel.Height = Math.Max(1d, height + deltaY);
                        }

                        //right side
                        if (this.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            deltaY = horizontalChange * height / width;

                            this.ViewModel.Width = Math.Max(1d, width + deltaX);
                            this.ViewModel.Height = Math.Max(1d, height + deltaY);
                        }

                        //bottom right corner
                        if (this.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            if (h > v)
                            {
                                deltaY = horizontalChange * height / width;
                            }
                            else
                            {
                                deltaX = verticalChange * width / height;
                            }

                            this.ViewModel.Width = Math.Max(1d, width + deltaX);
                            this.ViewModel.Height = Math.Max(1d, height + deltaY);
                        }
                    }
                }
                else //otherwise no aspect ratio locking so do normal resize
                {
                    switch (this.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            if (IsZero(rotationAngle) == true)
                            {
                                this.ViewModel.OffsetX += horizontalChange;
                            }
                            this.ViewModel.Width = Math.Max(1d, width - horizontalChange);
                            break;
                        case HorizontalAlignment.Right:
                            this.ViewModel.Width = Math.Max(1d, width + horizontalChange);
                            break;
                    }
                    switch (this.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            if (IsZero(rotationAngle) == true)
                            {
                                this.ViewModel.OffsetY += verticalChange;
                            }
                            this.ViewModel.Height = Math.Max(1d, height - verticalChange);
                            break;
                        case VerticalAlignment.Bottom:
                            this.ViewModel.Height = Math.Max(1d, height + verticalChange);
                            break;
                        default:
                            break;
                    }
                }
            }

            //keep element on center point when rotated
            if (IsZero(rotationAngle) == false)
            {
                this.ViewModel.CenterX = originalCenterX;
                this.ViewModel.CenterY = originalCenterY;
            }

            if (this.ViewContext != null)
            {
               // this.ViewContext.RaiseSelectionBoundsChanged();
            }

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
