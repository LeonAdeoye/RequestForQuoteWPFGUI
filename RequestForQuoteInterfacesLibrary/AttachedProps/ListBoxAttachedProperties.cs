using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace RequestForQuoteInterfacesLibrary.AttachedProps
{
    /// <summary>
    /// This class contains a few useful extenders for the ListBox
    /// </summary>
    public class ListBoxAttachedProperties : DependencyObject
    {
        public static readonly DependencyProperty AutoScrollToEndProperty = DependencyProperty.RegisterAttached("AutoScrollToEnd", typeof(bool), typeof(ListBoxAttachedProperties), new UIPropertyMetadata(default(bool), OnAutoScrollToEndChanged));

        /// <summary>
        /// Returns the value of the AutoScrollToEndProperty
        /// </summary>
        /// <param name="obj">The dependency-object whichs value should be returned</param>
        /// <returns>The value of the given property</returns>
        public static bool GetAutoScrollToEnd(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToEndProperty);
        }

        /// <summary>
        /// Sets the value of the AutoScrollToEndProperty
        /// </summary>
        /// <param name="obj">The dependency-object whichs value should be set</param>
        /// <param name="value">The value which should be assigned to the AutoScrollToEndProperty</param>
        public static void SetAutoScrollToEnd(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToEndProperty, value);
        }

        /// <summary>
        /// This method will be called when the AutoScrollToEnd
        /// property was changed
        /// </summary>
        /// <param name="sender">The sender (the ListBox)</param>
        /// <param name="eventArgs">Some additional information</param>
        public static void OnAutoScrollToEndChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            var listBox = sender as ListBox;
            if (listBox != null)
            {
                var listBoxItems = listBox.Items;
                var collection = listBoxItems.SourceCollection as INotifyCollectionChanged;
                if (collection != null)
                {

                    var scrollToEndHandler = new NotifyCollectionChangedEventHandler(
                        (s1, e1) =>
                        {
                            if (listBox.Items.Count > 0)
                            {
                                object lastItem = listBox.Items[listBox.Items.Count - 1];
                                listBox.ScrollIntoView(lastItem);
                            }
                        });

                    if ((bool)eventArgs.NewValue)
                        collection.CollectionChanged += scrollToEndHandler;
                    else
                        collection.CollectionChanged -= scrollToEndHandler;                    
                }                
            }
        }
    }
}
