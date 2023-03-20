using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Visualizer.View;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class ViewCanvas : Canvas, INotifyPropertyChanged
    {
        public class ViewModelViewCanvasDragEvent
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

            public ViewModelViewCanvasDragEvent(double change, ViewModelElement vm)
            {
                this._changeInX = change;
                this._viewModelMoved = vm;
            }
        }

        private Point _origCursorLocation;
        private bool _isDragging;
        private bool _isSelecting;
        private SelectionAdorner _selectionAdorner;
        //private ElementPropertiesContainerViewModel _elementPropertiesContainerViewModel;
        private AdornerContentPresenter _elementPropertiesContainerAdorner;
        //private AnchorEndpointViewModel _previouslyIntersectedAnchor;
        private bool _isMoving;

        public static bool _canEditLayout { get; set; }

        /// <summary>
        /// Ges the ViewContext for this canvas
        /// </summary>
        public ViewContext ViewContext { get { return this.DataContext as ViewContext; } }

        public ViewCanvas()
        {
            this._isDragging = false;
            this._isSelecting = false;
            this._selectionAdorner = null;
            this.MouseLeftButtonDown += WorldSurface_MouseButtonDown;
            this.MouseLeftButtonUp += WorldSurface_MouseLeftButtonUp;
            this.PreviewMouseLeftButtonUp += ViewCanvas_PreviewMouseLeftButtonUp;
            this.MouseRightButtonDown += WorldSurface_MouseButtonDown;
            this.MouseRightButtonUp += ViewCanvas_MouseRightButtonUp;
            // this.MouseRightButtonUp += ViewCanvas_MouseRightButtonUp;

            this.MouseMove += WorldSurface_MouseMove;
            this.PreviewKeyDown += ViewCanvas_PreviewKeyDown;
        }

        private void ViewCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._isMoving = false;
            this._isDragging = false;

            Border border = e.Source as Border;

            if (border != null)
            {
                ViewModelElement regionVM = border.DataContext as ViewModelElement;
                string typeString = regionVM.GetType().ToString();
                if (!typeString.Contains("RegionViewModel") && !typeString.Contains("Region2ViewModel"))
                    DeselectAll();
            }
        }

        private void ViewCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool wasMoving = this._isMoving;

            if (this._isDragging)
            {
                foreach (ViewModelElement element in this.ViewContext.SelectedViewModels)
                {
                    element.OnMoved(new EventArgs());
                }
            }

            this._isDragging = false;

            // if we were selecting ViewModels in a selection rectangle, perform the selection and clear the selection adorner
            if (this._isSelecting == true)
            {
                this._selectionAdorner.Update(e.GetPosition(this));
                SelectViews(this._selectionAdorner.SelectionBounds);
                AdornerLayer.GetAdornerLayer(this).Remove(this._selectionAdorner);
                this._selectionAdorner = null;
                this._isSelecting = false;
            }
            else
            {
                this._isMoving = false;
                this._isDragging = false;
            }
        }

        /// <summary>
        /// Handles key down presses to move selectedelements in a direction
        /// </summary>
        void ViewCanvas_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //this is an ugly technique to allow keypresses to properly work in the Shutterfly's editor
            //if (e.OriginalSource is FreeTextEntryBox)
            //{
            //    return;
            //}

            //this is an ugly technique to allow keypresses to properly work in the inserter's product's editor window
            if (e.OriginalSource is TextBox)
            {
                if (((TextBox)e.OriginalSource).Name == "PART_TextBoxInserterCustomProductData")
                {
                    return;
                }

                //add support for RFID Lot module
                if (((TextBox)e.OriginalSource).Name == "PART_TextBoxRFIDLot")
                {
                    return;
                }
            }

            // ignore event if we are currently dragging
            if (this._isDragging == true)
            {
                return;
            }

            //only handle when one items is selected
            if (this.ViewContext.SelectedViewModels.Count != 0)
            {
                return;
            }

            e.Handled = true;

            //move element by 0.25 inches
            double pixelOffset = MeasurementUnits.ConvertWorldUnitsToDisplayPixels(MeasurementUnits.ConvertInchesToMeters(0.25d));

            switch (e.Key)
            {
                case System.Windows.Input.Key.Left:
                    pixelOffset = -pixelOffset;
                    break;
                case System.Windows.Input.Key.Right:
                    break;
                default:
                    return;
            }

            // move all selected ViewModels
            foreach (ViewModelElement element in this.ViewContext.SelectedViewModels)
            {
                if (element.IsMovable == true) /*&& ((element is SourceEndpointViewModel) == false))*/
                {
                    element.OffsetX += pixelOffset;

                    if (element.ShouldMoveIntersecting == true)
                    {
                        List<ViewModelElement> intersectingElements = GetIntersectingViewModels(element);
                        foreach (ViewModelElement intersectingElement in intersectingElements)
                        {
                            intersectingElement.OffsetX += pixelOffset;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deselects all selected ViewModels
        /// </summary>
        private void DeselectAll()
        {
            this.ViewContext.DeselectAll();
            CloseElementPropertiesAdorner();
        }

        /// <summary>
        /// Provides functionality for selecting a single ViewModel or beginning a drag operation that selects multiple
        /// </summary>
        void WorldSurface_MouseButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // a backdoor hack to allow Kevin Kerr to do garbage collections at any time
            //if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            //{
            //    GC.Collect();
            //    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Alt)
            //    {
            //        GC.Collect();
            //    }
            //}
#if PREVENT_SELECTION_IN_JOB_MODE
            //if (this.ViewContext.AllowMove == false)
            //{
            //    return;
            //}
#endif

            ////ignore if both left and right mouse button are pressed
            //if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Pressed)
            //{
            //    return;
            //}

            //if (e.RightButton == MouseButtonState.Pressed)
            //{
            //    return;
            //}

            ////if the canvas was clicked, deselect any selected views, and start
            ////the selection rectangle
            //if (e.Source == this && this.ViewContext != null)
            //{
            //    if (this.ViewContext.SelectedViewModels.Count > 0)
            //    {
            //        DeselectAll();
            //    }

            //    //if we allow move, we can allow multiple selection
            //    if (this.ViewContext.AllowMove == true)
            //    {
            //        if (e.LeftButton == MouseButtonState.Pressed && this._isSelecting == false)
            //        {
            //            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            //            this._selectionAdorner = new SelectionAdorner(this, e.GetPosition(this), this.ActualWidth, this.ActualHeight);
            //            adornerLayer.Add(this._selectionAdorner);
            //            this._isSelecting = true;
            //        }
            //    }
            //}
            //else // If any view is clicked, select the view
            //{
            //    // Walk up the visual tree from the element that was clicked, 
            //    // looking for an element that is a direct child of the Canvas.
            //    ElementViewBase selectedView = FindCanvasView(e.Source as DependencyObject);
            //    if (selectedView != null)
            //    {
            //        ViewModelElement element = selectedView.ViewModel;
            //        if (element.IsSelectable == true && element.IsSelected == false)
            //        {
            //            DeselectAll();
            //            //element.IsSelected = true;
            //            this.ViewContext.SelectedViewModels.Add(element);
            //            this.ViewContext.SelectedViewModel = element;
            //        }

            //        if (this.ViewContext.AllowMove == true)
            //        {
            //            if (e.LeftButton == MouseButtonState.Pressed)
            //            {
            //                this._origCursorLocation = e.GetPosition(this);
            //                //this._isDragging = true;
            //            }
            //        }
            //    }

            //    if (Focus() == false)
            //    {
            //    }

                // e.Handled = true;
           // }

            ////Add for IWCO to enable operators to change device setttings without allowing them to create new configurations
            //if (_canEditLayout == true && !this.ViewContext.IsEditingLayout && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            //{
            //    if (this.ViewContext.SelectedViewModel != null && this.ViewContext.SelectedViewModel.SupportsPropertyContainer == true
            //            && this.ViewContext.SelectedViewModel.IsAllowedEditInXmatch && this._elementPropertiesContainerAdorner == null)
            //    {
            //        ViewCommands.PreviewElementShowProperties.Execute(this.ViewContext.SelectedViewModel, this);

            //        AdornerLayer al = AdornerLayer.GetAdornerLayer(this);
            //        this._elementPropertiesContainerViewModel = new ElementPropertiesContainerViewModel(this.ViewContext.SelectedViewModel);

            //        this._elementPropertiesContainerAdorner = new AdornerContentPresenter(this, this._elementPropertiesContainerViewModel);
            //        al.Add(this._elementPropertiesContainerAdorner);
            //    }
            //}
        }

        /// <summary>
        /// Handle mouse movement for dragging ViewModels or drawing the selection rectangle
        /// </summary>
        void WorldSurface_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.ViewContext != null)
            {
                if (this.ViewContext.AllowMove == false)
                {
                    return;
                }

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (this.ViewContext.SelectedViewModel != null)
                    {
                        if (this.ViewContext.SelectedViewModel.AllowDrag)
                            this._isDragging = true;
                    }
                }
                else if (e.LeftButton == MouseButtonState.Released)
                {
                    this._isDragging = false;
                    this._isMoving = false;
                }

                if (this._isDragging == true)
                {
                    if (this.ViewContext.SelectedViewModels.Count > 0)
                    {
                        // Get the position of the mouse cursor, relative to the Canvas.
                        Point cursorLocation = e.GetPosition(this);

                        foreach (ViewModelElement element in this.ViewContext.SelectedViewModels)
                        {
                            Type type = element.GetType();
                            //added for IWCO to only allow support users to move elements in a configuration
                            //if (XJetInfo.IWCOReadAndPrintLock)
                            //{
                            //    if (CurrentUser.Instance.User == User.Operator)
                            //    {
                            //        return;
                            //    }

                            //    else if ((CurrentUser.Instance.User == User.Technician) && (type.Name != "BarcodeRegionViewModel" && type.Name != "OcrRegionViewModel"))
                            //    {
                            //        return;
                            //    }
                            //}

                            if (element.IsMovable == true)
                            {
                                double deltaX = (cursorLocation.X - this._origCursorLocation.X);
                                double deltaY = (cursorLocation.Y - this._origCursorLocation.Y);

                                bool performMove = true;

                                if (deltaX == 0 && deltaY == 0)
                                {
                                    performMove = false;
                                }
                                else
                                {
                                    this._isMoving = true;
                                }

                                if (Double.IsNaN(this.Width) == false && Double.IsNaN(this.Height) == false)
                                {
                                    if (element.OffsetX + deltaX + element.Width > this.Width)
                                    {
                                        deltaX = this.Width - element.OffsetX - element.Width;
                                    }
                                    if (element.OffsetY + deltaY + element.Height > this.Height)
                                    {
                                        deltaY = this.Height - element.OffsetY - element.Height;
                                    }

                                    if (element.OffsetX + deltaX < 0)
                                    {
                                        deltaX = -element.OffsetX;
                                    }

                                    if (element.OffsetY + deltaY < 0)
                                    {
                                        deltaY = -element.OffsetY;
                                    }
                                }

                                if (performMove == true)
                                {
                                    //move model
                                    //if we're attached to something, check the rotation and only move it in that direction
                                    //hopefully this will avoid things from being dragged off the conveyor by accident
                                    if (!element.Attached)
                                    {
                                        element.OffsetX += deltaX;
                                        element.OffsetY += deltaY;
                                    }
                                    else
                                    {
                                        if (element.DirectionRotation != 0)
                                        {
                                            element.OffsetY += deltaY;
                                        }
                                        else
                                        {
                                            element.OffsetX += deltaX;
                                        }
                                    }

                                    //List<ViewModelElement> intersectingElements = GetIntersectingViewModels(element);
                                    //List<ViewModelElement> attachedElements = GetAttachedViewModels(element);

                                    ////update position of intersecting elements
                                    //if (element.ShouldMoveIntersecting == true)
                                    //{
                                    //    foreach (ViewModelElement intersectingElement in attachedElements)
                                    //    {
                                    //        if (element.Configuration.GetType().Name == "Conveyor" || element.Configuration.GetType().Name == "TakeAway")
                                    //        {
                                    //            Type t = element.GetType();
                                    //            if (element.Configuration.Id == intersectingElement.Configuration.GetConveyorId())
                                    //            {
                                    //                //if the intersecting item is allowed to move, then move it
                                    //                //also make sure it is not one of the selected items
                                    //                if (this.ViewContext.SelectedViewModels.Contains(intersectingElement) == false)
                                    //                {
                                    //                    intersectingElement.OffsetX += deltaX;
                                    //                    intersectingElement.OffsetY += deltaY;
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //else //otherwise the element should not move others that intersect, but it may want to attach/detach from some of them
                                    //{
                                    //    element.Intersects(intersectingElements);
                                    //}
                                }

                                ViewCommands.ElementMoved.Execute(element, this);
                                //}
                            }
                        }

                        this._origCursorLocation = cursorLocation;
                    }
                    this.InvalidateMeasure();
                }

                if (this._isSelecting == true)
                {
                    this._selectionAdorner.Update(e.GetPosition(this));
                }
            }
        }

        /// <summary>
        /// Handle mouse up to handle 'releasing' a ViewModel or to finish a selection rectangle
        /// </summary>
        void WorldSurface_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bool wasMoving = this._isMoving;

            if (this._isDragging)
            {
                foreach (ViewModelElement element in this.ViewContext.SelectedViewModels)
                {
                    element.OnMoved(new EventArgs());
                }
            }

            this._isDragging = false;

            //if the selected element was a source endpoint, deselect it upon mouse up
            //if (this.ViewContext.SelectedViewModel is SourceEndpointViewModel)
            //{
            //    SourceEndpointViewModel sourceEndpoint = this.ViewContext.SelectedViewModel as SourceEndpointViewModel;

            //    //if there was a source-anchor intersection, apply the attach operation,
            //    //then clear the intersection flag and the temp field
            //    if (this._previouslyIntersectedAnchor != null)
            //    {
            //        if (sourceEndpoint.IsAttached == false)
            //        {
            //            sourceEndpoint.Attach(this._previouslyIntersectedAnchor);
            //        }
            //        else //just make sure the source endpoint is snapped to anchor position since user may have moved source a bit
            //        {
            //            sourceEndpoint.SnapToAnchorPosition();
            //        }

            //        this._previouslyIntersectedAnchor.IsDragIntersecting = false;
            //        this._previouslyIntersectedAnchor = null;
            //    }
            //    else //otherwise the source endpoint is not intersecting an anchor so make sure it is detached
            //    {
            //        if (sourceEndpoint.IsAttached == true)
            //        {
            //            //only detach if the connector was moved
            //            if (wasMoving == true)
            //            {
            //                sourceEndpoint.Detach();
            //            }
            //        }
            //    }

            //    this.ViewContext.SelectedViewModel.IsSelected = false;
            //    this.ViewContext.SelectedViewModel = null;
            //}

            // if we were selecting ViewModels in a selection rectangle, perform the selection and clear the selection adorner
            if (this._isSelecting == true)
            {
                this._selectionAdorner.Update(e.GetPosition(this));
                SelectViews(this._selectionAdorner.SelectionBounds);
                AdornerLayer.GetAdornerLayer(this).Remove(this._selectionAdorner);
                this._selectionAdorner = null;
                this._isSelecting = false;
            }
            else
            {
                this._isMoving = false;
                this._isDragging = false;
            }
        }

        /// <summary>
        /// Handle right mouse button up to show a properties container for an item
        /// </summary>
        //void ViewCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    //ignore even if left mouse button is pressed
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        return;
        //    }

        //    if (_canEditLayout == true && this.ViewContext.IsEditingLayout)
        //    {
        //    }
        //}

        /// <summary>
        /// Closes the properties container adorner
        /// </summary>
        public void CloseElementPropertiesAdorner()
        {
            if (this._elementPropertiesContainerAdorner != null)
            {
                AdornerLayer.GetAdornerLayer(this).Remove(this._elementPropertiesContainerAdorner);
                this._elementPropertiesContainerAdorner = null;
                //if (this._elementPropertiesContainerViewModel != null)
                //{
                //    this._elementPropertiesContainerViewModel.Close();
                //    this._elementPropertiesContainerViewModel = null;
                //}
            }
        }

        /// <summary>
        /// Gets a list of all ViewModels intersecting the provided ViewModel
        /// </summary>
        private List<ViewModelElement> GetIntersectingViewModels(ViewModelElement element)
        {
            Rect rect = element.Bounds;
            List<ViewModelElement> collection = new List<ViewModelElement>();
            foreach (UIElement child in this.Children)
            {
                if (child is ContentPresenter)
                {
                    ViewModelElement viewModelElement = ((ContentPresenter)child).Content as ViewModelElement;
                    if (viewModelElement != null)
                    {

                        if (viewModelElement.Bounds.IntersectsWith(rect) == true && viewModelElement != element)
                        {

                            //if (viewModelElement.DirectionRotation != 0)
                            //{
                            //    rect = new Rect(rect.X, rect.Y, rect.Height, rect.Width);

                            //    if (viewModelElement.Bounds.IntersectsWith(rect) == true && viewModelElement != element)
                            //    {
                            //        collection.Add(viewModelElement);
                            //    }
                            //}
                            //else
                            //{
                            collection.Add(viewModelElement);
                            // }
                        }
                    }
                }
            }
            return collection;
        }

        //private List<ViewModelElement> GetAttachedViewModels(ViewModelElement element)
       // {
            //List<ViewModelElement> collection = new List<ViewModelElement>();

            //foreach (UIElement child in this.Children)
            //{
            //    if (child is ContentPresenter)
            //    {
            //        ViewModelElement viewModelElement = ((ContentPresenter)child).Content as ViewModelElement;
            //        if (viewModelElement != null && viewModelElement.Configuration != null)
            //        {
            //            if (viewModelElement.Configuration.RefersToDevice(element.Configuration.Id) != "")
            //            {
            //                collection.Add(viewModelElement);
            //            }
            //        }
            //    }
            //}

            //return collection;
        //}

        /// <summary>
        /// Gets a list of all ViewModels intersecting the provided rectangle
        /// </summary>
        private T GetIntersectingViewModel<T>(Rect rect)
            where T : ViewModelElement
        {
            foreach (UIElement child in this.Children)
            {
                if (child is ContentPresenter)
                {
                    T viewModelElement = ((ContentPresenter)child).Content as T;
                    if (viewModelElement != null)
                    {
                        if (viewModelElement.Bounds.IntersectsWith(rect) == true)
                        {
                            return viewModelElement;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Selects all ViewModels within the provided selection rectangle
        /// </summary>
        private void SelectViews(Rect selectionRectangle)
        {
            foreach (UIElement child in this.Children)
            {
                if (child is ContentPresenter)
                {
                    ViewModelElement viewModelElement = ((ContentPresenter)child).Content as ViewModelElement;
                    if (viewModelElement != null)
                    {
                        if (viewModelElement.Bounds.IntersectsWith(selectionRectangle) == true && viewModelElement.IsSelectable == true)
                        {
                            this.ViewContext.SelectedViewModels.Add(viewModelElement);
                        }
                    }
                }
            }
            if (this.ViewContext.SelectedViewModels.Count > 0)
            {
                this.ViewContext.SelectedViewModel = this.ViewContext.SelectedViewModels[0];

                //if there are more than 1 vm's selected, or the selected is not a source endpoint, 
                //then we should remove all source endpoints from the selection
                if (!(this.ViewContext.SelectedViewModels.Count == 1))// || !(this.ViewContext.SelectedViewModel is SourceEndpointViewModel))
                {
                    //remove all source endpoints
                    //this.ViewContext.SelectedViewModels.RemoveAll(vm => vm is SourceEndpointViewModel);

                    //if all selections were just source endpoints, do not do any 'selection'
                    if (this.ViewContext.SelectedViewModels.Count == 0)
                    {
                        this.ViewContext.SelectedViewModel = null;
                    }
                }

                //now make all selection items' IsSelected flag TRUE
                foreach (ViewModelElement viewModelElement in this.ViewContext.SelectedViewModels)
                {
                    viewModelElement.IsSelected = true;
                }
            }
            else //otherwise nothing is selected
            {
                this.ViewContext.SelectedViewModel = null;
            }
        }

        /// <summary>
        /// Walks up the visual tree starting with the specified DependencyObject, 
        /// looking for a UIElement which is a child of the Canvas.  If a suitable 
        /// element is not found, null is returned.  If the 'depObj' object is a 
        /// UIElement in the Canvas's Children collection, it will be returned.
        /// </summary>
        /// <param name="depObj">
        /// A DependencyObject from which the search begins.
        /// </param>
        public ElementViewBase FindCanvasView(DependencyObject depObj)
        {
            while (depObj != null)
            {
                // If the current object is a UIElement which is a child of the
                // Canvas, exit the loop and return it.
                ElementViewBase viewBase = depObj as ElementViewBase;
                if (viewBase != null && (this.Children.Contains((UIElement)VisualTreeHelper.GetParent(viewBase)))) // || VisualTreeHelper.GetParent(depObj) is AdornerDecorator
                    break;

                // VisualTreeHelper works with objects of type Visual or Visual3D.
                // If the current object is not derived from Visual or Visual3D,
                // then use the LogicalTreeHelper to find the parent element.
                if (depObj is Visual || depObj is Visual3D)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else
                    depObj = LogicalTreeHelper.GetParent(depObj);
            }
            return depObj as ElementViewBase;
        }

        /// <summary>
        /// Override the WPF Measure method for measuring the dimensions of the canvas (using the children dimensions)
        /// </summary>
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    Size size = new Size(0, 0);

        //    foreach (UIElement child in this.Children)
        //    {
        //        child.Measure(constraint);
        //        if (child is ContentPresenter)
        //        {
        //            ViewModelElement viewModelElement = ((ContentPresenter)child).Content as ViewModelElement;
        //            if (viewModelElement != null && viewModelElement.IsIncludedInParentMeasure == true)
        //            {
        //                double addHeight = 0;

        //                //make room for debug info (which is on a canvas so it isn't included in measurements)
        //                if (viewModelElement.IsDebugInfoVisible == true)
        //                {
        //                    addHeight = 70;
        //                }


        //                //a messy hack for CameraPane where not having a maximized camera tile causes the actual width/height to be NaN 
        //                //due to its width and height being set to stretch instead of an actual number
        //                double widthVal2;
        //                double heightVal2;

        //                if (Double.IsNaN(viewModelElement.Right))
        //                {
        //                    widthVal2 = 0;
        //                }
        //                else
        //                {
        //                    widthVal2 = viewModelElement.Right;
        //                }

        //                if (Double.IsNaN(viewModelElement.Bottom))
        //                {
        //                    heightVal2 = 0;
        //                }
        //                else
        //                {
        //                    heightVal2 = viewModelElement.Bottom;
        //                }

        //                size.Width = Math.Max(size.Width, widthVal2);
        //                size.Height = Math.Max(size.Height, heightVal2 + addHeight);
        //            }
        //        }
        //    }

        //    size.Width += 20;
        //    size.Height += 20;

        //    // Give the ViewContext the current ViewCanvas size so it can calculate the proper zoom size on a subsequent ArrangeOverride pass
        //    if (this.ViewContext != null)
        //    {
        //        this.ViewContext.ViewCanvasMeasuredSize = size;
        //    }

        //    return size;
        //}

        /// <summary>
        /// Override the WPF Arrange method so the ViewContext can do a zoom operation
        /// </summary>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            // Call a method on the ViewContext so it can calculate the idea zoom size (only if a zoom was requested)
            if (this.ViewContext != null)
            {
                this.ViewContext.OnViewCanvasArrangedSize(arrangeSize);
            }

            Size size = base.ArrangeOverride(arrangeSize);
            return size;
        }


        #region INotifyPropertyChanged Members

        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
