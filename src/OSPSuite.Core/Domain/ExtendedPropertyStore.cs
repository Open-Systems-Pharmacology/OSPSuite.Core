using System;
using System.Linq.Expressions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Domain
{
   public class ExtendedPropertyStore<T> 
   {
      private readonly ExtendedProperties _extendedProperties;

      public ExtendedPropertyStore(ExtendedProperties extendedProperties)
      {
         _extendedProperties = extendedProperties;
      }

      public U Get<U>(Expression<Func<T, U>> exp)
      {
         var key = getKey(exp);
         if (_extendedProperties.Contains(key))
            return _extendedProperties[key].ValueAsObject.ConvertedTo<U>();

         return default(U);
      }

      public ExtendedProperty<U> Property<U>(Expression<Func<T,U>> exp)
      {
         var key = getKey(exp);
         if (_extendedProperties.Contains(key)) return _extendedProperties[key].DowncastTo<ExtendedProperty<U>>();

         var extendedProperty = new ExtendedProperty<U>
         {
            Name = key,
            ValueAsObject = default(U)
         };

         _extendedProperties.Add(extendedProperty);
         return _extendedProperties[key].DowncastTo<ExtendedProperty<U>>();
      }

      public void Set<U>(Expression<Func<T, U>> exp, U value)
      {
         var key = getKey(exp);
         if (!_extendedProperties.Contains(key))
         {
            _extendedProperties.Add(new ExtendedProperty<U> { Name = key });
         }
         _extendedProperties[key].ValueAsObject = value;
      }

      private string getKey<U>(Expression<Func<T, U>> exp)
      {
         return exp.Name();
      }
   }
}