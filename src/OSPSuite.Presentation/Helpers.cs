using System;
using System.Linq.Expressions;
using System.Reflection;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Presentation
{
   public static class Helpers
   {
      public static PropertyInfo Property<T>(Expression<Func<T, object>> property)
      {
         return ReflectionHelper.PropertyFor(property);
      }
   }
}