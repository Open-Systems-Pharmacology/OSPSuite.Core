using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class WithDimensionAndDisplayUnitExtensions
   {
      public static double ConvertToUnit<T>(this T withDimension, double value, Unit unit) where T : IWithDimension
      {
         if (withDimension.Dimension == null)
            return value;

         return withDimension.Dimension.BaseUnitValueToUnitValue(unit, value);
      }

      public static double ConvertToUnit<T>(this T withDimension, double value, string unit) where T : IWithDimension
      {
         if (withDimension.Dimension == null || !withDimension.Dimension.HasUnit(unit))
            return value;

         return ConvertToUnit(withDimension, value, withDimension.Dimension.Unit(unit));
      }

      public static double ConvertToDisplayUnit<T>(this T quantity, double? valueToConvert) where T : IWithDisplayUnit
      {
         if (canConvert(quantity, valueToConvert))
            return quantity.ConvertToUnit(valueToConvert.Value, quantity.DisplayUnit);

         return double.NaN;
      }

      public static double ConvertToBaseUnit<T>(this T quantity, double? valueToConvert) where T : IWithDisplayUnit
      {
         if (canConvert(quantity, valueToConvert))
            return quantity.Dimension.UnitValueToBaseUnitValue(quantity.DisplayUnit, valueToConvert.Value);

         if (valueToConvert.HasValue)
            return valueToConvert.Value;

         return double.NaN;
      }

      private static bool canConvert<T>(T quantity, double? valueToConvert) where T : IWithDisplayUnit
      {
         return valueToConvert.HasValue && quantity.DisplayUnit != null && quantity.Dimension != null;
      }

      public static IReadOnlyList<double> ConvertToDisplayValues<T>(this T withDisplayUnit, IEnumerable<double> baseValues) where T : IWithDisplayUnit
      {
         return baseValues.Select(v => ConvertToDisplayUnit(withDisplayUnit, v)).ToList();
      }

      public static IReadOnlyList<double> ConvertToBaseValues<T>(this T withDisplayUnit, IEnumerable<double> displayValues) where T : IWithDisplayUnit
      {
         return displayValues.Select(v => ConvertToBaseUnit(withDisplayUnit, v)).ToList();
      }

      public static IReadOnlyList<float> ConvertToDisplayValues<T>(this T withDisplayUnit, IEnumerable<float> baseValues) where T : IWithDisplayUnit
      {
         return baseValues.Select(v => ConvertToDisplayUnit(withDisplayUnit, v)).ToFloatArray();
      }

      public static IReadOnlyList<float> ConvertToBaseValues<T>(this T withDisplayUnit, IEnumerable<float> displayValues) where T : IWithDisplayUnit
      {
         return displayValues.Select(v => ConvertToBaseUnit(withDisplayUnit, v)).ToFloatArray();
      }

      public static bool IsConcentrationBased(this IWithDimension withDimension)
      {
         return ReactionDimension(withDimension) == ReactionDimensionMode.ConcentrationBased;
      }

      public static bool IsAmountBased(this IWithDimension withDimension)
      {
         return ReactionDimension(withDimension) == ReactionDimensionMode.AmountBased;
      }

      public static ReactionDimensionMode ReactionDimension(this IWithDimension withDimension)
      {
         if (withDimension.Dimension.Name.IsOneOf(Constants.Dimension.MOLAR_CONCENTRATION, Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME))
            return ReactionDimensionMode.ConcentrationBased;

         return ReactionDimensionMode.AmountBased;
      }
   }
}