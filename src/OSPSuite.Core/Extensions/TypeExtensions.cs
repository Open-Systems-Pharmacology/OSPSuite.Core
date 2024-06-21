using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Extensions
{
   public static class TypeExtensions
   {
      public static bool IsAnImplementationOfAny<T>(this T obj, IEnumerable<Type> typeList)
      {
         return obj.GetType().IsAnImplementationOfAny(typeList);
      }

      public static bool IsAnImplementationOfAny(this Type type, IEnumerable<Type> typeList)
      {
         return typeList.Any(type.IsAnImplementationOf);
      }
   }
}