using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class SelectionAdorner : Adorner
    {
        private Point _start;
        private Point _end;
        private Canvas _rubberBandCanvas;
        private VisualCollection _visualChildren;
        private Rectangle _selectionRectangle;
        private double _canvasWidth;
        private double _canvasHeight;

        public SelectionAdorner(UIElement adornedElement, Point start, double canvasWidth, double canvasHeight)
            : base(adornedElement)
        {
            this._start = start;
            this._end = start;

            this._canvasWidth = canvasWidth;
            this._canvasHeight = canvasHeight;

            this._rubberBandCanvas = new Canvas();
            this._rubberBandCanvas.IsHitTestVisible = false; //allow view canvas to handle mouse events
            this._rubberBandCanvas.Width = this._canvasWidth;
            this._rubberBandCanvas.Height = this._canvasHeight;
            this._rubberBandCanvas.Background = new SolidColorBrush(Colors.Transparent);

            this._visualChildren = new VisualCollection(this);
            this._visualChildren.Add(this._rubberBandCanvas);

            this._selectionRectangle = new Rectangle();
            this._selectionRectangle.Fill = new SolidColorBrush(Color.FromArgb(64, 0, 0, 255));

            UpdateSelectionRectangle();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._rubberBandCanvas.Arrange(new Rect(0, 0, this._canvasWidth, this._canvasHeight));
            return finalSize;
        }

        public void Update(Point end)
        {
            this._end = end;
            UpdateSelectionRectangle();
        }

        private void UpdateSelectionRectangle()
        {
            Rect bounds = this.SelectionBounds;
            this._selectionRectangle.Width = bounds.Width;
            this._selectionRectangle.Height = bounds.Height;
            Canvas.SetTop(this._selectionRectangle, bounds.Top);
            Canvas.SetLeft(this._selectionRectangle, bounds.Left);

            this._rubberBandCanvas.Children.Remove(this._selectionRectangle);
            this._rubberBandCanvas.Children.Add(this._selectionRectangle);
        }

        public Rect SelectionBounds
        {
            get
            {
                return new Rect(this._start, this._end);
            }
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return this._visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return this._visualChildren[index]; }
    }
}
