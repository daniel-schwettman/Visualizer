using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Visualizer.Util;

namespace Visualizer.Model
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class ViewModelElementCollection : INotifyPropertyChanged
    {
        private List<ViewModelElement> _topLevelItems;
        private ReadOnlyCollection<ViewModelElement> _readOnlyTopLevelItems;
        private ObservableCollection<ViewModelElement> _items;
        private ReadOnlyObservableCollection<ViewModelElement> _readOnlyItems;

        public ViewModelElementCollection()
        {
            this._topLevelItems = new List<ViewModelElement>();
            this._readOnlyTopLevelItems = new ReadOnlyCollection<ViewModelElement>(this._topLevelItems);

            this._items = new ObservableCollection<ViewModelElement>();
            this._readOnlyItems = new ReadOnlyObservableCollection<ViewModelElement>(this._items);
        }

        public static List<ViewModelElement> FlattenTopLevelItems(IList<ViewModelElement> topLevelItems)
        {
            List<ViewModelElement> flatList = new List<ViewModelElement>();

            foreach (ViewModelElement element in topLevelItems)
            {
                //add children first
                foreach (ViewModelElement child in element.Children)
                {
                    flatList.Add(child);
                }

                flatList.Add(element);
            }

            return flatList;
        }

        public void Add(List<ViewModelElement> elements)
        {
            foreach (ViewModelElement element in elements)
            {
                AddInternal(element);
            }

            //this._items.Sort(item => item.ZOrder, ListSortDirection.Descending);
        }

        public void Add(ViewModelElement element)
        {
            AddInternal(element);
            //if (element.ZOrder > 0)
            //{
            //    this._items.Sort(item => item.ZOrder, ListSortDirection.Descending);
            //}
        }

        private void AddInternal(ViewModelElement element)
        {
            //add children first
            List<ViewModelElement> children = element.Children;
            foreach (ViewModelElement child in children)
            {
                //update the child element's zorder
                child.ZOrder = element.ZOrder;

                this._items.Insert(GetInsertionIndex(child.ZOrder), child);
            }

            this._topLevelItems.Add(element);
            this._items.Insert(GetInsertionIndex(element.ZOrder), element);
        }

        public void Remove(ViewModelElement element)
        {
            IDisposable disposable = element.ModelElement as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            //remove children first
            List<ViewModelElement> children = element.Children;
            foreach (ViewModelElement child in children)
            {
                disposable = child.ModelElement as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                this._items.Remove(child);
            }

            this._topLevelItems.Remove(element);
            this._items.Remove(element);
        }

        private int GetInsertionIndex(int zOrder)
        {
            //0 is topmost, so just add it to the end
            if (zOrder == 0)
            {
                return this._items.Count;
            }
            else //otherwise find the first slot
            {
                for (int i = 0; i < this._items.Count; ++i)
                {
                    //find the first index where the zorder is less (higher zorder) than the passed in zorder
                    if (this._items[i].ZOrder < zOrder)
                    {
                        return i;
                    }
                }

                //if nothing at a higher level is found, just add to the end
                return this._items.Count;
            }
        }

        public ReadOnlyCollection<ViewModelElement> TopLevelItems
        {
            get
            {
                return this._readOnlyTopLevelItems;
            }
        }

        public ReadOnlyObservableCollection<ViewModelElement> Items
        {
            get
            {
                return this._readOnlyItems;
            }
        }

        /// <summary>
        /// Disposes and clears all items
        /// </summary>
        public void Clear()
        {
            foreach (ViewModelElement viewModel in this._readOnlyItems)
            {
                viewModel.Unsubscribe();

                IDisposable disposable = viewModel.ModelElement as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            this._items.Clear();
            this._topLevelItems.Clear();
            OnPropertyChanged("Items");
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
