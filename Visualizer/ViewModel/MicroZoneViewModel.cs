using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Visualizer.Model;
using Visualizer.Util;
using Visualizer.View;

namespace Visualizer.ViewModel
{
    public class MicroZoneViewModel : ViewModelElement
    {
        public event EventHandler<GenericEventArgs<MicroZoneViewModel>> MicroZoneLocked;
        public event EventHandler<GenericEventArgs<MicroZoneViewModel>> MicroZoneDeleted;
        public event EventHandler<GenericEventArgs<MicroZoneViewModel>> MicroZoneSelected;
        public event EventHandler LayoutUpdated;

        public double ImageHeight { get; set; }
        public double ImageWidth { get; set; }

        public ObservableCollection<AssetViewModel> Assets { get; set; }

        public DelegateCommand LockMicroZoneCommand { get; set; }
        public DelegateCommand UnlockMicroZoneCommand { get; set; }
        public DelegateCommand LockUnlockCommand { get; set; }
        public DelegateCommand RenameCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        public bool IsRotateCommandEnabled { get; set; }
        public bool IsLocked { get; set; }
        public string RawId { get; set; }

        private double _scale = 1d;
        private int _cornerRadius;
        private double _rotation;
        private double _centerY;
        private double _centerX;
        private double _originalCenterY;
        private double _originalCenterX;
        private double _locationX;
        private double _locationY;
        private double _width;
        private double _height;
        private double _scaleX;
        private double _scaleY;
        private Brush _displayBackground;
        private Brush _borderBrush;
        private string _name;
        private Brush _displayForeground;
        private string _microZoneId;
        private int _departmentId;
        private bool is_MouseOver;
        private string _lockUnlockText;
        private string lockUnlockButtonBackground;
        private string _tagAssociationNumber;

        public void UpdateScale(double scale)
        {
            this._scale = scale;
            OnPropertyChanged("Scale");

            //InvalidateBounds();
        }

        public double Scale
        {
            get
            {
                return this._scale;
            }
        }

