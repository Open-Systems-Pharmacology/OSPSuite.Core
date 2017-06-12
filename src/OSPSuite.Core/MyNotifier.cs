using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core
{
   public abstract class MyNotifier : Notifier
   {
      protected bool SetProperty<T>(ref T backingField, T value, Expression<Func<T>> exp)
      {
         var changed = !EqualityComparer<T>.Default.Equals(backingField, value);

         if (changed)
         {
            backingField = value;
            OnPropertyChanged(exp);
         }

         return changed;
      }
   }
}