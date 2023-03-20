using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    [System.Reflection.Obfuscation(Exclude = true)]
    public class ViewContext : INotifyPropertyChanged
    {
        public event EventHandler SelectionChanged;

        private bool _allowMove;
        private bool _isJobRunning;
        private double _zoomScale;
        private bool _zoomAfterArrage;
        private ViewModelElement _selectedViewModel;
        private List<ViewModelElement> _selectedViewModels;
        private bool _isEditingLayout;

        /// <summary>
        /// A helper for for this ViewContext so that it knows the size of its 
        /// parent ViewCanvas so that it can calculate the proper zoom scale
        /// </summary>
        public Size ViewCanvasMeasuredSize { get; set; }

        public ViewContext()
        {
            this._selectedViewModels = new List<ViewModelElement>();
            this._selectedViewModel = null;
            this._allowMove = true;
            this._isJobRunning = false;
            this._zoomScale = 1d;
            this._isEditingLayout = true;
        }

        /// <summary>
        /// Deselects all ViewModels
        /// </summary>
        public void DeselectAll()
        {
            foreach (ViewModelElement element in this.SelectedViewModels)
            {
                if (element.IsSelected == true)
                {
                    element.IsSelected = false;
                }
            }
            this.SelectedViewModels.Clear();
            this.SelectedViewModel = null;
        }

        /// <summary>
        /// Asynchronously requests a zoom operation
        /// </summary>
        public void ZoomToFit()
        {
            // set a flag such that when OnViewCanvasArrangedSize is called, the zoom level can be calculated
            this._zoomAfterArrage = true;

            // Force a binding update (thus will force an ArrangeOverride pass) by using a scale not perfectly 1
            this.ZoomScale = 1.0000001d;
        }

        /// <summary>
        /// Handles ArrangeOverride for the ViewCanvas so that a proper zoom scale can be calculated
        /// </summary>
        /// <param name="arrangeSize"></param>
        public void OnViewCanvasArrangedSize(Size arrangeSize)
        {
            // if a zoom calculation is pendning
            if (this._zoomAfterArrage == true)
            {
                // reset flag so we only calculate once (there will be multiple/many ArrangeOverride calls)
                this._zoomAfterArrage = false;

                //add another 20 to width and height
                double zoom = Math.Min(arrangeSize.Width / (this.ViewCanvasMeasuredSize.Width + 20),
                    arrangeSize.Height / (this.ViewCanvasMeasuredSize.Height + 20));

                this.ZoomScale = zoom;
            }
        }

        /// <summary>
        ///  Gets or sets the list of selected ViewModels
        /// </summary>
        public List<ViewModelElement> SelectedViewModels
        {
            get { return _selectedViewModels; }
            set { _selectedViewModels = value; }
        }

        /// <summary>
        /// Gets or sets the selected ViewModel
        /// </summary>
        public ViewModelElement SelectedViewModel
        {
            get
            {
                return this._selectedViewModel;
            }
            set
            {
                //if the selected view model is being deselected, make sure to reset its IsSelected flag
                if (value == null && this._selectedViewModel != null)
                {
                    this._selectedViewModel.IsSelected = false;
                }

                this._selectedViewModel = value;

                if (this._selectedViewModel != null)
                {
                    this._selectedViewModel.IsSelected = true;
                }
                OnPropertyChanged("SelectedViewModel");
                OnSelectionChnged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether ViewModel movement is allowed
        /// </summary>
        public bool AllowMove
        {
            get
            {
                return this._allowMove;
            }
            set
            {
                this._allowMove = value;
                OnPropertyChanged("AllowMove");
            }
        }

        /// <summary>
        /// Gets or sets whether a job is running
        /// </summary>
        public bool IsJobRunning
        {
            get
            {
                return this._isJobRunning;
            }
            set
            {
                this._isJobRunning = value;
                OnPropertyChanged("IsJobRunning");
            }
        }

        /// <summary>
        /// Gets or sets the zoom scale for the view
        /// </summary>
        public double ZoomScale
        {
            get
            {
                return this._zoomScale;
            }
            set
            {
                this._zoomScale = value;
                OnPropertyChanged("ZoomScale");
            }
        }

        public bool IsEditingLayout
        {
            get { return this._isEditingLayout; }
            set { this._isEditingLayout = value; }
        }

        /// <summary>
        /// Gets the number of pixels for 1 meter for grid rendering
        /// </summary>
        public double GridSize
        {
            get
            {
                //1 tick per meter
                return Util.MeasurementUnits.ConvertWorldUnitsToDisplayPixels(1d / 3.2808399d); //1 foot
            }
        }

        protected virtual void OnSelectionChnged(EventArgs e)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, e);
            }
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
