using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Import
{
   /// <summary>
   ///    Values accepted in a column defining the error type (arithmetic or geometric standard deviation) of a data set
   /// </summary>
   public static class ErrorStdDevValues
   {
      private static readonly IReadOnlyList<string> _arithmeticValues = new[] { "ASD", "ArithmeticStdDev", Constants.STD_DEV_ARITHMETIC };
      private static readonly IReadOnlyList<string> _geometricValues = new[] { "GSD", "GeometricStdDev", Constants.STD_DEV_GEOMETRIC };

      public static IReadOnlyList<string> AcceptedValues { get; } = _arithmeticValues.Concat(_geometricValues).ToList();

      /// <summary>
      ///    Returns <see cref="Constants.STD_DEV_ARITHMETIC" /> or <see cref="Constants.STD_DEV_GEOMETRIC" /> for an accepted
      ///    <paramref name="value" /> (case-insensitive, trimmed) and null otherwise
      /// </summary>
      public static string ErrorStdDevFor(string value)
      {
         var trimmedValue = value?.Trim();
         if (string.IsNullOrEmpty(trimmedValue))
            return null;

         if (_arithmeticValues.Any(x => string.Equals(x, trimmedValue, StringComparison.OrdinalIgnoreCase)))
            return Constants.STD_DEV_ARITHMETIC;

         if (_geometricValues.Any(x => string.Equals(x, trimmedValue, StringComparison.OrdinalIgnoreCase)))
            return Constants.STD_DEV_GEOMETRIC;

         return null;
      }
   }
}
