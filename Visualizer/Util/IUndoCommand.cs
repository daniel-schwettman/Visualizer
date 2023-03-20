using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Visualizer.Util
{
    public interface IUndoCommand : ICommand
    {
        void Undo();
        List<ViewModelElement> GetViewModelElements();


        //void Execute(List<object> list);
    }
}
