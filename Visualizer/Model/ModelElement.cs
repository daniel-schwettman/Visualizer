using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Visualizer.Util;

namespace Visualizer.Model
{
    public abstract class ModelElement// : INotifyPropertyChanged
    {
        //public event EventHandler<OffsetEventArgs> ChangeOffsetXRequested;
        public event EventHandler<OffsetEventArgs> ChangeCenterXRequested;

        private int _referenceId;

        private const double _nearZeroWidth = 0.0001;

        public virtual int ReferenceId
        {
            get { return _referenceId; }
            set { _referenceId = value; }
        }

        private Rect _bounds;

        public ModelElement()
        {
            this._bounds = new Rect(0d, 0d, 1d, 1d);
        }

        ///// <summary>
        ///// Helper method so models can notify viewmodels of a request to change the x offset
        ///// </summary>
        ///// <param name="offsetX"></param>
        //public void RequestChangeOffsetX(double offsetX)
        //{
        //    OnChangeOffsetXRequested(new OffsetEventArgs(offsetX));
        //}

        /// <summary>
        /// Helper method so models can notify viewmodels of a request to change the center x
        /// </summary>
        /// <param name="offsetX"></param>
        public void RequestChangeCenterX(double centerX)
        {
            OnChangeCenterXRequested(new OffsetEventArgs(centerX));
        }

        public IEnumerable<T> GetModelsIntersectingCenter<T>(List<T> models)
            where T : ModelElement
        {
            return models.Where(model =>
            {
                Rect bounds = this.Bounds;
                bounds.Inflate(-(bounds.Width * 0.5d - 0.000001d), 0);
                return bounds.IntersectsWith(model.Bounds);
            });
        }

        public bool GetIsModelIntersectingCenter<T>(List<T> models)
            where T : ModelElement
        {
            return models.Any(model =>
            {
                Rect bounds = this.Bounds;
                bounds.Inflate(-(bounds.Width * 0.5d - 0.000001d), 0);
                return bounds.IntersectsWith(model.Bounds);
            });
        }

        public bool GetIsModelIntersectingCenter<T>(T model)
            where T : ModelElement
        {
            if (model == null)
            {
                return false;
            }
            Rect bounds = this.Bounds;
            bounds.Inflate(-(bounds.Width * 0.5d - 0.000001d), 0);
            return bounds.IntersectsWith(model.Bounds);
        }

        public bool GetIsModelIntersectingCenter<T>(T model, double offsetFromCenter)
            where T : ModelElement
        {
            if (model == null)
            {
                return false;
            }

            Rect bounds = this.Bounds;

            double centerX = bounds.Left + (bounds.Width / 2);
            double offsetDistanceFromCenter = centerX + offsetFromCenter;

            bounds = new Rect(offsetDistanceFromCenter, bounds.Y, _nearZeroWidth, bounds.Height);
            return bounds.IntersectsWith(model.Bounds);
        }

        public bool GetIsModelIntersectingLeft<T>(T model)
            where T : ModelElement
        {
            if (model == null)
            {
                return false;
            }
            Rect bounds = this.Bounds;
            bounds = new Rect(bounds.X, bounds.Y, 0.00001d, bounds.Height);
            return bounds.IntersectsWith(model.Bounds);
        }

        public bool GetIsModelIntersectingRight<T>(T model)
            where T : ModelElement
        {
            if (model == null)
            {
                return false;
            }
            Rect bounds = this.Bounds;
            bounds = new Rect(bounds.TopRight.X - 0.00001d, bounds.Y, 0.00001d, bounds.Height);
            return bounds.IntersectsWith(model.Bounds);
        }

        public int GetIntersectingModelCount<T>(List<T> models)
            where T : ModelElement
        {
            return models.Count(model => this.Bounds.IntersectsWith(model.Bounds) == true);
        }

        public bool GetIsModelIntersecting<T>(List<T> models)
            where T : ModelElement
        {
            return models.Any(model => this.Bounds.IntersectsWith(model.Bounds) == true);
        }

        //public List<T> GetIntersectingCenterModels<T>(List<T> models)
        //    where T : ModelElement
        //{
        //    if (models == null)
        //    {
        //        return null;
        //    }

        //    List<T> intersectingModels = null;

        //    Rect bounds = this.Bounds;
        //    bounds.Inflate(-(bounds.Width * 0.5d - 0.000001d), 0);

