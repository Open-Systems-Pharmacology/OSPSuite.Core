using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class WithDisplayUnitExtensions
   {
      public static double ConvertToDisplayUnit(this IWithDisplayUnit quantity, double? valueToConvert) 
      {
         if (canConvert(quantity, valueToConvert))
            return quantity.ConvertToUnit(valueToConvert.Value, quantity.DisplayUnit);

         return double.NaN;
      }

      public static double ConvertToBaseUnit(this IWithDisplayUnit quantity, double? valueToConvert)
      {
         if (canConvert(quantity, valueToConvert))
            return quantity.Dimension.UnitValueToBaseUnitValue(quantity.DisplayUnit, valueToConvert.Value);

         if (valueToConvert.HasValue)
            return valueToConvert.Value;

         return double.NaN;
      }

      private static bool canConvert(this IWithDisplayUnit quantity, double? valueToConvert)
      {
         return valueToConvert.HasValue && quantity.DisplayUnit != null && quantity.Dimension != null;
      }

      public static IReadOnlyList<double> ConvertToDisplayValues(this IWithDisplayUnit withDisplayUnit, IEnumerable<double> baseValues)
      {
         return baseValues.Select(v => ConvertToDisplayUnit(withDisplayUnit, v)).ToList();
      }

      public static IReadOnlyList<double> ConvertToBaseValues(this IWithDisplayUnit withDisplayUnit, IEnumerable<double> displayValues)
      {
         return displayValues.Select(v => ConvertToBaseUnit(withDisplayUnit, v)).ToList();
      }

      public static IReadOnlyList<float> ConvertToDisplayValues(this IWithDisplayUnit withDisplayUnit, IEnumerable<float> baseValues)
      {
         return baseValues.Select(v => ConvertToDisplayUnit(withDisplayUnit, v)).ToFloatArray();
      }

      public static IReadOnlyList<float> ConvertToBaseValues(this IWithDisplayUnit withDisplayUnit, IEnumerable<float> displayValues)
      {
         return displayValues.Select(v => ConvertToBaseUnit(withDisplayUnit, v)).ToFloatArray();
      }

      public static string DisplayUnitName(this IWithDisplayUnit withDisplayUnit) => withDisplayUnit?.DisplayUnit?.Name ?? string.Empty;
   }
}