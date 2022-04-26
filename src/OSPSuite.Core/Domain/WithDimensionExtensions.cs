using System;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class WithDimensionExtensions
   {
      public static double ConvertToUnit(this IWithDimension withDimension, double valueInBaseUnit, Unit unit)
      {
         return withDimension.Dimension?.BaseUnitValueToUnitValue(unit, valueInBaseUnit) ?? valueInBaseUnit;
      }

      public static float ConvertToUnit(this IWithDimension withDimension, float valueInBaseUnit, Unit unit)
      {
         return withDimension.Dimension?.BaseUnitValueToUnitValue(unit, valueInBaseUnit) ?? valueInBaseUnit;
      }

      public static double ConvertToUnit(this IWithDimension withDimension, double valueInBaseUnit, string unit)
      {
         return HasUnit(withDimension, unit) ? ConvertToUnit(withDimension, valueInBaseUnit, withDimension.Dimension.Unit(unit)) : valueInBaseUnit;
      }

      public static float ConvertToUnit(this IWithDimension withDimension, float valueInBaseUnit, string unit)
      {
         return HasUnit(withDimension, unit) ? ConvertToUnit(withDimension, valueInBaseUnit, withDimension.Dimension.Unit(unit)) : valueInBaseUnit;
      }

      public static double ConvertToBaseUnit(this IWithDimension withDimension, double value, Unit unit)
      {
         return withDimension.Dimension?.UnitValueToBaseUnitValue(unit, value) ?? value;
      }

      public static double ConvertToBaseUnit(this IWithDimension withDimension, double value, string unit)
      {
         return HasUnit(withDimension, unit) ? ConvertToBaseUnit(withDimension, value, withDimension.Dimension.Unit(unit)) : value;
      }

      public static float ConvertToBaseUnit(this IWithDimension withDimension, float value, Unit unit)
      {
         return withDimension.Dimension?.UnitValueToBaseUnitValue(unit, value) ?? value;
      }

      public static float ConvertToBaseUnit(this IWithDimension withDimension, float value, string unit)
      {
         return HasUnit(withDimension, unit) ? ConvertToBaseUnit(withDimension, value, withDimension.Dimension.Unit(unit)) : value;
      }

      public static bool IsConcentrationBased(this IWithDimension withDimension) => ReactionDimension(withDimension) == ReactionDimensionMode.ConcentrationBased;

      public static bool IsAmountBased(this IWithDimension withDimension) => ReactionDimension(withDimension) == ReactionDimensionMode.AmountBased;

      public static ReactionDimensionMode ReactionDimension(this IWithDimension withDimension)
      {
         return withDimension.DimensionIsOneOf(Constants.Dimension.MOLAR_CONCENTRATION, Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME) ? ReactionDimensionMode.ConcentrationBased : ReactionDimensionMode.AmountBased;
      }

      public static bool IsAmount(this IWithDimension withDimension) => withDimension.DimensionIsOneOf(Constants.Dimension.MOLAR_AMOUNT, Constants.Dimension.MASS_AMOUNT);

      public static bool IsConcentration(this IWithDimension withDimension) => withDimension.DimensionIsOneOf(Constants.Dimension.MOLAR_CONCENTRATION, Constants.Dimension.MASS_CONCENTRATION);

      public static bool IsFraction(this IWithDimension withDimension) => withDimension.DimensionIsOneOf(Constants.Dimension.FRACTION);

      public static bool DimensionIsOneOf(this IWithDimension withDimension, params string[] dimensionNames) => withDimension.DimensionName().IsOneOf(dimensionNames);

      public static string DimensionName(this IWithDimension withDimension) => withDimension?.Dimension == null ? string.Empty : withDimension.Dimension.Name;

      public static string BaseUnitName(this IWithDimension withDimension) => withDimension?.Dimension?.BaseUnit.Name ?? string.Empty;

      public static bool HasUnit(this IWithDimension withDimension, string unit) => withDimension?.Dimension != null && withDimension.Dimension.HasUnit(unit);

      public static string[] AllUnitNames(this IWithDimension withDimension) => withDimension?.Dimension?.GetUnitNames().ToArray() ?? Array.Empty<string>();

      public static Unit[] AllUnits(this IWithDimension withDimension) => withDimension?.Dimension?.Units.ToArray() ?? Array.Empty<Unit>();
   }
}