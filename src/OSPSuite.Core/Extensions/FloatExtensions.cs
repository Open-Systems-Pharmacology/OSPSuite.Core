using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Extensions
{
   public static class FloatExtensions
   {
      public static bool IsValid(this float value)
      {
         return !float.IsNaN(value) && !float.IsInfinity(value);
      }

      /// <summary>
      /// Returns an ordered array of floats from which invalid values where removed
      /// </summary>
      /// <param name="floats">Floats to order and purify</param>
      /// <returns></returns>
      public static float[] OrderedAndPurified(this IEnumerable<float> floats)
      {
         return floats.Where(f => f.IsValid()).OrderBy(x => x).ToArray();
      }
   }
}