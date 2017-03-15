using System;

namespace OSPSuite.Core.Domain.Services
{
   public static class ValueComparer
   {
      /// <summary>
      ///    returns true if the values are equals otherwise false
      /// </summary>
      public static bool AreValuesEqual(double value1, double value2)
      {
         return AreValuesEqual(value1, value2, Constants.DOUBLE_RELATIVE_EPSILON);
      }

      /// <summary>
      ///   returns true if the values are equals otherwise false according to the given relative tolerance
      /// </summary>
      public static bool AreValuesEqual(double value1, double value2, double relTol)
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
      ///   returns true if the values of the given parameters are equals otherwise false
      /// </summary>
      public static bool AreValuesEqual(IParameter parameter1, IParameter parameter2)
      {
         return AreValuesEqual(parameter1.Value, parameter2.Value);
      }

      /// <summary>
      ///   returns true if the percentile of the given parameters are equals otherwise false
      /// </summary>
      public static bool ArePercentilesEqual(IDistributedParameter parameter1, IDistributedParameter parameter2)
      {
         return ArePercentilesEqual(parameter1.Percentile, parameter2.Percentile);
      }

      public static bool ArePercentilesEqual(double percentile1, double percentile2)
      {
         return AreValuesEqual(percentile1, percentile2, 1e-2);
      }

      public static bool AreValuesEqual(float value1, float value2)
      {
         return AreValuesEqual(value1, value2, Constants.FLOAT_RELATIVE_EPSILON);
      }

      public static bool AreValuesEqual(float value1, float value2, float relTol)
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