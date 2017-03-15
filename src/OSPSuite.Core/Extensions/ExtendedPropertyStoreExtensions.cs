using System;
using System.Linq.Expressions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Extensions
{
   public static class ExtendedPropertyStoreExtensions
   {
      public static void ConfigureProperty<T, U>(this ExtendedPropertyStore<T> propertyStore, Expression<Func<T, U>> propertyNameExpression, string fullName = null, string description = null)
      {
         var extendedProperty = propertyStore.Property(propertyNameExpression);
         
         if(!string.IsNullOrEmpty(fullName))
            extendedProperty.FullName = fullName;

         if (!string.IsNullOrEmpty(description))
            extendedProperty.Description = description;
      }
   }
}
