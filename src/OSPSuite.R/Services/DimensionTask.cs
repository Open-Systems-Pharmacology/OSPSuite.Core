using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.R.Domain.UnitSystem;

namespace OSPSuite.R.Services
{
   public interface IDimensionTask
   {
      IDimension DimensionByName(string dimensionName);

      IDimension DimensionForUnit(string unit);

      double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit, double? molWeight = null);

      double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit, double? molWeight = null);
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

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit, double? molWeight = null)
      {
         var converterContext = new DoubleArrayContext(dimension, molWeight);
         var mergedDimension = _dimensionFactory.MergedDimensionFor(converterContext);
         var unit = mergedDimension.Unit(targetUnit);
         return mergedDimension.BaseUnitValuesToUnitValues(unit, valuesInBaseUnit);
      }

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit, double? molWeight = null)
      {
         return ConvertToUnit(DimensionByName(dimensionName), targetUnit, valuesInBaseUnit, molWeight);
      }
   }
}