        public double Rotation
        {
            get
            {
                return this._rotation;
            }
            set
            {
                //make sure rotation is always 0 to 360
                double val = value;
                if (val < 0)
                {
                    val += 360d;
                }

                this._rotation = val;
                OnPropertyChanged("Rotation");
            }
        }

        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                OnPropertyChanged("CornerRadius");
            }
        }

        public double CenterX
        {
            get { return this._centerX; }
            set
            {
                this._centerX = value;
                OnPropertyChanged("CenterX");
            }
        }

        public double CenterY
        {
            get { return this._centerY; }
            set
            {
                this._centerY = value;
                OnPropertyChanged("CenterY");
            }
        }

        public double OriginalCenterX
        {
            get { return this._originalCenterX; }
            set
            {
                this._originalCenterX = value;
                OnPropertyChanged("OriginalCenterX");
            }
        }

        public double OriginalCenterY
        {
            get { return this._originalCenterY; }
            set
            {
                this._originalCenterY = value;
                OnPropertyChanged("OriginalCenterX");
            }
        }

        public MicroZoneViewModel(IViewManager vm, double x, double y, double width, double height, bool nope)
            :base(vm, x, y, width, height, nope)
        {
            this.Assets = new ObservableCollection<AssetViewModel>();

            this.LockMicroZoneCommand = new DelegateCommand(LockMicroZone);
            this.UnlockMicroZoneCommand = new DelegateCommand(UnlockMicroZone);
            this.LockUnlockCommand = new DelegateCommand(LockUnlock);
            this.RenameCommand = new DelegateCommand(Rename);
            this.DeleteCommand = new DelegateCommand(Delete);

            this.ScaleX = 1.0;
            this.ScaleY = 1.0;
            this.BorderBrush = Brushes.Black;
            this.DisplayBackground = Brushes.Blue;
            this.DisplayForeground = Brushes.Black;
            this.IsLocked = false;

            this.LockUnlockText = "Lock";
            this.LockUnlockPath = "Green";
        }

        private void Rename(object action)
        {
            RadWindow.Prompt("Name:", this.OnRenameClosed);
        }

        private void Delete(object action)
        {
            RadWindow.Confirm("Delete selected MicroZone?", this.OnDeleteClosed);
        }

        private void OnDeleteClosed(object sender, WindowClosedEventArgs e)
        {
            int result = 0;

            if (e.DialogResult == true)
            {
                OnMicroZoneDeleted(new GenericEventArgs<MicroZoneViewModel>(this));
            }
        }

        private void OnRenameClosed(object sender, WindowClosedEventArgs e)
        {
            int result = 0;

            if (e.DialogResult == true)
            {
                this.Name = e.PromptResult;
                OnLayoutUpdated();
            }
        }

        public void UpdateMicroZoneLayout()
        {
            UpdateMicroZoneProperties();
            OnLayoutUpdated();
        }

        public void UpdateMicroZoneProperties()
        {
            OnPropertyChanged("Assets");
            OnPropertyChanged("BorderBrush");
        }

        private void LockMicroZone(object action)
        {
            this.IsLocked = true;
            OnMicroZoneLocked(new GenericEventArgs<MicroZoneViewModel>(this));
            OnPropertyChanged("LockUnlockPath");
        }

        private void UnlockMicroZone(object action)
        {
            this.IsLocked = false;
            OnMicroZoneLocked(new GenericEventArgs<MicroZoneViewModel>(this));
            OnPropertyChanged("LockUnlockPath");
        }

        private void SelectMicroZone(object action)
        {
            OnMicroZoneSelected(new GenericEventArgs<MicroZoneViewModel>(this));
        }

        private void LockUnlock(object action)
        {
            this.IsLocked = !this.IsLocked;

            if (this.IsLocked)
            {
                this.LockUnlockText = "Unlock";
                this.LockUnlockPath = "Red";
            }
            else
            {
                this.LockUnlockText = "Lock";
                this.LockUnlockPath = "Green";
            }

            OnMicroZoneLocked(new GenericEventArgs<MicroZoneViewModel>(this));
            OnPropertyChanged("LockUnlockPath");
        }

        public string LockUnlockText
        {
            get
            {
                return _lockUnlockText;
            }
            set
            {
                _lockUnlockText = value;
                OnPropertyChanged("LockUnlockText");
            }
        }

        public double LocationX
        {
            get
            {
                return this._locationX;
            }
            set
            {
                this._locationX = value;
                OnPropertyChanged("LocationX");
            }
        }

        public double LocationY
        {
            get
            {
                return this._locationY;
            }
            set
            {
                this._locationY = value;
                OnPropertyChanged("LocationY");
            }
        }

        //public new double Width2
        //{
        //    get
        //    {
        //        return this._width;
        //    }
        //    set
        //    {
        //        this._width = value;
        //        OnPropertyChanged("Width");
        //    }
        //}

        //public new double Height2
        //{
        //    get
        //    {
        //        return this._height;
        //    }
        //    set
        //    {
        //        this._height = value;
        //        OnPropertyChanged("Height");
        //    }
        //}

        public double ScaleX
        {
            get
            {
                return this._scaleX;
            }
            set
            {
                this._scaleX = value;
                OnPropertyChanged("ScaleX");
            }
        }

        public double ScaleY
        {
            get
            {
                return this._scaleY;
            }
            set
            {
                this._scaleY = value;
                OnPropertyChanged("ScaleY");
            }
        }

        public Brush DisplayBackground
        {
            get
            {
                return this._displayBackground;
            }
            set
            {
                this._displayBackground = value;
                OnPropertyChanged("DisplayBackground");
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return this._borderBrush;
            }
            set
            {
                this._borderBrush = value;
                OnPropertyChanged("BorderBrush");
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                OnPropertyChanged("Name");
            }
        }

        public Brush DisplayForeground
        {
            get
            {
                return this._displayForeground;
            }
            set
            {
                this._displayForeground = value;
                OnPropertyChanged("DisplayForeground");
            }
        }

        public string MicroZoneId
        {
            get
            {
                return this._microZoneId;
            }
            set
            {
                this._microZoneId = value;
                OnPropertyChanged("MicroZoneId");
            }
        }

        public bool IsGeneralZone
        {
            get
            {
                return true;
            }
        }

        public int DepartmentId
        {
            get
            {
                return _departmentId;
            }

            set
            {
                _departmentId = value;
            }
        }

        public bool IsMouseOver
        {
            get
            {
                return is_MouseOver;
            }
            set
            {
                is_MouseOver = value;
                OnPropertyChanged("IsMouseOver");
                OnPropertyChanged("LockUnlockPath");
            }
        }

        public string LockUnlockPath
        {
            get
            {
                return this.lockUnlockButtonBackground;
            }
            set
            {
                this.lockUnlockButtonBackground = value;
                OnPropertyChanged("LockUnlockPath");
            }
        }

        public string TagAssociationNumber
        {
            get
            {
                return this._tagAssociationNumber;
            }
            set
            {
                this._tagAssociationNumber = value;
                OnPropertyChanged("TagAssociationNumber");
            }
        }

        protected virtual void OnLayoutUpdated()
        {
            if (this.LayoutUpdated != null)
            {
                this.LayoutUpdated(this, EventArgs.Empty);
            }
        }

        protected virtual void OnMicroZoneDeleted(GenericEventArgs<MicroZoneViewModel> microZone)
        {
            if (this.MicroZoneDeleted != null)
            {
                this.MicroZoneDeleted(this, microZone);
            }
        }

        protected void OnMicroZoneLocked(GenericEventArgs<MicroZoneViewModel> microZone)
        {
            if (this.MicroZoneLocked != null)
            {
                this.MicroZoneLocked(this, microZone);
            }
        }

        protected void OnMicroZoneSelected(GenericEventArgs<MicroZoneViewModel> microZone)
        {
            if (this.MicroZoneSelected != null)
            {
                this.MicroZoneSelected(this, microZone);
            }
        }

        protected override double DisplayPixelsToWorldUnits(double value)
        {
            return value / this._scale;
        }

        protected override double WorldUnitsToDisplayPixels(double value)
        {
            return value * this._scale;
        }
    }
}
