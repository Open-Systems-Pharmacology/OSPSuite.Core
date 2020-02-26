using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.R.Domain.UnitSystem;

namespace OSPSuite.R.Services
{
   public interface IDimensionTask
   {
      IDimension DimensionByName(string dimensionName);

      IDimension DimensionForUnit(string unit);

      double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit);

      double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit, double molWeight);

      double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit);

      double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit, double molWeight);
   }

   public class DimensionTask : IDimensionTask
   {
      private readonly IDimensionFactory _dimensionFactory;

      public DimensionTask(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public IDimension DimensionByName(string dimensionName) => _dimensionFactory.Dimension(dimensionName);

      public IDimension DimensionForUnit(string unit) => _dimensionFactory.DimensionForUnit(unit);

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit, double molWeight)
      {
         return convertToUnit(dimension, targetUnit, valuesInBaseUnit, molWeight);
      }

      private double[] convertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit, double? molWeight = null)
      {
         var converterContext = new DoubleArrayContext(dimension, molWeight);
         var mergedDimension = _dimensionFactory.MergedDimensionFor(converterContext);
         var unit = mergedDimension.Unit(targetUnit);
         return mergedDimension.BaseUnitValuesToUnitValues(unit, valuesInBaseUnit);
      }

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit)
      {
         return convertToUnit(dimension, targetUnit, valuesInBaseUnit);
      }

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit)
      {
         return ConvertToUnit(DimensionByName(dimensionName), targetUnit, valuesInBaseUnit);
      }

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit, double molWeight)
      {
         return ConvertToUnit(DimensionByName(dimensionName), targetUnit, valuesInBaseUnit, molWeight);
      }
   }
}