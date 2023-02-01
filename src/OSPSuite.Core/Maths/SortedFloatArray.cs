using System.Collections.Generic;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Maths
{
   public class SortedFloatArray
   {
      private readonly IReadOnlyList<float> _sortedArray;

      /// <summary>
      /// Takes a list of floats which will be used to preform quantile operations
      /// </summary>
      /// <param name="floatArray">The list of floats used to calculate quantiles</param>
      /// <param name="alreadySorted">If the list is already sorted, then specify <paramref name="alreadySorted"/> as false
      /// If the list needs to be sorted, specify true</param>
      public SortedFloatArray(IReadOnlyList<float> floatArray, bool alreadySorted)
      {
         _sortedArray = floatArray;
         if (!alreadySorted)
            _sortedArray = _sortedArray.OrderedAndPurified();
      }

      public float Percentile(double percentile)
      {
         return Quantile(percentile / 100);
      }

      public float Median()
      {
         return Percentile(50);
      }

      public float Quantile(double quantile)
      {
         int N = _sortedArray.Count;

         if (N == 0)
            return float.NaN;

         double n = (N - 1) * quantile + 1;

         if (n == 1d)
            return _sortedArray[0];

         if (n == N)
            return _sortedArray[N - 1];

         int k = (int)n;
         var d = n - k;
         double value = _sortedArray[k - 1] + d * (_sortedArray[k] - _sortedArray[k - 1]);
         return (float)value;
      }
   }
}
