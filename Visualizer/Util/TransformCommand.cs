using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Visualizer.Util
{
    public class TransformCommand : IUndoCommand
    {
        public event EventHandler CanExecuteChanged;
        bool modelBeforeSet;
        //List<FormDesignerElementViewModel> modelsTransformedBefore;
        //List<FormDesignerElementViewModel> modelsTransformedAfter;
        bool useBefore = false;
        //Transformation DeltaTransformation = new Transformation();
        public Vector DeltaXY = new Vector();
        public double DeltaRotation;



        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                //if (modelsTransformedBefore == null && modelBeforeSet == false)
                //{

                    //List<ViewModelElement> element = parameter as List<ViewModelElement>;
                    //modelsTransformedBefore = new List<FormDesignerElementViewModel>();
                    //foreach (ViewModelElement e in element)
                    //{
                    //    modelsTransformedBefore.Add((e as FormDesignerElementViewModel).DeepClone());
                    //}
                    //modelBeforeSet = true;

                //}
               // else if (modelBeforeSet == true)
                //{
                    //List<ViewModelElement> element = parameter as List<ViewModelElement>;
                    //modelsTransformedAfter = new List<FormDesignerElementViewModel>();
                    //foreach (ViewModelElement e in element)
                    //{
                    //    modelsTransformedAfter.Add((e as FormDesignerElementViewModel).DeepClone());

                    //}

                    //foreach (FormDesignerElementViewModel e in modelsTransformedAfter)
                    //{
                    //    //Draw the elements onto the formDesigner
                    //    e.InvalidateVisuals();
                    //}

                    //CommandStateManager.Instance.Executed(this);
               // }

           // }
           // else
           // {
            //    useBefore = !useBefore;
            }



        }



        public void Undo()
        {
            useBefore = !useBefore;
        }

        public List<ViewModelElement> GetViewModelElements()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            //string x1 = modelsTransformedBefore[0].OffsetX.ToString();
            //string y1 = modelsTransformedBefore[0].OffsetY.ToString();
            //string x2 = modelsTransformedAfter[0].OffsetX.ToString();
            //string y2 = modelsTransformedAfter[0].OffsetY.ToString();
            //return $"B: {x1},{y1}, A: {x2}, {y2}";
            return "";
        }

        //List<FormDesignerElementViewModel> IUndoCommand.GetDesignerElementViewModels()
        //{
        //    if (useBefore)
        //        return modelsTransformedBefore;
        //    else
        //        return modelsTransformedAfter;
        //}
    }
}
