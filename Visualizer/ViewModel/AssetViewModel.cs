using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Controls;
using Visualizer.Util;

namespace Visualizer.ViewModel
{
    public class AssetViewModel : ViewModelBase
    {
        public event EventHandler<GenericEventArgs<AssetViewModel>> AssetDeleted;

        public DelegateCommand DeleteAssetCommand { get; set; }

        private string _assetId;
        private string _name;
        private string _deptName;
        private double _locationX;
        private double _locationY;

        private string _batteryStatus;

        private bool _isSelected;
        private bool _isAlarmEnabled;
        private bool _isMachine;
        private bool _isEmployee;
        private bool _isYellowStatus;
        private bool _isGreenStatus;
        private bool _isDoubleTap;
        private bool _isMissing;
        private bool _isButtonEvent;

        private string _microZoneName;
        private string _microZoneCurrent;
        private string _microZonePrevious;
        private DateTime _enteredMicroZoneTime;
        private DateTime _lastUpdatedOn;

        private string _maintenanceNotes;
        private string _maintenanceLastDate;
        private string _maintenanceSigner;

        public AssetViewModel()
        {
            this.DeleteAssetCommand = new DelegateCommand(DeleteAsset);
        }

        public void UpdateBindings()
        {
            OnPropertyChanged(() => IsReportedByGatewayWithoutPreviousMicroZone);
            OnPropertyChanged(() => IsStationary);
            OnPropertyChanged(() => IsInMotion);
        }

        public string AssetId
        {
            get
            {
                return this._assetId;
            }
            set
            {
                this._assetId = value;
                OnPropertyChanged(() => AssetId);
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
                OnPropertyChanged(() => Name);
            }
        }

        public string DeptName
        {
            get
            {
                return this._deptName;
            }
            set
            {
                this._deptName = value;
                OnPropertyChanged(() => DeptName);
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
                OnPropertyChanged(() => LocationX);
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
                OnPropertyChanged(() => LocationY);
            }
        }

        public string MicroZoneName
        {
            get
            {
                return this._microZoneName;
            }
            set
            {
                this._microZoneName = value;
                OnPropertyChanged(MicroZoneName);
            }
        }

        public string MicroZoneCurrent
        {
            get
            {
                return this._microZoneCurrent;
            }
            set
            {
                this._microZoneCurrent = value;
                OnPropertyChanged(MicroZoneCurrent);
            }
        }

        public string MicroZonePrevious
        {
            get
            {
                return this._microZonePrevious;
            }
            set
            {
                this._microZonePrevious = value;
                OnPropertyChanged(MicroZonePrevious);
            }
        }

        public string IdleTime
        {
            get
            {
                TimeSpan span = new TimeSpan();

                if (this.EnteredMicroZoneTime != DateTime.MinValue)
                {

                    span = (DateTime.Now - this.EnteredMicroZoneTime);
                }

                return String.Format("{0}d:{1}h:{2}m:{3}s", span.Days, span.Hours, span.Minutes, span.Seconds);
            }
        }

        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }

