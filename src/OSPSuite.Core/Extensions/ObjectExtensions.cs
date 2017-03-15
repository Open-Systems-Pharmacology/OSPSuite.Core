using System.Linq;

namespace OSPSuite.Core.Extensions
{
   public static class ObjectExtensions
   {
      public static bool IsOneOf<T>(this T objectToCheck, params T[] items)
      {
         if (items == null || !items.Any())
            return false;

         return items.Any(item => Equals(item, (objectToCheck)));
      }
   }
}