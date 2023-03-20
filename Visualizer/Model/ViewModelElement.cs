using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Visualizer.Model;
using Visualizer.Settings;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public abstract class ViewModelElement : DependencyObject, INotifyPropertyChanged
    {
        public class ViewModelLengthChangedEvent
        {
            int deviceId;
            double lengthChange;

            public int DeviceId
            {
                get
                {
                    return this.deviceId;
                }
            }

            public double LengthChange
            {
                get
                {
                    return this.lengthChange;
                }
            }

            public ViewModelLengthChangedEvent(int id, double lengthDif)
            {
                this.deviceId = id;
                this.lengthChange = lengthDif;
            }
        }

        public class ViewModelHeightChangedEvent
        {
            int deviceId;
            double heightChange;

            public int DeviceId
            {
                get
                {
                    return this.deviceId;
                }
            }

            public double HeightChange
            {
                get
                {
                    return this.heightChange;
                }
            }

            public ViewModelHeightChangedEvent(int id, double heightDif)
            {
                this.deviceId = id;
                this.heightChange = heightDif;
            }
        }

        public class ViewModelMovedEvent
        {
            private double _changeInX;
            private ViewModelElement _viewModelMoved;

            public double ChangeInX
            {
                get { return _changeInX; }
            }

            public ViewModelElement ViewModelMoved
            {
                get { return _viewModelMoved; }
            }

            public ViewModelMovedEvent(double change, ViewModelElement vm)
            {
                this._changeInX = change;
                this._viewModelMoved = vm;
            }
        }


        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(ViewModelElement), new UIPropertyMetadata(0d, new PropertyChangedCallback(WidthChangedCallback)));
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(ViewModelElement), new UIPropertyMetadata(0d, new PropertyChangedCallback(HeightChangedCallback)));
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register("OffsetX", typeof(double), typeof(ViewModelElement), new UIPropertyMetadata(0d, new PropertyChangedCallback(OffsetXChangedCallback)));
        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register("OffsetY", typeof(double), typeof(ViewModelElement), new UIPropertyMetadata(0d, new PropertyChangedCallback(OffsetYChangedCallback)));
        public static readonly DependencyProperty IsSimpleUIEnabledProperty = DependencyProperty.Register("IsSimpleUIEnabled", typeof(bool), typeof(ViewModelElement), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(ViewModelElement), new UIPropertyMetadata(false));
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(double), typeof(ViewModelElement), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(double), typeof(ViewModelElement), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(double), typeof(ViewModelElement), new UIPropertyMetadata(0d));
        //public static readonly DependencyProperty IsLeftToRightProperty = DependencyProperty.Register("IsLeftToRight", typeof(bool), typeof(ViewModelElement), new UIPropertyMetadata(true));
        //public static readonly DependencyProperty IsRightToLeftProperty = DependencyProperty.Register("IsRightToLeft", typeof(bool), typeof(ViewModelElement), new UIPropertyMetadata(false));

        public static readonly DependencyProperty IsResizingAllowedProperty = DependencyProperty.Register("IsResizingAllowed", typeof(bool), typeof(ViewModelElement), new UIPropertyMetadata(true));

        public event EventHandler Moved;
        public event EventHandler SizeChanged;
        public event EventHandler Selected;
        public event EventHandler<OffsetEventArgs> ChangeCenterXRequested;
        private const double _nearZeroWidth = 0.0001;

        protected Rect _worldUnitBounds;    // in Meters


        private int _zOrder;
        private bool _isSelectable;
        private bool _isDebugInfoVisible;
        private bool _isRightToLeft;
        private bool _isLeftToRight;

        private List<ViewModelElement> _children;

        private bool _shouldMoveIntersecting;
        private bool _allowIntersectingToMove;
        private bool _allowWidthResize;
        private bool _allowHeightResize;
        private bool _allowDrag;
        private bool _isMovable;
        private bool _isDeletable;
        private bool _attached;
        private bool _isDynamic;

        private Brush _highlightColor;
        private bool _highlightColorVisibility;
        private ModelElement _modelElement;

        /// <summary>
        /// Gets or sets a flag indicating whether this item should be included in the parent canvas's measure override
        /// </summary>
        public bool IsIncludedInParentMeasure { get; set; }

        public bool IsPropertiesVisible { get; set; }
        public string ThumbBeingDragged { get; set; }

        public bool IsDipUnits { get; set; }
        public ViewCanvas ViewCanvas { get; set; }

        public int DirectionRotation { get; set; }

        protected IViewManager ViewManager { get; }

        public ViewModelElement(IViewManager vm)
            : this(vm, 0d, 0d, 1d, 1d, false)
        {
            this.ViewManager = vm;
            this._worldUnitBounds = new Rect(0d, 0d, 1d, 1d);
            this.IsDipUnits = false;
            this._zOrder = 0;
            this._isSelectable = false;
            this._isDebugInfoVisible = false;
            this._children = new List<ViewModelElement>();

            this._allowWidthResize = true;
            this._allowHeightResize = true;
            this._allowDrag = true;
            this._isMovable = true;
            this._isDeletable = true;
            this._attached = false;
            this._isDynamic = false;

            this.IsIncludedInParentMeasure = true;

            this.Moved -= ViewModelElement_Moved;
            this.Moved += ViewModelElement_Moved;
        }

        private void ViewModelElement_Moved(object sender, EventArgs e)
        {
        }

        public ViewModelElement(IViewManager vm, double x, double y, double width, double height, bool isDipUnits)
        {
            this.ViewManager = vm;
            this._worldUnitBounds = new Rect(x, y, width, height);
            this.IsDipUnits = isDipUnits;
            this._zOrder = 0;
            this._isSelectable = false;
            this._isDebugInfoVisible = false;
            this._children = new List<ViewModelElement>();

            this._allowWidthResize = true;
            this._allowHeightResize = true;
            this._allowDrag = true;
            this._isMovable = true;
            this._isDeletable = true;
            this._attached = false;
            this._isDynamic = false;

            this.IsIncludedInParentMeasure = true;
            this.Moved -= ViewModelElement_Moved;
            this.Moved += ViewModelElement_Moved;

            //if (device != null)
            //{
            //    //causes invalidation
            //    this.ModelElement = device;
            //}
        }

        public virtual void BeginShowPropertyContainer()
        {
        }

        public virtual void SetIsRightToLeft(bool isRightToLeft)
        {
            this.IsRightToLeft = isRightToLeft;
            this.IsLeftToRight = !isRightToLeft;
        }

        protected void OnUIThread(Action action)
        {
            this.Dispatcher.BeginInvoke(action);
        }

        /// <summary>
        /// A helper method that allows viewmodels to attach or detach from other viewmodels
        /// that intersect with this one.
        /// </summary>
        /// <param name="elements">The elements that intersect with this viewmodel</param>
        public virtual void Intersects(List<ViewModelElement> elements)
        {
        }

        public List<ViewModelElement> Children
        {
            get
            {
                return this._children;
            }
        }

        public virtual bool SupportsPropertyContainer
        {
            get
            {
                return false;
            }
        }

        public double IsEnabledOpacity
        {
            get
            {
                return 1;
            }
        }

        public bool IsDynamic
        {
            get
            {
                return this._isDynamic;
            }
            set
            {
                this._isDynamic = value;
            }
        }

        public int ZOrder
        {
            get
            {
                return this._zOrder;
            }
            set
            {
                this._zOrder = value;
            }
        }

        public bool IsSimpleUIEnabled
        {
            get { return (bool)GetValue(IsSimpleUIEnabledProperty); }
            set { SetValue(IsSimpleUIEnabledProperty, value); }
        }


        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set
            {
                SetValue(IsSelectedProperty, value);
                if (value == true)
                {
                    OnSelected(EventArgs.Empty);
                }
                else
                {
                    OnDeselected();
                }
            }
        }

        protected virtual void OnDeselected()
        {
        }

        public virtual bool IsDebugInfoVisible
        {
            get
            {
                return this._isDebugInfoVisible;
            }
            set
            {
                this._isDebugInfoVisible = value;
                OnPropertyChanged("IsDebugInfoVisible");
                OnPropertyChanged("IsDebugInfoNotVisible");
            }
        }

        public virtual bool IsDebugInfoNotVisible
        {
            get
            {
                return !this._isDebugInfoVisible;
            }
        }

        /// <summary>
        /// Gets whether this item should move items that intersect it
        /// </summary>
        public bool ShouldMoveIntersecting
        {
            get
            {
                return this._shouldMoveIntersecting;
            }
            set
            {
                this._shouldMoveIntersecting = value;
            }
        }

        /// <summary>
        /// Gets whether intersecting items that are being moved are allowed to move this item
        /// </summary>
        public bool AllowIntersectingToMove
        {
            get
            {
                return this._allowIntersectingToMove;
            }
            set
            {
                this._allowIntersectingToMove = value;
            }
        }

        public bool AllowWidthResize
        {
            get
            {
                return this._allowWidthResize;
            }
            set
            {
                this._allowWidthResize = value;
            }
        }

        public bool AllowHeightResize
        {
            get
            {
                return this._allowHeightResize;
            }
            set
            {
                this._allowHeightResize = value;
            }
        }

        public bool AllowDrag
        {
            get
            {
                return this._allowDrag;
            }
            set
            {
                this._allowDrag = value;
            }
        }

        public virtual bool IsMovable
        {
            get
            {
                return this._isMovable;
            }
            set
            {
                this._isMovable = value;
            }
        }

        public bool IsDeletable
        {
            get
            {
                return this._isDeletable;
            }
            set
            {
                this._isDeletable = value;
            }
        }

        public bool Attached
        {
            get { return this._attached; }
            set { this._attached = value; }
        }

        //public bool IsSupport
        //{
        //   get
        //{
        ////        if (CurrentUser.Instance.User == User.Support)
        //          return true;
        ////        else
        ////            return false;
        //  }
        //}


        protected virtual double WorldUnitsToDisplayPixels(double value)
        {
            if (this.IsDipUnits == true)
            {
                return value;
            }
            return MeasurementUnits.ConvertWorldUnitsToDisplayPixels(value);
        }

        protected virtual double DisplayPixelsToWorldUnits(double value)
        {
            if (this.IsDipUnits == true)
            {
                return value;
            }
            return MeasurementUnits.ConvertDisplayPixelsToWorldUnits(value);
        }

        /// <summary>
        /// Gets or sets the unique reference identifier for a viewmodel in a list of viewmodels.
        /// It is the owner of the list's responsibility to ensure reference Ids are unique
        /// </summary>
        public int ObjectId
        {
            get
            {
                return -1;
            }
        }

        public Rect WorldUnitBounds
        {
            get { return new Rect(_worldUnitBounds.X, _worldUnitBounds.Y, _worldUnitBounds.Width, _worldUnitBounds.Height); }
        }

        public Rect Bounds
        {
            get
            {
                return new Rect(OffsetX, OffsetY, Width, Height);
            }
        }

        public double Right
        {
            get
            {
                return WorldUnitsToDisplayPixels(this._worldUnitBounds.Right);
            }
        }

        public Brush HighlightColor
        {
            get
            {
                return this._highlightColor;
            }
            set
            {
                this._highlightColor = value;
                OnPropertyChanged("HighlightColor");
            }
        }

        public bool HighlightColorVisibility
        {
            get
            {
                return _highlightColorVisibility;
            }
            set
            {
                this._highlightColorVisibility = value;
                OnPropertyChanged("HighlightColorVisibility");
            }
        }

        public bool? ShowSettingsWindow(object settingsVM, string vmName, out object settings)
        {
            RadWindow window = new RadWindow();
            window.Owner = System.Windows.Application.Current.MainWindow;
            window.HideMaximizeButton = true;
            window.HideMinimizeButton = true;
            window.Header = vmName + " Settings";
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            Point positionInApp = System.Windows.Input.Mouse.GetPosition(Application.Current.MainWindow);
            Point mousePosition = Application.Current.MainWindow.PointToScreen(positionInApp);
            window.Left = mousePosition.X;
            window.Top = mousePosition.Y;

            window.Content = settingsVM;
            window.Loaded += Window_Loaded;

            window.ShowDialog();

            settings = settingsVM;

            return window.DialogResult;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RadWindow window = e.OriginalSource as RadWindow;

            double configWindowRight = System.Windows.Application.Current.MainWindow.Left + 1600;
            double configWindowBottom = System.Windows.Application.Current.MainWindow.Top + 900;

            ////check for positioning to make sure window isn't off the left side of the screen
            if (window.Left + window.ActualWidth > configWindowRight) //size of the config editor, not the app window
            {
                //properties window is too far right off the screen, so move back in to the left a bit
                double difference = (window.Left + window.ActualWidth) - configWindowRight;

                window.Left -= (difference + 120); //added on a buffer to the right shift
            }

            ////check the same for bottom of the screen
            if (window.Top + window.ActualHeight > configWindowBottom) //size of the config editor, not the app window
            {
                //properties window is too far off the bottom of the screen, so move it back up a bit
                double difference = (window.Top + window.ActualHeight) - configWindowBottom;

                window.Top -= difference;
            }
        }

        public void SetBoundsMeters(double x, double y)
        {
            this._worldUnitBounds.X = x;
            this._worldUnitBounds.Y = y;
            InvalidateBounds();
            InvalidateCenterXProperty();
            InvalidateCenterYProperty();
        }

        public void SetBoundsMeters(double x, double y, double width)
        {
            this._worldUnitBounds.X = x;
            this._worldUnitBounds.Y = y;
            this._worldUnitBounds.Width = width;
            InvalidateBounds();
            InvalidateCenterXProperty();
            InvalidateCenterYProperty();
        }

        public void SetBoundsMeters(double x, double y, double width, double height)
        {
            this._worldUnitBounds.X = x;
            this._worldUnitBounds.Y = y;
            this._worldUnitBounds.Width = width;
            this._worldUnitBounds.Height = height;
            InvalidateBounds();
            InvalidateCenterXProperty();
            InvalidateCenterYProperty();
        }

        public void InvalidateBounds()
        {
            InvalidateWidthProperty();
            InvalidateHeightProperty();
            InvalidateOffsetXProperty();
            InvalidateOffsetYProperty();
            InvalidateBottomProperty();
        }

        public void SetCenterX(double centerX)
        {
            this._worldUnitBounds.X = DisplayPixelsToWorldUnits(centerX) - 0.5d * this._worldUnitBounds.Width;
            InvalidateOffsetXProperty();
            InvalidateCenterXProperty();
        }

        public void SetCenterY(double centerY)
        {
            this._worldUnitBounds.Y = DisplayPixelsToWorldUnits(centerY) - 0.5d * this._worldUnitBounds.Height;
            InvalidateOffsetYProperty();
            InvalidateCenterYProperty();
        }

        protected void InvalidateWidthProperty()
        {
            double width = WorldUnitsToDisplayPixels(this._worldUnitBounds.Width);
            if (width != this.Width)
            {
                this.Width = width;
            }
        }

        protected void InvalidateHeightProperty()
        {
            double height = WorldUnitsToDisplayPixels(this._worldUnitBounds.Height);
            if (height != this.Height)
            {
                this.Height = height;
            }
        }

        protected void InvalidateOffsetXProperty()
        {
            double offsetX = WorldUnitsToDisplayPixels(this._worldUnitBounds.X);
            if (offsetX != this.OffsetX)
            {
                this.OffsetX = offsetX;
            }
        }

        protected void InvalidateOffsetYProperty()
        {
            double offsetY = WorldUnitsToDisplayPixels(this._worldUnitBounds.Y);
            if (offsetY != this.OffsetY)
            {
                this.OffsetY = offsetY;
            }
        }

        protected void InvalidateCenterXProperty()
        {
            double centerX = this.OffsetX + 0.5d * this.Width;
            if (centerX != this.CenterX)
            {
                this.CenterX = centerX;
            }
        }

        protected void InvalidateCenterYProperty()
        {
            double centerY = this.OffsetY + 0.5d * this.Height;
            if (centerY != this.CenterY)
            {
                this.CenterY = centerY;
            }
        }

        protected void InvalidateBottomProperty()
        {
            double bottom = WorldUnitsToDisplayPixels(this._worldUnitBounds.Bottom);
            if (bottom != this.Bottom)
            {
                this.Bottom = bottom;
            }
        }

        private static void WidthChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewModelElement viewModel = (ViewModelElement)d;
            double incrementWidth = (double)e.NewValue - (double)e.OldValue;
            viewModel._worldUnitBounds.Width = viewModel.DisplayPixelsToWorldUnits((double)e.NewValue);
            viewModel.InvalidateCenterXProperty();
            viewModel.InvalidateCenterYProperty();

            viewModel.OnBoundsChanged(0d, 0d, incrementWidth, 0d);
        }

        private static void HeightChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewModelElement viewModel = (ViewModelElement)d;
            double incrementHeight = (double)e.NewValue - (double)e.OldValue;
            viewModel._worldUnitBounds.Height = viewModel.DisplayPixelsToWorldUnits((double)e.NewValue);
            viewModel.InvalidateCenterXProperty();
            viewModel.InvalidateCenterYProperty();

            viewModel.OnBoundsChanged(0d, 0d, 0d, incrementHeight);
        }

        private static void OffsetXChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewModelElement viewModel = (ViewModelElement)d;

            double incrementX = (double)e.NewValue - (double)e.OldValue;
            viewModel._worldUnitBounds.X = viewModel.DisplayPixelsToWorldUnits((double)e.NewValue);
            viewModel.InvalidateCenterXProperty();
            viewModel.OnBoundsChanged(incrementX, 0d, 0d, 0d);
        }

        private static void OffsetYChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewModelElement viewModel = (ViewModelElement)d;

            double incrementY = (double)e.NewValue - (double)e.OldValue;
            viewModel._worldUnitBounds.Y = viewModel.DisplayPixelsToWorldUnits((double)e.NewValue);
            viewModel.InvalidateCenterYProperty();
            viewModel.InvalidateBottomProperty();
            viewModel.OnBoundsChanged(0d, incrementY, 0d, 0d);
        }

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        public double Bottom
        {
            get { return (double)GetValue(BottomProperty); }
            set { SetValue(BottomProperty, value); }
        }

        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        public bool IsLeftToRight
        {
            get { return _isLeftToRight; }
            set { _isLeftToRight = value; }
        }

        public bool IsRightToLeft
        {
            get { return _isRightToLeft; }
            set { _isRightToLeft = value; }
        }

        /// <summary>
        /// Gets or sets whether the bounds thumb has permission to resize the element
        /// </summary>
        public bool IsResizingAllowed
        {
            get { return (bool)GetValue(IsResizingAllowedProperty); }
            set { SetValue(IsResizingAllowedProperty, value); }
        }

        public bool IsSelectable
        {
            get
            {
                return this._isSelectable;
            }
            set
            {
                this._isSelectable = value;
            }
        }

        public virtual void UpdateUIBindings(BindingUpdateInterval interval)
        {
        }

        public virtual void OnMoved(EventArgs e)
        {
            if (this.Moved != null)
            {
                this.Moved(this, e);
            }
        }

        protected virtual void OnSizeChanged(EventArgs e)
        {
            if (this.SizeChanged != null)
            {
                this.SizeChanged(this, e);
            }
        }

        protected virtual void OnSelected(EventArgs e)
        {
            if (this.Selected != null)
            {
                this.Selected(this, e);
            }
        }

        //refreshes a viewmodel layout
        public void ForceBoundsChanged()
        {
            OnBoundsChanged(0, 0, 0, 0);
        }

        //gives children chance to update any of their information (such as moving endpoints)
        protected virtual void OnBoundsChanged(double incrementX, double incrementY, double incrementWidth, double incrementHeight)
        {
        }

        public virtual void UpdateDeviceReferences()
        {

        }

        public ModelElement ModelElement
        {
            get
            {
                return this._modelElement;
            }
            protected set
            {
                this._modelElement = value;
                InvalidateBounds();
            }
        }

        protected ViewModelElement FindViewModel(int deviceId)
        {
            ViewModelElement vm = null;

            vm = this.ViewManager.ViewState.TopLevelItems.FirstOrDefault(x => x.ObjectId == deviceId);

            return vm;
        }

        protected ViewModelElement FindViewModel<T>(int deviceId)
        {
            ViewModelElement vm = null;

            vm = this.ViewManager.ViewState.TopLevelItems.FirstOrDefault(x => x.ObjectId == deviceId && x.GetType() == typeof(T));

            return vm;
        }

        //public void RequestChangeCenterX(double centerX)
        //{
        //    OnChangeCenterXRequested(new OffsetEventArgs(centerX));
        //}

        //public IEnumerable<T> GetModelsIntersectingCenter<T>(List<T> models)
        //    where T : ModelElement
        //{
        //    return models.Where(model =>
        //    {
        //        Rect bounds = this.Bounds;
        //        bounds.Inflate(-(bounds.Width * 0.5d - 0.000001d), 0);
        //        return bounds.IntersectsWith(model.Bounds);
        //    });
        //}

        //public bool GetIsModelIntersectingCenter<T>(List<T> models)
        //    where T : ModelElement
        //{
        //    return models.Any(model =>
        //    {
        //        Rect bounds = this.Bounds;
        //        bounds.Inflate(-(bounds.Width * 0.5d - 0.000001d), 0);
        //        return bounds.IntersectsWith(model.Bounds);
        //    });
        //}

        //public bool GetIsModelIntersectingCenter<T>(T model)
        //    where T : ModelElement
        //{
        //    if (model == null)
        //    {
        //        return false;
        //    }
        //    Rect bounds = this.Bounds;
        //    bounds.Inflate(-(bounds.Width * 0.5d - 0.000001d), 0);
        //    return bounds.IntersectsWith(model.Bounds);
        //}

        //public bool GetIsModelIntersectingCenter<T>(T model, double offsetFromCenter)
        //    where T : ModelElement
        //{
        //    if (model == null)
        //    {
        //        return false;
        //    }

        //    Rect bounds = this.Bounds;

        //    double centerX = bounds.Left + (bounds.Width / 2);
        //    double offsetDistanceFromCenter = centerX + offsetFromCenter;

        //    bounds = new Rect(offsetDistanceFromCenter, bounds.Y, _nearZeroWidth, bounds.Height);
        //    return bounds.IntersectsWith(model.Bounds);
        //}

        //public bool GetIsModelIntersectingLeft<T>(T model)
        //    where T : ModelElement
        //{
        //    if (model == null)
        //    {
        //        return false;
        //    }
        //    Rect bounds = this.Bounds;
        //    bounds = new Rect(bounds.X, bounds.Y, 0.00001d, bounds.Height);
        //    return bounds.IntersectsWith(model.Bounds);
        //}

        //public bool GetIsModelIntersectingRight<T>(T model)
        //    where T : ModelElement
        //{
        //    if (model == null)
        //    {
        //        return false;
        //    }
        //    Rect bounds = this.Bounds;
        //    bounds = new Rect(bounds.TopRight.X - 0.00001d, bounds.Y, 0.00001d, bounds.Height);
        //    return bounds.IntersectsWith(model.Bounds);
        //}

        //public int GetIntersectingModelCount<T>(List<T> models)
        //    where T : ModelElement
        //{
        //    return models.Count(model => this.Bounds.IntersectsWith(model.Bounds) == true);
        //}

        //public bool GetIsModelIntersecting<T>(List<T> models)
        //    where T : ModelElement
        //{
        //    return models.Any(model => this.Bounds.IntersectsWith(model.Bounds) == true);
        //}

        ////public List<T> GetIntersectingCenterModels<T>(List<T> models)
        ////    where T : ModelElement
        ////{
        ////    if (models == null)
        ////    {
        ////        return null;
        ////    }

        ////    List<T> intersectingModels = null;

        ////    Rect bounds = this.Bounds;
        ////    bounds.Inflate(-(bounds.Width * 0.5d - 0.000001d), 0);

        ////    foreach (T modelElement in models)
        ////    {
        ////        if (bounds.IntersectsWith(modelElement.Bounds) == true)
        ////        {
        ////            //only create list if needed
        ////            if (intersectingModels == null)
        ////            {
        ////                intersectingModels = new List<T>();
        ////            }
        ////            intersectingModels.Add(modelElement);
        ////        }
        ////    }
        ////    return intersectingModels;
        ////}

        //public void GetIntersectingModels<T>(List<T> models, List<T> output, bool fromEachModelCenter)
        //    where T : ModelElement
        //{
        //    output.Clear();

        //    if (models == null)
        //    {
        //        return;
        //    }

        //    //List<T> intersectingModels = null;

        //    Rect bounds = this.Bounds;

        //    foreach (T modelElement in models)
        //    {
        //        Rect elementBounds = modelElement.Bounds;
        //        if (fromEachModelCenter == true)
        //        {
        //            elementBounds.Inflate(-(elementBounds.Width * 0.5d - 0.000001d), 0);
        //        }

        //        if (bounds.IntersectsWith(elementBounds) == true)
        //        {
        //            //only create list if needed
        //            //if (intersectingModels == null)
        //            //{
        //            //    intersectingModels = new List<T>();
        //            //}
        //            output.Add(modelElement);
        //        }
        //    }
        //    //return intersectingModels;
        //}

        //public void GetIntersectingModels<T>(List<T> models, List<T> output)
        //    where T : ModelElement
        //{
        //    GetIntersectingModels(models, output, false);
        //}

        //public static T GetIntersectingModel<T>(Point point, List<T> models)
        //    where T : ModelElement
        //{
        //    if (models == null)
        //    {
        //        return null;
        //    }

        //    for (int i = 0; i < models.Count; ++i)
        //    {
        //        if (models[i].Bounds.Contains(point) == true)
        //        {
        //            return models[i];
        //        }
        //    }
        //    return null;
        //}

        ////public List<ModelElement> GetIntersectingModels(List<ModelElement> models)
        ////{
        ////    if (models == null)
        ////    {
        ////        return null;
        ////    }

        ////    List<ModelElement> intersectingModels = null;

        ////    foreach (ModelElement modelElement in models)
        ////    {
        ////        if (this.Bounds.IntersectsWith(modelElement.Bounds) == true)
        ////        {
        ////            //only create list if needed
        ////            if (intersectingModels == null)
        ////            {
        ////                intersectingModels = new List<ModelElement>();
        ////            }
        ////            intersectingModels.Add(modelElement);
        ////        }
        ////    }
        ////    return intersectingModels;
        ////}

        ////protected virtual void OnChangeOffsetXRequested(OffsetEventArgs e)
        ////{
        ////    if (this.ChangeOffsetXRequested != null)
        ////    {
        ////        this.ChangeOffsetXRequested(this, e);
        ////    }
        ////}

        //protected virtual void OnChangeCenterXRequested(OffsetEventArgs e)
        //{
        //    if (this.ChangeCenterXRequested != null)
        //    {
        //        this.ChangeCenterXRequested(this, e);
        //    }
        //}

        ///// <summary>
        ///// Get the display matrix
        ///// </summary>
        //public Matrix RenderTransform
        //{
        //    get
        //    {
        //        Matrix m = Matrix.Identity;
        //        m.Translate(this._bounds.X, this._bounds.Y);
        //        return m;
        //    }
        //}

        //public double CenterX
        //{
        //    get
        //    {
        //        return this.OffsetX + this.Width * 0.5d;
        //    }
        //    set
        //    {
        //        this.OffsetX = value - this.Width * 0.5d;
        //    }
        //}
        //public double CenterY
        //{
        //    get
        //    {
        //        return this.OffsetY + this.Height * 0.5d;
        //    }
        //}

        //public Rect Bounds
        //{
        //    get
        //    {
        //        return this._bounds;
        //    }
        //    set
        //    {
        //        this._bounds = value;
        //    }
        //}

        //protected virtual void OnOffsetXChangedOverride()
        //{
        //}

        //protected virtual void OnOffsetYChangedOverride()
        //{
        //}

        ////#region INotifyPropertyChanged Members

        /////// <summary>
        /////// Notify property changed
        /////// </summary>
        /////// <param name="propertyName">Property name</param>
        ////protected void OnPropertyChanged(string propertyName)
        ////{
        ////    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        ////}

        ////public event PropertyChangedEventHandler PropertyChanged;

        ////#endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        //protected virtual void OnPropertyChanged(Expression<Func<object>> expression)
        //{
        //    this.PropertyChanged.Raise(this, expression);
        //}

        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, args);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion



        public virtual IModelElementSettings Serialize()
        {
            throw new NotImplementedException();
        }

        public virtual void PostLoadResolveReferences(List<ViewModelElement> referenceCollection)
        {
        }

        //public virtual void UpdateRelativePosition(List<ViewModelElement> intersectingViewModels)
        //{
        //}

        public virtual void Unsubscribe()
        {

        }

        public virtual void Detach()
        {
        }
    }
}
