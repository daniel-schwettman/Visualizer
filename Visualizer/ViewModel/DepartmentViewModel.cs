using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Controls;

namespace Visualizer.ViewModel
{
    public class DepartmentViewModel : ViewModelBase
    {
        private int _departmentId;
        private string _name;
        private string _filePath;
        private double screenWidth;
        private double screenHeight;

        public DepartmentViewModel()
        {

        }

        public bool IsLastLoaded { get; set; }

        public int DepartmentId
        {
            get
            {
                return _departmentId;
            }

            set
            {
                _departmentId = value;
                OnPropertyChanged(() => DepartmentId);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }

            set
            {
                _filePath = value;
                OnPropertyChanged(() => FilePath);
            }
        }

        public double ScreenWidth
        {
            get { return screenWidth; }
            set 
            { 
                screenWidth = value;
                OnPropertyChanged(() => ScreenWidth);
            }
        }

        public double ScreenHeight
        {
            get { return screenHeight; }
            set
            {
                screenHeight = value;
                OnPropertyChanged(() => ScreenHeight);
            }
        }
    }
}
