using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Helpers
{
   public static class DomainHelperForSpecs
   {
      private static readonly string PATH_TO_DATA = "..\\..\\..\\Data\\";

      private static Dimension _lengthDimension;
      private static Dimension _concentrationDimension;
      private static Dimension _timeDimension;
      private static Dimension _fractionDimension;

      public static IDimension TimeDimensionForSpecs()
      {
         if (_timeDimension == null)
         {
            _timeDimension = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, Constants.Dimension.TIME, "min");
            _timeDimension.AddUnit(new Unit("h", 60, 0));
         }

         return _timeDimension;
      }

      public static IDimension ConcentrationDimensionForSpecs()
      {
         if (_concentrationDimension == null)
         {
            _concentrationDimension = new Dimension(new BaseDimensionRepresentation {AmountExponent = 3, LengthExponent = -1}, Constants.Dimension.MOLAR_CONCENTRATION, "µmol/l");
            _concentrationDimension.AddUnit(new Unit("mol/l", 1E6, 0));
         }

         return _concentrationDimension;
      }

      public static IDimension ConcentrationMassDimensionForSpecs()
      {
         if (_concentrationDimension == null)
         {
            _concentrationDimension = new Dimension(new BaseDimensionRepresentation { AmountExponent = 3, LengthExponent = -1 }, Constants.Dimension.MASS_CONCENTRATION, "mg/l");
            _concentrationDimension.AddUnit(new Unit("mg/l", 1E6, 0));
         }

         return _concentrationDimension;
      }

      public static IEnumerable<IDimension> ExtendedDimensionsForSpecs()
      {
         return new[] {ConcentrationDimensionForSpecs(), Constants.Dimension.NO_DIMENSION, FractionDimensionForSpecs()};
      }

      public static IDimension LengthDimensionForSpecs()
      {
         if (_lengthDimension == null)
         {
            _lengthDimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Length", "m");
            _lengthDimension.AddUnit(new Unit("cm", 0.01, 0));
            _lengthDimension.AddUnit(new Unit("mm", 0.001, 0));
         }

         return _lengthDimension;
      }

      private static void addDimensionTo(IParameter parameter)
      {
         var dimension = LengthDimensionForSpecs();
         parameter.Dimension = dimension;
      }

      public static IParameter ConstantParameterWithValue(double value, bool isDefault = false)
      {
         var parameter = new Parameter().WithFormula(new ConstantFormula(value).WithId("constantFormulaId"));
         parameter.Visible = true;
         addDimensionTo(parameter);
         parameter.IsFixedValue = true;
         parameter.IsDefault = isDefault;
         return parameter;
      }

      public static IDistributedParameter NormalDistributedParameter(double defaultMean = 0, double defaultDeviation = 1, double defaultPercentile = 0.5, bool isDefault = false, bool distributionParameterIsDefault = true)
      {
         var parameter = new DistributedParameter().WithId("P1");
         parameter.IsDefault = isDefault;
         var pathFactory = new ObjectPathFactoryForSpecs();
         var meanParameter = new Parameter {Name = Constants.Distribution.MEAN}.WithFormula(new ConstantFormula(defaultMean).WithId("MeanFormula")).WithId("Mean");
         meanParameter.IsDefault = distributionParameterIsDefault;
         addDimensionTo(meanParameter);
         var stdParameter = new Parameter {Name = Constants.Distribution.DEVIATION}.WithFormula(new ConstantFormula(defaultDeviation).WithId("DeviationFormula")).WithId("Deviation");
         stdParameter.IsDefault = distributionParameterIsDefault;
         addDimensionTo(stdParameter);
         var percentileParameter = new Parameter {Name = Constants.Distribution.PERCENTILE}.WithFormula(new ConstantFormula(defaultPercentile).WithId("PercentileFormula")).WithId("Percentile");
         percentileParameter.IsDefault = distributionParameterIsDefault;
         addDimensionTo(percentileParameter);
         parameter.Add(meanParameter);
         parameter.Add(stdParameter);
         parameter.Add(percentileParameter);
         parameter.Formula = new NormalDistributionFormula().WithId("NormalDistributionFormula");
         parameter.Formula.AddObjectPath(pathFactory.CreateRelativeFormulaUsablePath(parameter, meanParameter));
         parameter.Formula.AddObjectPath(pathFactory.CreateRelativeFormulaUsablePath(parameter, stdParameter));
         addDimensionTo(parameter);
         return parameter;
      }

      public static IdentificationParameter IdentificationParameter(string name = "IdentificationParameter", double min = 0, double max = 10, double startValue = 5, bool isFixed = false)
      {
         var identificationParameter = new IdentificationParameter
         {
            ConstantParameterWithValue(min).WithName(Constants.Parameters.MIN_VALUE),
            ConstantParameterWithValue(startValue).WithName(Constants.Parameters.START_VALUE),
            ConstantParameterWithValue(max).WithName(Constants.Parameters.MAX_VALUE)
         }.WithName(name);

         identificationParameter.IsFixed = isFixed;
         return identificationParameter;
      }

      public static DataRepository ObservedData(string id = "TestData", IDimension timeDimension = null, IDimension concentrationDimension = null, string obsDataColumnName = null)
      {
         var observedData = new DataRepository(id).WithName(id);
         var baseGrid = new BaseGrid("Time", timeDimension ?? TimeDimensionForSpecs())
         {
            Values = new[] {1.0f, 2.0f, 3.0f}
         };
         observedData.Add(baseGrid);

         var data = ConcentrationColumnForObservedData(baseGrid, concentrationDimension, obsDataColumnName);
         observedData.Add(data);

         return observedData;
      }

      public static DataColumn ConcentrationColumnForObservedData(BaseGrid baseGrid, IDimension concentrationDimension = null, string obsDataColumnName = null)
      {
         var data = new DataColumn(obsDataColumnName ?? "Col", concentrationDimension ?? ConcentrationDimensionForSpecs(), baseGrid)
         {
            Values = new[] {10f, 20f, 30f},
            DataInfo = {Origin = ColumnOrigins.Observation}
         };
         return data;
      }

      public static DataRepository ObservedDataWithLLOQ(string id = "TestDataWithLLOQ")
      {
         var observedData = ObservedData(id);
         observedData.BaseGrid.Values = new[] {0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f};
         var data = observedData.ObservationColumns().First();
         data.Values = new[] {0.05f, 0.05f, 5f, 10f, 4f, 1f, 0.05f, 0.2f, 0.1f, 0.05f, 0.05f};
         data.DataInfo.LLOQ = 0.1F;
         return observedData;
      }

      public static DataRepository ObservedDataRepository2WithLLOQ(string id = "TestData2WithLLOQ")
      {
         var observedData = ObservedData(id);
         observedData.BaseGrid.Values = new[] {1f, 2f, 3f};
         var data = observedData.ObservationColumns().First();
         data.Values = new[] {2f, 1.1f, 0.5f};
         data.DataInfo.LLOQ = 1F;
         return observedData;
      }

      public static DataRepository SimulationDataRepositoryFor(string simulationName)
      {
         var simulationResults = new DataRepository("Results");
         var baseGrid = new BaseGrid("Time", TimeDimensionForSpecs())
         {
            Values = new[] {0f, 1f, 2f, 3f, 4f}
         };
         simulationResults.Add(baseGrid);

         var data = new DataColumn("Col", ConcentrationDimensionForSpecs(), baseGrid)
         {
            Values = new[] {0f, 2.5f, 0.9f, 0.9f, 0.5f},
            DataInfo = {Origin = ColumnOrigins.Calculation},
            QuantityInfo = new QuantityInfo(new[] {simulationName, "Organism", "Blood", "Plasma", "Concentration"}, QuantityType.Drug)
         };

         simulationResults.Add(data);

         return simulationResults;
      }

      public static IDimension FractionDimensionForSpecs()
      {
         if (_fractionDimension == null)
         {
            _fractionDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.FRACTION, "");
            _fractionDimension.AddUnit(new Unit("%", 1e-2, 0));
         }

         return _fractionDimension;
      }

      public static DataRepository IndividualSimulationDataRepositoryFor(string simulationName)
      {
         var simulationResults = new DataRepository("Results");
         var baseGrid = new BaseGrid("Time", TimeDimensionForSpecs())
         {
            Values = new[] {1.0f, 2.0f, 3.0f}
         };
         simulationResults.Add(baseGrid);

         var data = ConcentrationColumnForSimulation(simulationName, baseGrid);

         simulationResults.Add(data);

         return simulationResults;
      }

      public static DataColumn ConcentrationColumnForSimulation(string simulationName, BaseGrid baseGrid)
      {
         var data = new DataColumn("Col", ConcentrationDimensionForSpecs(), baseGrid)
         {
            Values = new[] {10f, 20f, 30f},
            DataInfo = {Origin = ColumnOrigins.Calculation},
            QuantityInfo = new QuantityInfo(new[] {simulationName, "Comp", "Liver", "Cell", "Concentration"}, QuantityType.Drug)
         };
         return data;
      }

      public static IDimension NoDimension()
      {
         return Constants.Dimension.NO_DIMENSION;
      }

      public static SensitivityParameter SensitivityParameter(string name = "SensitivityParameter", double range = 0.1, double steps = 2)
      {
         return new SensitivityParameter
         {
            ConstantParameterWithValue(range).WithName(Constants.Parameters.VARIATION_RANGE),
            ConstantParameterWithValue(steps).WithName(Constants.Parameters.NUMBER_OF_STEPS),
         }.WithName(name);
      }

      public static string SimulationResultsFilePathFor(string fileNameWithoutExtension)
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PATH_TO_DATA, "SimulationResultsFiles", fileNameWithoutExtension + ".csv");
      }

      public static string SensitivityAnalysisResultsFilePathFor(string fileNameWithoutExtension)
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PATH_TO_DATA, "SensitivityAnalysisResultsFiles", fileNameWithoutExtension + ".csv");
      }

      public static string PKAnalysesFilePathFor(string fileNameWithoutExtension)
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PATH_TO_DATA, "PKAnalysesFiles", fileNameWithoutExtension + ".csv");
      }

      public static string ExcelImportFilePathFor(string fileNameWithoutExtension)
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PATH_TO_DATA, "ExcelImport", fileNameWithoutExtension + ".xlsx");
      }
   }

   public class PathCacheForSpecs<T> : PathCache<T> where T : class, IEntity
   {
      public PathCacheForSpecs() : base(new EntityPathResolverForSpecs())
      {
      }
   }
}