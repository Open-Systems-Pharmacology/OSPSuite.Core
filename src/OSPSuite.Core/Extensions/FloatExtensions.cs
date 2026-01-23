using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Extensions
{
   public static class FloatExtensions
   {
      /// <summary>
      /// Returns an ordered array of floats from which invalid values where removed
      /// </summary>
      /// <param name="floats">Floats to order and purify</param>
      /// <returns></returns>
      public static float[] OrderedAndPurified(this IEnumerable<float> floats)
      {
         return floats.Where(f => f.IsFinite()).OrderBy(x => x).ToArray();
      }

      /// <summary>
      /// Returns true if <paramref name="f"/> is not NaN, PositiveInfinity or NegativeInfinity
      /// </summary>
      public static bool IsFinite(this float f)
      {
         return !float.IsNaN(f) && !float.IsInfinity(f);
      }
   }
}