            set
            {
                this._isSelected = value;
                OnPropertyChanged(() => IsSelected);
            }
        }

        public DateTime EnteredMicroZoneTime
        {
            get
            {
                return this._enteredMicroZoneTime;
            }

            set
            {
                this._enteredMicroZoneTime = value;
                OnPropertyChanged(() => EnteredMicroZoneTime);
            }
        }

        public string BatteryStatus
        {
            get
            {
                try
                {
                    int batteryStatus = 0;
                    if (Int32.TryParse(this._batteryStatus, out batteryStatus) == true)
                    {
                        if (batteryStatus == 0)
                            return "N/A";

                        else if (batteryStatus > 65)
                            return "FULL";

                        else if (batteryStatus >= 40)
                            return "2/3 FULL";

                        else if (batteryStatus >= 15)
                            return "1/3 FULL";

                        else
                            return "EMPTY";
                    }
                }
                catch (Exception e)
                {
                   // Logger.LogException(e, "AssetViewModel BatteryStatus Failure");
                }

                return "N/A";
            }
            set
            {
                this._batteryStatus = value;
                OnPropertyChanged(() => BatteryStatus);
            }
        }

        public string MaintenanceNotes
        {
            get
            {
                return this._maintenanceNotes;
            }

            set
            {
                this._maintenanceNotes = value;
                OnPropertyChanged(() => MaintenanceNotes);
            }
        }

        public string MaintenanceLastDate
        {
            get
            {
                return this._maintenanceLastDate;
            }

            set
            {
                this._maintenanceLastDate = value;
                OnPropertyChanged(() => MaintenanceLastDate);
            }
        }

        public string MaintenanceSigner
        {
            get
            {
                return this._maintenanceSigner;
            }
            set
            {
                this._maintenanceSigner = value;
                OnPropertyChanged(() => MaintenanceSigner);
            }
        }

        public bool IsAlarmEnabled
        {
            get
            {
                return _isAlarmEnabled;
            }

            set
            {
                _isAlarmEnabled = value;
                OnPropertyChanged(() => IsAlarmEnabled);
            }
        }

        public bool IsEmployee
        {
            get
            {
                return _isEmployee;
            }

            set
            {
                _isEmployee = value;
                OnPropertyChanged(() => IsEmployee);
            }
        }

        public bool IsUnassigned
        {
            get
            {
                if (!IsEmployee && !IsMachine)
                    return true;
                else
                    return false;
            }
        }

        public bool IsReported
        {
            //a microzone with an ID of 000000, also means there is no microzone
            get
            {
                return String.IsNullOrEmpty(this.MicroZoneCurrent) == false || this.MicroZoneCurrent != "000000" || this.MicroZoneCurrent == String.Empty;
            }
        }

        public bool IsCurrentMicroZoneReported
        {
            get
            {
                if (String.IsNullOrEmpty(this.MicroZoneCurrent) == true)
                {
                   // Logger.LogDebug("IsCurrentMicroZoneReported is False");
                    return false;
                }

                if (this.MicroZoneCurrent == "000000")
                {
                    //Logger.LogDebug("IsCurrentMicroZoneReported is False");
                    return false;
                }

                //Logger.LogDebug(String.Format("MicrozoneCurrent: {0}", this.MicroZoneCurrent));// "IsCurrentMicroZoneReported is True");

                return true;
            }
        }

        public bool IsPreviousMicroZoneReported
        {
            get
            {
                if (String.IsNullOrEmpty(this.MicroZonePrevious) == true)
                {
                    return false;
                }

                if (this.MicroZonePrevious == "000000")
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsReportedByGatewayWithoutPreviousMicroZone
        {
            get
            {
                return (this.LastUpdatedOn.AddMinutes(5) <= DateTime.Now) && this.IsCurrentMicroZoneReported == false && this.IsPreviousMicroZoneReported == false;
            }
        }

        public DateTime LastUpdatedOn
        {
            get
            {
                return _lastUpdatedOn;
            }

            set
            {
                _lastUpdatedOn = value;
            }
        }

        public bool IsMachine
        {
            get
            {
                return _isMachine;
            }

            set
            {
                _isMachine = value;
                OnPropertyChanged(() => IsMachine);
            }
        }

        public bool IsStationary
        {
            get
            {
                return this._isYellowStatus;
            }
            set
            {
                this._isYellowStatus = value;
            }
        }

        public bool IsInMotion
        {
            get
            {
                return this._isGreenStatus;
            }
            set
            {
                this._isGreenStatus = value;
            }
        }

        public bool IsDoubleTap
        {
            get
            {
                return this._isDoubleTap;
            }
            set
            {
                this._isDoubleTap = value;
            }
        }

        public bool IsMissing
        {
            get
            {
                return this._isMissing;
            }
            set
            {
                this._isMissing = value;
            }
        }

        public bool IsButtonEvent
        {
            get
            {
                return this._isButtonEvent;
            }
            set
            {
                this._isButtonEvent = value;
            }
        }

        public string SystemSettingsInMotionColor
        {
            get
            {
                //if (SystemSettings.InMotionColor != null && SystemSettings.InMotionColor != String.Empty && SystemSettings.InMotionColor != "")
                //    return SystemSettings.InMotionColor;
                //else
                    return "Green";
            }
        }

        public string SystemSettingsStationaryColor
        {
            get
            {
                //if (SystemSettings.StationaryColor != null && SystemSettings.StationaryColor != String.Empty && SystemSettings.StationaryColor != "")
                //    return SystemSettings.StationaryColor;
                //else
                    return "Yellow";
            }
        }

        public string SystemSettingsMissingColor
        {
            get
            {
                //if (SystemSettings.MissingColor != null && SystemSettings.MissingColor != String.Empty && SystemSettings.MissingColor != "")
                //    return SystemSettings.MissingColor;
                //else
                    return "Red";
            }
        }

        public void DeleteAsset(object action)
        {
            OnAssetDeleted(new GenericEventArgs<AssetViewModel>(this));
        }

        protected void OnAssetDeleted(GenericEventArgs<AssetViewModel> asset)
        {
            if (this.AssetDeleted != null)
            {
                this.AssetDeleted(this, asset);
            }
        }
    }
}
