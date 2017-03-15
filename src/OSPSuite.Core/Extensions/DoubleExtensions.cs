using System;
using System.Globalization;

namespace OSPSuite.Core.Extensions
{
   public static class DoubleExtensions
   {
      public static bool IsValidPercentile(this double percentile)
      {
         return percentile > 0 && percentile < 1;
      }

      /// <summary>
      ///    Converts a float to double. This method ensure that we do not lose precision. This should be used
      ///    for instance when importing user values
      /// </summary>
      public static double ToDouble(this float floatValue)
      {
         //in order to avoid lose of precision in convertion, we convert to string and load the string into a double
         //thus a number like 0.20 would be interpreted correctly
         double value;
         double.TryParse(floatValue.ToString(NumberFormatInfo.InvariantInfo), out value);
         return value;
      }

      public static float ToFloat(this double doubleValue)
      {
         return Convert.ToSingle(doubleValue);
      }

      public static bool IsValid(this double value)
      {
         return !double.IsNaN(value) && !double.IsInfinity(value);
      }

      /// <summary>
      /// compare the two given values and return true if the values are equal within the tolerance 
      /// otherwise false
      /// </summary>
      public static bool EqualsByTolerance(this double value, double equalValue, double tolerance)
      {
         return Math.Abs(value - equalValue) < tolerance;
      }

      /// <summary>
      /// compare the two given values and return true if the values are equal within a predefined tolerance
      /// of 1e-10 otherwise false
      /// </summary>
      public static bool EqualsByTolerance(this double value, double equalValue)
      {
         return EqualsByTolerance(value, equalValue, 1e-10);
      }

      /// <summary>
      /// compare the two given arrays and return true if the values in the arrays are equal within the tolerance 
      /// otherwise false
      /// </summary>
      public static bool EqualsByTolerance(this double[] values, double[] equalValues, double tolerance)
      {
         if (values == null && equalValues == null)
            return true;

         if (equalValues == null ^ values == null)
            return false;

         if (values.GetUpperBound(0) != equalValues.GetUpperBound(0))
            return false;

         for (int i = 0; i <= values.GetUpperBound(0); i++)
         {
            if (!values[i].EqualsByTolerance(equalValues[i], tolerance))
               return false;
         }

         return true;
      }

      /// <summary>
      /// compare the two given values and return true if the values are equal within a predifined tolerance 
      /// of 1e-10 otherwise false
      /// </summary>
      public static bool EqualsByTolerance(this double[] value, double[] equalValue)
      {
         return EqualsByTolerance(value, equalValue, 1e-10);
      }

   }
}