using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Extensions
{
   public static class ArrayExtensions
   {
      /// <summary>
      ///    Initialize the given array with the <paramref name="value" />.
      ///    (each element of the array will be assigned the value)
      /// </summary>
      public static T[] InitializeWith<T>(this T[] array, T value)
      {
         for (int i = 0; i < array.Length; i++)
         {
            array[i] = value;
         }
         return array;
      }

      /// <summary>
      ///    Makes the float array from a double array
      /// </summary>
      /// <param name="doubles">The double array to create a float array from</param>
      /// <returns></returns>
      public static float[] ToFloatArray(this IEnumerable<double> doubles)
      {
         return doubles.Select(d => (float) d).ToArray();
      }

      public static double[] ToDoubleArray(this IEnumerable<float> floats)
      {
         return floats.Select(f => (double) f).ToArray();
      }

      public static bool IsEqual(this IReadOnlyList<float> array1, IReadOnlyList<float> array2, float absoluteTolerance = 1e-5f)
      {
         if (array1 == null || array2 == null)
            return false;

         if (array1.Count != array2.Count)
            return false;

         for (int i = 0; i < array1.Count; i++)
         {
            if (Math.Abs(array1[i] - array2[i]) > absoluteTolerance)
               return false;
         }

         return true;
      }

      public static string ToPathString(this IEnumerable<string> pathArray)
      {
         return pathArray.ToString(ObjectPath.PATH_DELIMITER);
      }
   }
}