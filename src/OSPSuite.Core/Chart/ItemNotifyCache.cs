using System;
using System.ComponentModel;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Chart
{
  

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

}