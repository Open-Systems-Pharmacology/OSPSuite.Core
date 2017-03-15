using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public static class DimensionExtensions
   {
      /// <summary>
      ///    Returns true if the <paramref name="dimension" /> is a time dimension
      /// </summary>
      public static bool IsTime(this IDimension dimension)
      {
         return string.Equals(dimension.Name, Constants.Dimension.TIME);
      }

      public static IEnumerable<string> GetSharedUnitNames(this IDimension first, IDimension second)
      {
         if (first == null || second == null)
            return Enumerable.Empty<string>();

         return first.GetUnitNames().Intersect(second.GetUnitNames());
      }

      public static bool HasSharedUnitNamesWith(this IDimension first, IDimension second)
      {
         if (first == null || second == null) return false;
         return first.GetSharedUnitNames(second).Any();
      }

      public static IEnumerable<Unit> VisibleUnits(this IDimension dimension)
      {
         return dimension.Units.Where(x => x.Visible);
      }

      /// <summary>
      ///    Converts the given values (in base unit) to a representation of the value in the <paramref name="unit" />
      /// </summary>
      /// <param name="dimension">Dimension to use for conversion</param>
      /// <param name="unit">Unit into which the value should be converted</param>
      /// <param name="values">Values in base unit to convert</param>
      public static IReadOnlyList<double> BaseUnitValuesToUnitValues(this IDimension dimension, Unit unit, IEnumerable<double> values)
      {
         return values.Select(value => dimension.BaseUnitValueToUnitValue(unit, value)).ToList();
      }

      /// <summary>
      ///    Converts the given values in <paramref name="unit" /> to values in the base unit
      /// </summary>
      /// <param name="dimension">Dimension to use for conversion</param>
      /// <param name="unit">Unit of the value given as parameter</param>
      /// <param name="values">Value to be converted in base unit </param>
      public static IReadOnlyList<double> UnitValuesToBaseUnitValues(this IDimension dimension, Unit unit, IEnumerable<double> values)
      {
         return values.Select(value => dimension.UnitValueToBaseUnitValue(unit, value)).ToList();
      }

      public static bool IsEquivalentTo(this IDimension dimension, IDimension otherDimension)
      {
         if (Equals(dimension, otherDimension))
            return true;

         var mergedDimension = dimension as IMergedDimension;
         var otherMergedDimensions = otherDimension as IMergedDimension;

         if (mergedDimension == null && otherMergedDimensions == null)
            return Equals(dimension.BaseRepresentation, otherDimension.BaseRepresentation);

         if (otherMergedDimensions != null)
            return areEquivalent(dimension, otherMergedDimensions);

         return areEquivalent(otherDimension, mergedDimension);
      }

      private static bool areEquivalent(IDimension dimension, IMergedDimension mergedDimension)
      {
         return IsEquivalentTo(dimension, mergedDimension.SourceDimension) || mergedDimension.TargetDimensions.Any(x => IsEquivalentTo(dimension, x));
      }
   }
}