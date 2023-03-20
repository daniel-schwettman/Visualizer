using System;
using System.Collections.Generic;
using System.Text;
using Visualizer.Util;

namespace Visualizer.Model
{
    public interface IViewManager
    {
        bool IsDebugViewsVisible { get; set; }
        ViewContext ViewContext { get; }
        ViewModelElementCollection ViewState { get; }

        void AddElement(ViewModelElement viewModel);
        void AddElements(List<ViewModelElement> viewModels);
        void RemoveElement(ViewModelElement viewModel);
        public int ElementCount();

    }
}
