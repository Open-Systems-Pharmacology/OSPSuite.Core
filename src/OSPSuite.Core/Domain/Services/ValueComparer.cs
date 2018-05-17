using System;

namespace OSPSuite.Core.Domain.Services
{
   public static class ValueComparer
   {
      /// <summary>
      ///    returns <c>true</c> if the values are equals according to the given relative tolerance otherwise <c>false</c>
      /// </summary>
      public static bool AreValuesEqual(double value1, double value2, double relTol = Constants.DOUBLE_RELATIVE_EPSILON)
      {
         //two double NaN are not equal
         if (double.IsNaN(value1))
            return double.IsNaN(value2);

         //relative : 
         if (value1 == 0)
            return value2 == 0;

         var diff = Math.Abs((value1 - value2) / value1);
         return diff <= relTol;
      }

      /// <summary>
      ///    returns <c>true</c> if the values of the given parameters are equals otherwise <c>false</c>
      /// </summary>
      public static bool AreValuesEqual(IParameter parameter1, IParameter parameter2)
      {
         if (parameter1 == null && parameter2 == null)
            return true;

         if (parameter1 == null || parameter2 == null)
            return false;

         return AreValuesEqual(parameter1.Value, parameter2.Value);
      }

      /// <summary>
      ///    returns <c>true</c> if the value of <paramref name="parameter"/> is equal to <paramref name="value"/> otherwise <c>false</c>
      /// </summary>
      public static bool AreValuesEqual(IParameter parameter, double value)
      {
         if (parameter == null)
            return false;

         return AreValuesEqual(parameter.Value, value);
      }

      /// <summary>
      ///    returns <c>true</c> if the percentile of the given parameters are equals otherwise <c>false</c>
      /// </summary>
      public static bool ArePercentilesEqual(IDistributedParameter parameter1, IDistributedParameter parameter2)
      {
         if (parameter1 == null && parameter2 == null)
            return true;

         if (parameter1 == null || parameter2 == null)
            return false;

         return ArePercentilesEqual(parameter1.Percentile, parameter2.Percentile);
      }

      public static bool ArePercentilesEqual(double percentile1, double percentile2)
      {
         return AreValuesEqual(percentile1, percentile2, Constants.DOUBLE_PERCENTILE_RELATIVE_TOLERANCE);
      }

      public static bool AreValuesEqual(float value1, float value2, float relTol = Constants.FLOAT_RELATIVE_EPSILON)
      {
         // two float NaN are not equal! Wonder of .NET
         if (float.IsNaN(value1))
            return float.IsNaN(value2);

         //relative : 
         if (value1 == 0)
            return value2 == 0;

         var diff = Math.Abs((value1 - value2) / value1);
         return diff <= relTol;
      }
   }
}