        //    foreach (T modelElement in models)
        //    {
        //        if (bounds.IntersectsWith(modelElement.Bounds) == true)
        //        {
        //            //only create list if needed
        //            if (intersectingModels == null)
        //            {
        //                intersectingModels = new List<T>();
        //            }
        //            intersectingModels.Add(modelElement);
        //        }
        //    }
        //    return intersectingModels;
        //}

        public void GetIntersectingModels<T>(List<T> models, List<T> output, bool fromEachModelCenter)
            where T : ModelElement
        {
            output.Clear();

            if (models == null)
            {
                return;
            }

            //List<T> intersectingModels = null;

            Rect bounds = this.Bounds;

            foreach (T modelElement in models)
            {
                Rect elementBounds = modelElement.Bounds;
                if (fromEachModelCenter == true)
                {
                    elementBounds.Inflate(-(elementBounds.Width * 0.5d - 0.000001d), 0);
                }

                if (bounds.IntersectsWith(elementBounds) == true)
                {
                    //only create list if needed
                    //if (intersectingModels == null)
                    //{
                    //    intersectingModels = new List<T>();
                    //}
                    output.Add(modelElement);
                }
            }
            //return intersectingModels;
        }

        public void GetIntersectingModels<T>(List<T> models, List<T> output)
            where T : ModelElement
        {
            GetIntersectingModels(models, output, false);
        }

        public static T GetIntersectingModel<T>(Point point, List<T> models)
            where T : ModelElement
        {
            if (models == null)
            {
                return null;
            }

            for (int i = 0; i < models.Count; ++i)
            {
                if (models[i].Bounds.Contains(point) == true)
                {
                    return models[i];
                }
            }
            return null;
        }

        //public List<ModelElement> GetIntersectingModels(List<ModelElement> models)
        //{
        //    if (models == null)
        //    {
        //        return null;
        //    }

        //    List<ModelElement> intersectingModels = null;

        //    foreach (ModelElement modelElement in models)
        //    {
        //        if (this.Bounds.IntersectsWith(modelElement.Bounds) == true)
        //        {
        //            //only create list if needed
        //            if (intersectingModels == null)
        //            {
        //                intersectingModels = new List<ModelElement>();
        //            }
        //            intersectingModels.Add(modelElement);
        //        }
        //    }
        //    return intersectingModels;
        //}

        //protected virtual void OnChangeOffsetXRequested(OffsetEventArgs e)
        //{
        //    if (this.ChangeOffsetXRequested != null)
        //    {
        //        this.ChangeOffsetXRequested(this, e);
        //    }
        //}

        protected virtual void OnChangeCenterXRequested(OffsetEventArgs e)
        {
            if (this.ChangeCenterXRequested != null)
            {
                this.ChangeCenterXRequested(this, e);
            }
        }

        /// <summary>
        /// Get the display matrix
        /// </summary>
        public Matrix RenderTransform
        {
            get
            {
                Matrix m = Matrix.Identity;
                m.Translate(this._bounds.X, this._bounds.Y);
                return m;
            }
        }

        public double CenterX
        {
            get
            {
                return this.OffsetX + this.Width * 0.5d;
            }
            set
            {
                this.OffsetX = value - this.Width * 0.5d;
            }
        }
        public double CenterY
        {
            get
            {
                return this.OffsetY + this.Height * 0.5d;
            }
        }

        public Rect Bounds
        {
            get
            {
                return this._bounds;
            }
            set
            {
                this._bounds = value;
            }
        }

        /// <summary>
        /// Gets the width in meters
        /// </summary>
        public double Width
        {
            get
            {
                return this._bounds.Width;
            }
            set
            {
                this._bounds.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return this._bounds.Height;
            }
            set
            {
                this._bounds.Height = value;
            }
        }
        /// <summary>
        /// Left Bounds
        /// </summary>
        public double OffsetX
        {
            get
            {
                return this._bounds.X;
            }
            set
            {
                this._bounds.X = value;
                OnOffsetXChangedOverride();
            }
        }

        public double OffsetY
        {
            get
            {
                return this._bounds.Y;
            }
            set
            {
                this._bounds.Y = value;
                OnOffsetXChangedOverride();
            }
        }

        protected virtual void OnOffsetXChangedOverride()
        {
        }

        protected virtual void OnOffsetYChangedOverride()
        {
        }

        //#region INotifyPropertyChanged Members

        ///// <summary>
        ///// Notify property changed
        ///// </summary>
        ///// <param name="propertyName">Property name</param>
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //#endregion
    }
}
