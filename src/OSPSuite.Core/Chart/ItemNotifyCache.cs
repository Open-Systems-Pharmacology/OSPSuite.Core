using System;
using System.ComponentModel;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Chart
{
   public interface IItemNotifyCache<TKey, TValue> : INotifyCache<TKey, TValue> where TValue : INotifier
   {
      event EventHandler<ItemChangedEventArgs> ItemPropertyChanged;
      event EventHandler<ItemChangedEventArgs> ItemChanged;
   }

   public class ItemChangedEventArgs : EventArgs
   {
      public object Item { get; private set; }
      public string PropertyName { get; private set; }

      public ItemChangedEventArgs(object item, string propertyName)
      {
         Item = item;
         PropertyName = propertyName;
      }
   }

   public class ItemNotifyCache<TKey, TValue> : NotifyCache<TKey, TValue>, IItemNotifyCache<TKey, TValue> where TValue : INotifier
   {
      public event EventHandler<ItemChangedEventArgs> ItemPropertyChanged = delegate { };
      public event EventHandler<ItemChangedEventArgs> ItemChanged = delegate { };

      public ItemNotifyCache(Func<TValue, TKey> getKey) : base(getKey)
      {
      }

      public override void Add(TKey key, TValue value)
      {
         base.Add(key, value);
         value.PropertyChanged += onPropertyChanged;
         value.Changed += onChanged;
      }

      public override void Remove(TKey key)
      {
         var objectToRemove = base[key];
         objectToRemove.PropertyChanged -= onPropertyChanged;
         objectToRemove.Changed -= onChanged;
         base.Remove(key);
      }

      public override void Clear()
      {
         foreach (var value in this)
         {
            value.PropertyChanged -= onPropertyChanged;
            value.Changed -= onChanged;
         }
         base.Clear();
      }

      private void onPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         ItemPropertyChanged(this, new ItemChangedEventArgs(sender, e.PropertyName));
      }

      private void onChanged(object sender)
      {
         ItemChanged(this, new ItemChangedEventArgs(sender, null));
      }
   }
}