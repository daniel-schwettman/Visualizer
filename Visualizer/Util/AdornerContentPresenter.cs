using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace Visualizer.Util
{
    //http://www.switchonthecode.com/tutorials/wpf-tutorial-using-a-visual-collection
    public class AdornerContentPresenter : Adorner
    {
        private VisualCollection _visualChildren;
        private ContentPresenter _contentPresenter;

        public AdornerContentPresenter(UIElement adornedElement, object content)
            : base(adornedElement)
        {
            this._contentPresenter = new ContentPresenter();
            this._contentPresenter.Content = content;
            this._visualChildren = new VisualCollection(this);
            this._visualChildren.Add(this._contentPresenter);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return this._contentPresenter.RenderSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this._contentPresenter.Measure(constraint);
            return this._contentPresenter.DesiredSize;
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return this._visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return this._visualChildren[index]; }
    }
}
