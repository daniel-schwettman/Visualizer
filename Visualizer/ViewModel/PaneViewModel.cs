using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Telerik.Windows.Controls.Docking;

namespace Visualizer.ViewModel
{
    public class PaneViewModel : INotifyPropertyChanged
    {
        private string _header;
        private bool _isActive;
        private bool _isHidden;
        private bool _isDocument;
        private DockState _initialPosition;

        public PaneViewModel()
        {
        }

        public PaneViewModel(Type contentType)
        {
            this.ContentType = contentType;
        }

        public string Header
        {
            get
            {
                return this._header;
            }
            set
            {
                if (this._header != value)
                {
                    this._header = value;
                    OnPropertyChanged("Header");
                }
            }
        }

        public DockState InitialPosition
        {
            get
            {
                return this._initialPosition;
            }
            set
            {
                if (this._initialPosition != value)
                {
                    this._initialPosition = value;
                    OnPropertyChanged("InitialPosition");
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return this._isActive;
            }
            set
            {
                if (this._isActive != value)
                {
                    this._isActive = value;
                    OnPropertyChanged("IsActive");
                }
            }
        }

        public bool IsHidden
        {
            get
            {
                return this._isHidden;
            }
            set
            {
                if (this._isHidden != value)
                {
                    this._isHidden = value;
                    OnPropertyChanged("IsHidden");
                }
            }
        }
        public bool IsDocument
        {
            get
            {
                return this._isDocument;
            }
            set
            {
                if (this._isDocument != value)
                {
                    this._isDocument = value;
                    OnPropertyChanged("IsDocument");
                }
            }
        }

        public Type ContentType { get; private set; }

        public string TypeName
        {
            get
            {
                return this.ContentType == null ? string.Empty : this.ContentType.AssemblyQualifiedName;
            }
            set
            {
                this.ContentType = value == null ? null : Type.GetType(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
