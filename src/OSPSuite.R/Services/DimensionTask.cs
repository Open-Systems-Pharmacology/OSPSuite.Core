using System;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.R.Domain.UnitSystem;

namespace OSPSuite.R.Services
{
   public interface IDimensionTask
   {
      IDimension DimensionByName(string dimensionName);

      IDimension DimensionForUnit(string unit);

      // We need all those overloads because rClr does not support nullable types and arrays are converted to single value when the array as only one entry!
      double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit, double molWeight);
      double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit);

      double[] ConvertToUnit(IDimension dimension, string targetUnit, double valueInBaseUnit, double molWeight);
      double[] ConvertToUnit(IDimension dimension, string targetUnit, double valueInBaseUnit);

      double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit, double molWeight);
      double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit);

      double[] ConvertToUnit(string dimensionName, string targetUnit, double valueInBaseUnit, double molWeight);
      double[] ConvertToUnit(string dimensionName, string targetUnit, double valueInBaseUnit);
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

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit)
      {
         return convertToUnit(dimension, targetUnit, null, valuesInBaseUnit);
      }

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double valueInBaseUnit)
      {
         return convertToUnit(dimension, targetUnit,null , valueInBaseUnit);
      }

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit)
      {
         return convertToUnit(DimensionByName(dimensionName), targetUnit, null, valuesInBaseUnit);
      }

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double valueInBaseUnit)
      {
         return convertToUnit(DimensionByName(dimensionName), targetUnit, null, valueInBaseUnit);
      }

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double valueInBaseUnit, double molWeight)
      {
         return convertToUnit(dimension, targetUnit, molWeight, valueInBaseUnit);
      }

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit, double molWeight)
      {
         return convertToUnit(DimensionByName(dimensionName), targetUnit, molWeight, valuesInBaseUnit);
      }

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double valueInBaseUnit, double molWeight)
      {
         return convertToUnit(DimensionByName(dimensionName), targetUnit, molWeight, valueInBaseUnit);
      }

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit, double molWeight)
      {
         return convertToUnit(dimension, targetUnit, molWeight, valuesInBaseUnit);
      }

      private double[] convertToUnit(IDimension dimension, string targetUnit, double? molWeight, params double[] valuesInBaseUnit)
      {
         var converterContext = new DoubleArrayContext(dimension, molWeight);
         var mergedDimension = _dimensionFactory.MergedDimensionFor(converterContext);
         var unit = mergedDimension.Unit(targetUnit);
         return mergedDimension.BaseUnitValuesToUnitValues(unit, valuesInBaseUnit);
      }
   }
}