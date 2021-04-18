using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace AdornerBehavior
{
    /// <summary>
    /// List of adorners. Implements INotifyCollectionChanged.
    /// </summary>
    public sealed class AdornerCollection : IList<FrameworkElement>, IEnumerable<FrameworkElement>, IList, IEnumerable, INotifyCollectionChanged
    {
        private readonly List<FrameworkElement> _internalCollection;
        private readonly FrameworkElement _adornedElement;

        public AdornerCollection(FrameworkElement adornedElement)
        {
            this._internalCollection = new List<FrameworkElement>();
            this._adornedElement = adornedElement;
        }

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this._adornedElement, args);
        }

        #endregion

        #region Properties

        public int Count
        {
            get
            {
                return this._internalCollection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return ((IList)this._internalCollection).IsFixedSize;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return ((ICollection)this._internalCollection).IsSynchronized;
            }
        }

        public object SyncRoot
        {
            get
            {
                return ((ICollection)this._internalCollection).SyncRoot;
            }
        }

        #endregion

        #region IList<FrameworkElement>

        public FrameworkElement this[int index]
        {
            get
            {
                return this._internalCollection[index];
            }

            set
            {
                var previous = this._internalCollection[index];
                this._internalCollection[index] = value;
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, previous, index));
            }
        }

        public int IndexOf(FrameworkElement item)
        {
            return this.IndexOf((object)item);
        }

        public void Insert(int index, FrameworkElement item)
        {
            this.Insert(index, (object)item);
        }

        public void Add(FrameworkElement item)
        {
            this.Add((object)item);
        }

        public bool Contains(FrameworkElement item)
        {
            return this.Contains((object)item);
        }

        public void CopyTo(FrameworkElement[] array, int arrayIndex)
        {
            this._internalCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(FrameworkElement item)
        {
            if (!this.Contains(item))
                return false;

            this.Remove(item);
            return true;
        }

        public IEnumerator<FrameworkElement> GetEnumerator()
        {
            foreach (var fe in this._internalCollection)
                yield return fe;
        }

        #endregion

        #region IList

        object IList.this[int index]
        {
            get
            {
                return ((IList)this._internalCollection)[index];
            }

            set
            {
                var previous = this._internalCollection[index];
                ((IList)this._internalCollection)[index] = value;
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, previous, index));
            }
        }

        public int Add(object value)
        {
            var newIndex = ((IList)this._internalCollection).Add(value);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
            return newIndex;
        }

        public bool Contains(object value)
        {
            return ((IList)this._internalCollection).Contains(value);
        }

        public int IndexOf(object value)
        {
            return ((IList)this._internalCollection).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList)this._internalCollection).Insert(index, value);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index));
        }

        public void Remove(object value)
        {
            ((IList)this._internalCollection).Remove(value);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value));
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)this._internalCollection).CopyTo(array, index);
        }

        public void Clear()
        {
            var removed = this._internalCollection.ToArray();
            this._internalCollection.Clear();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
        }

        public void RemoveAt(int index)
        {
            var removed = this._internalCollection[index];
            this._internalCollection.RemoveAt(index);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed, index));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._internalCollection.GetEnumerator();
        }

        #endregion
    }
}
