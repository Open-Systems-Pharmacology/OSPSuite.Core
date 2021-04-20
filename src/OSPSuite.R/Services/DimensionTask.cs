using System;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.R.Domain.UnitSystem;

namespace OSPSuite.R.Services
{
   public interface IDimensionTask
   {
      IDimension DimensionByName(string dimensionName);

      IDimension DimensionForUnit(string unit);

      /// <summary>
      ///    Returns <c>true</c> if a dimension named <paramref name="dimensionName" /> exists otherwise <c>false</c>
      /// </summary>
      bool HasDimension(string dimensionName);

      /// <summary>
      ///    Returns the default dimension for the <paramref name="standardPKParameter" />.
      /// </summary>
      IDimension DimensionForStandardPKParameter(StandardPKParameter standardPKParameter);

      /// <summary>
      ///    Returns the default dimension for the <paramref name="standardPKParameter" />. Note: we use an int because there is
      ///    an issue with signature matching with R
      /// </summary>
      IDimension DimensionForStandardPKParameter(int standardPKParameter);

      // We need all those overloads because rClr does not support nullable types and arrays are converted to single value when the array as only one entry!
      double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit, double molWeight);
      double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit);

      double[] ConvertToUnit(IDimension dimension, string targetUnit, double valueInBaseUnit, double molWeight);
      double[] ConvertToUnit(IDimension dimension, string targetUnit, double valueInBaseUnit);

      double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit, double molWeight);
      double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit);

      double[] ConvertToUnit(string dimensionName, string targetUnit, double valueInBaseUnit, double molWeight);
      double[] ConvertToUnit(string dimensionName, string targetUnit, double valueInBaseUnit);

      // We need all those overloads because rClr does not support nullable types and arrays are converted to single value when the array as only one entry!
      double[] ConvertToBaseUnit(IDimension dimension, string displayUnit, double[] valuesInDisplayUnit, double molWeight);
      double[] ConvertToBaseUnit(IDimension dimension, string displayUnit, double[] valuesInDisplayUnit);

      double[] ConvertToBaseUnit(IDimension dimension, string displayUnit, double valueInDisplayUnit, double molWeight);
      double[] ConvertToBaseUnit(IDimension dimension, string displayUnit, double valueInDisplayUnit);

      double[] ConvertToBaseUnit(string dimensionName, string displayUnit, double[] valuesInDisplayUnit, double molWeight);
      double[] ConvertToBaseUnit(string dimensionName, string displayUnit, double[] valuesInDisplayUnit);

      double[] ConvertToBaseUnit(string dimensionName, string displayUnit, double valueInDisplayUnit, double molWeight);
      double[] ConvertToBaseUnit(string dimensionName, string displayUnit, double valueInDisplayUnit);

      /// <summary>
      ///    Returns an array containing all dimensions defined in the suite
      /// </summary>
      IDimension[] AllAvailableDimensions();

      /// <summary>
      ///    Returns the name of all dimensions defined in the suite
      /// </summary>
      string[] AllAvailableDimensionNames();

      /// <summary>
      ///    Returns the name of all units defined in the suite for the given <paramref name="dimensionName"/>
      /// </summary>
      string[] AllAvailableUnitNamesFor(string dimensionName);

      /// <summary>
      ///    Returns <c>true</c> if <paramref name="unit" /> exists in <paramref name="dimensionName" /> otherwise <c>false</c>
      ///    Throws an exception if a dimension named <paramref name="dimensionName" /> does not exist
      /// </summary>
      bool HasUnit(string dimensionName, string unit);

      /// <summary>
      ///    Returns the baseUnit for the dimension named <paramref name="dimensionName" />.
      ///    Throws an exception if a dimension named <paramref name="dimensionName" /> does not exist
      /// </summary>
      string BaseUnitFor(string dimensionName);
   }

   public class DimensionTask : IDimensionTask
   {
      private readonly IDimensionFactory _dimensionFactory;

      public DimensionTask(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public bool HasUnit(string dimensionName, string unit) => DimensionByName(dimensionName).HasUnit(unit);

      public string BaseUnitFor(string dimensionName) => DimensionByName(dimensionName).BaseUnit.Name;

      public IDimension DimensionByName(string dimensionName) => _dimensionFactory.Dimension(dimensionName);

      public IDimension DimensionForUnit(string unit) => _dimensionFactory.DimensionForUnit(unit);

      public bool HasDimension(string dimensionName) => _dimensionFactory.TryGetDimension(dimensionName, out _);

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double[] valuesInBaseUnit)
      {
         return convertToUnit(dimension, targetUnit, null, valuesInBaseUnit);
      }

      public double[] ConvertToUnit(IDimension dimension, string targetUnit, double valueInBaseUnit)
      {
         return convertToUnit(dimension, targetUnit, null, valueInBaseUnit);
      }

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double[] valuesInBaseUnit)
      {
         return convertToUnit(DimensionByName(dimensionName), targetUnit, null, valuesInBaseUnit);
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

      public double[] ConvertToUnit(string dimensionName, string targetUnit, double valueInBaseUnit)
      {
         return convertToUnit(DimensionByName(dimensionName), targetUnit, null, valueInBaseUnit);
      }

      public double[] ConvertToBaseUnit(IDimension dimension, string displayUnit, double[] valuesInDisplayUnit, double molWeight)
      {
         return convertToBaseUnit(dimension, displayUnit, molWeight, valuesInDisplayUnit);
      }

      public double[] ConvertToBaseUnit(IDimension dimension, string displayUnit, double[] valuesInDisplayUnit)
      {
         return convertToBaseUnit(dimension, displayUnit, molWeight: null, valuesInDisplayUnit);
      }

      public double[] ConvertToBaseUnit(IDimension dimension, string displayUnit, double valueInDisplayUnit, double molWeight)
      {
         return convertToBaseUnit(dimension, displayUnit, molWeight, valueInDisplayUnit);
      }

      public double[] ConvertToBaseUnit(IDimension dimension, string displayUnit, double valueInDisplayUnit)
      {
         return convertToBaseUnit(dimension, displayUnit, molWeight: null, valueInDisplayUnit);
      }

      public double[] ConvertToBaseUnit(string dimensionName, string displayUnit, double[] valuesInDisplayUnit, double molWeight)
      {
         return convertToBaseUnit(DimensionByName(dimensionName), displayUnit, molWeight, valuesInDisplayUnit);
      }

      public double[] ConvertToBaseUnit(string dimensionName, string displayUnit, double[] valuesInDisplayUnit)
      {
         return convertToBaseUnit(DimensionByName(dimensionName), displayUnit, molWeight: null, valuesInDisplayUnit);
      }

      public double[] ConvertToBaseUnit(string dimensionName, string displayUnit, double valueInDisplayUnit, double molWeight)
      {
         return convertToBaseUnit(DimensionByName(dimensionName), displayUnit, molWeight, valueInDisplayUnit);
      }

      public double[] ConvertToBaseUnit(string dimensionName, string displayUnit, double valueInDisplayUnit)
      {
         return convertToBaseUnit(DimensionByName(dimensionName), displayUnit, molWeight: null, valueInDisplayUnit);
      }

      public IDimension[] AllAvailableDimensions() => _dimensionFactory.DimensionsSortedByName;

      public string[] AllAvailableDimensionNames() => _dimensionFactory.DimensionNamesSortedByName;

      public IDimension DimensionForStandardPKParameter(StandardPKParameter standardPKParameter)
      {
         switch (standardPKParameter)
         {
            case StandardPKParameter.Unknown:
               return Constants.Dimension.NO_DIMENSION;
            case StandardPKParameter.C_max:
            case StandardPKParameter.C_min:
            case StandardPKParameter.C_trough:
               return DimensionByName(Constants.Dimension.MOLAR_CONCENTRATION);
            case StandardPKParameter.C_max_norm:
            case StandardPKParameter.C_min_norm:
            case StandardPKParameter.C_trough_norm:
               return DimensionByName(Constants.Dimension.MASS_CONCENTRATION);
            case StandardPKParameter.t_max:
            case StandardPKParameter.t_min:
            case StandardPKParameter.Tthreshold:
            case StandardPKParameter.MRT:
            case StandardPKParameter.Thalf:
               return DimensionByName(Constants.Dimension.TIME);
            case StandardPKParameter.AUC_tEnd:
            case StandardPKParameter.AUC_inf:
            case StandardPKParameter.AUC_tEnd_inf:
               return DimensionByName(Constants.Dimension.MOLAR_AUC);
            case StandardPKParameter.AUC_tEnd_norm:
            case StandardPKParameter.AUC_inf_norm:
            case StandardPKParameter.AUC_tEnd_inf_norm:
               return DimensionByName(Constants.Dimension.MASS_AUC);
            case StandardPKParameter.AUCM_tEnd:
               return DimensionByName(Constants.Dimension.MOLAR_AUCM);
            case StandardPKParameter.FractionAucEndToInf:
               return DimensionByName(Constants.Dimension.FRACTION);
            case StandardPKParameter.Vss:
            case StandardPKParameter.Vd:
               return DimensionByName(Constants.Dimension.VOLUME_PER_BODY_WEIGHT);
            default:
               throw new ArgumentOutOfRangeException(nameof(standardPKParameter), standardPKParameter, null);
         }
      }

      public IDimension DimensionForStandardPKParameter(int standardPKParameterValue)
      {
         return DimensionForStandardPKParameter((StandardPKParameter) standardPKParameterValue);
      }

      public string[] AllAvailableUnitNamesFor(string dimensionName)
      {
         var dimension = DimensionByName(dimensionName);
         return dimension.GetUnitNames().ToArray();
      }

      private double[] convertToUnit(IDimension dimension, string targetUnit, double? molWeight, params double[] valuesInBaseUnit)
      {
         var converterContext = new DoubleArrayContext(dimension, molWeight);
         var mergedDimension = _dimensionFactory.MergedDimensionFor(converterContext);
         var unit = mergedDimension.Unit(targetUnit);
         return mergedDimension.BaseUnitValuesToUnitValues(unit, valuesInBaseUnit);
      }

      private double[] convertToBaseUnit(IDimension dimension, string displayUnit, double? molWeight, params double[] valuesInDisplayUnit)
      {
         var converterContext = new DoubleArrayContext(dimension, molWeight);
         var mergedDimension = _dimensionFactory.MergedDimensionFor(converterContext);
         var unit = mergedDimension.Unit(displayUnit);
         return mergedDimension.UnitValuesToBaseUnitValues(unit, valuesInDisplayUnit);
      }
   }
}