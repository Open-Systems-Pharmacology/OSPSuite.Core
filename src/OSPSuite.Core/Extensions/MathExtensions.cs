using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Extensions
{
   public static class MathExtensions
   {

      public static float ArithmeticMean(this IReadOnlyList<float> array)
      {
         var count = array.Count;
         if (count == 0)
            return 0;

         return array.Sum() / count;
      }

      public static float GeometricMean(this IReadOnlyList<float> array)
      {
         var count = array.Count;
         if (count == 0)
            return 0;

         if (AllValuesAreZero(array))
            return 0;

         var logValues = ConvertToLogArray(array);
         return Convert.ToSingle(Math.Exp(ArithmeticMean(logValues)));
      }

      public static bool AllValuesAreZero(this IReadOnlyList<float> array)
      {
         return array.All(t => Equals(t, 0f));
      }

      public static float GeometricStandardDeviation(this IReadOnlyList<float> array)
      {
         var logValues = ConvertToLogArray(array);
         return Convert.ToSingle(Math.Exp(ArithmeticStandardDeviation(logValues)));
      }

      public static float[] AbsoluteValues(this IReadOnlyList<float> array)
      {
         var absValues = new float[array.Count];
         for (int i = 0; i < array.Count; i++)
         {
            absValues[i] = Math.Abs(array[i]);
         }
         return absValues;
      }

      public static float[] ConvertToLogArray(this IReadOnlyList<float> array)
      {
         return convertToLog10Array(array, Math.Log);
      }

      public static float[] ConvertToLog10Array(this IReadOnlyList<float> array)
      {
         return convertToLog10Array(array, Math.Log10);
      }

      private static float[] convertToLog10Array(IReadOnlyList<float> array, Func<double, double> logFunc)
      {
         var logValues = new float[array.Count];
         for (int i = 0; i < array.Count; i++)
         {
            if (array[i] > 0)
               logValues[i] = Convert.ToSingle(logFunc(array[i]));
            else
               logValues[i] = float.NaN;
         }
         return logValues;
      }

      public static float ArithmeticStandardDeviation(this IReadOnlyList<float> array)
      {
         int count = array.Count;
         if (count == 0)
            return 0;

         if (count == 1)
            return array[0];

         float mean = ArithmeticMean(array);
         double sum = 0;
         for (int i = 0; i < count; i++)
         {
            sum += Math.Pow(array[i] - mean, 2);
         }
         sum /= (count - 1);

         return Convert.ToSingle(Math.Pow(sum, 0.5));
      }

      public static float Percentile(this IReadOnlyList<float> sortedArray, double percentile)
      {
         return sortedArray.Quantile(percentile / 100);
      }

      public static float Median(this IReadOnlyList<float> sortedArray)
      {
         return sortedArray.Percentile(50);
      }

      public static float Quantile(this IReadOnlyList<float> sortedArray, double quantile)
      {
         int N = sortedArray.Count;

         if (N == 0)
            return float.NaN;

         double n = (N - 1) * quantile + 1;

         if (n == 1d)
            return sortedArray[0];

         if (n == N)
            return sortedArray[N - 1];

         int k = (int) n;
         var d = n - k;
         double value = sortedArray[k - 1] + d * (sortedArray[k] - sortedArray[k - 1]);
         return (float) value;
      }
   }
}