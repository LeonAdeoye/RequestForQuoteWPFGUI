using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;

namespace RequestForQuoteInterfacesLibrary.Utilities
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool suppressNotification;
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!suppressNotification)
                base.OnCollectionChanged(e);
        }
        protected virtual void OnCollectionChangedMultiItem(NotifyCollectionChangedEventArgs e)
        {
            var handlers = CollectionChanged;

            if (handlers != null)
            {
                foreach (NotifyCollectionChangedEventHandler handler in handlers.GetInvocationList())
                {
                    if (handler.Target is CollectionView)
                        ((CollectionView)handler.Target).Refresh();
                    else
                        handler(this, e);
                }
            }
        }
        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            suppressNotification = true;

            foreach (var item in list)
                Add(item);

            suppressNotification = false;

            NotifyCollectionChangedEventArgs obEvtArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                list as System.Collections.IList);

            OnCollectionChangedMultiItem(obEvtArgs);
        }
    }
}
