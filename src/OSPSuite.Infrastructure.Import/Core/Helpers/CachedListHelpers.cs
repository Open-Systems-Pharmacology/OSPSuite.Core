using OSPSuite.Utility.Collections;
using System.Collections.Generic;

namespace OSPSuite.Infrastructure.Import.Core.Helpers
{
   static public class CachedListHelpers
   {
      public static void Add<T1, T2>(Cache<T1, List<T2>> cache, T1 key, T2 x)
      {
         if (cache.Contains(key))
            cache[key].Add(x);
         else
            cache.Add(key, new List<T2>() { x });
      }

      public static void Add<T1, T2>(Cache<T1, List<T2>> cache, T1 key, IEnumerable<T2> list)
      {
         if (cache.Contains(key))
            cache[key].AddRange(list);
         else
            cache.Add(key, new List<T2>(list));
      }
   }
}
