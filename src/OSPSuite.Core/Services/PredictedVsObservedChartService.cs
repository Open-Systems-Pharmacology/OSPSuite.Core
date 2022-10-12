using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface IPredictedVsObservedChartService
   {
      /// <summary>
      ///    This will be the name of the curve added to the chart representing the Identity curve where predicted equals
      ///    observed
      /// </summary>
      string Identity { get; }

      /// <summary>
      ///    Adds the identity curve to the <paramref name="chart" /> where the values predicted equals the values observed
      ///    <returns>Any identity repositories created</returns>
      /// </summary>
      IReadOnlyList<DataRepository> AddIdentityCurves(IEnumerable<DataColumn> observationColumns, PredictedVsObservedChart chart);

      /// <summary>
      ///    Adds curves to the <paramref name="chart" /> for each combination of <paramref name="observationColumns" /> and
      ///    <paramref name="calculationColumn" />
      ///    <paramref name="action" /> will be run when the curve is created for the <paramref name="calculationColumn" />
      /// </summary>
      void AddCurvesFor(IEnumerable<DataColumn> observationColumns, DataColumn calculationColumn, PredictedVsObservedChart chart,
         Action<DataColumn, Curve> action = null);

      /// <summary>
      ///    Sets the <paramref name="chart" /> x axis dimension to the most occurring dimension from
      ///    <paramref name="observationColumns" /> and adjusts the axes names
      /// </summary>
      void ConfigureAxesDimensionAndTitle(IReadOnlyList<DataColumn> observationColumns, PredictedVsObservedChart chart);

      /// <summary>
      ///    Creates new deviation line with fold value <paramref name="foldValue" />
      /// </summary>
      IEnumerable<DataRepository> AddDeviationLine(float foldValue, List<DataColumn> observationColumns, PredictedVsObservedChart chart,
         int deviationLinesCount);
   }

   public class PredictedVsObservedChartService : IPredictedVsObservedChartService
   {
      private const string IDENTITY = "Identity";
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public PredictedVsObservedChartService(IDimensionFactory dimensionFactory, IDisplayUnitRetriever displayUnitRetriever)
      {
         _dimensionFactory = dimensionFactory;
         _displayUnitRetriever = displayUnitRetriever;
      }

      public string Identity => IDENTITY;

      private DataRepository createIdentityRepository(IReadOnlyList<DataColumn> allObservationColumns, IDimension mergedDimension)
      {
         var dataRepository = new DataRepository { Name = IDENTITY };

         var columnsWithMatchingDimension = allObservationColumns.Where(x => x.Dimension.IsEquivalentTo(mergedDimension)).ToList();

         var columnsForIdentityRepository = columnsWithMatchingDimension.Where(column => columnNonZeroValues(column).Any()).ToList();
         if (!columnsForIdentityRepository.Any())
            return null;

         var identityMinimum = getIdentityMinimum(columnsForIdentityRepository, mergedDimension);
         var identityMaximum = getIdentityMaximum(columnsForIdentityRepository, mergedDimension);

         dataRepository.Add(Math.Abs(identityMaximum - identityMinimum) > float.Epsilon
            ? createIdentityColumn(mergedDimension, identityMinimum, identityMaximum)
            : createIdentityColumn(mergedDimension, identityMinimum));

         return dataRepository;
      }

      private DataColumn createIdentityColumn(IDimension mergedDimension, params float[] identityValues)
      {
         var baseGrid = new BaseGrid(mergedDimension.Name, mergedDimension)
         {
            Values = identityValues
         };
         var values = new DataColumn("Marker_Identity", mergedDimension, baseGrid)
         {
            Values = identityValues,
            Name = IDENTITY,
            DataInfo = new DataInfo(ColumnOrigins.IdentityCurve, AuxiliaryType.Undefined, string.Empty, string.Empty, null)
         };
         return values;
      }

      public void AddCurvesFor(IEnumerable<DataColumn> observationColumns, DataColumn calculationColumn, PredictedVsObservedChart chart,
         Action<DataColumn, Curve> action = null)
      {
         observationColumns.Each(observationColumn => plotPredictedVsObserved(observationColumn, calculationColumn, chart, action));
      }

      public void ConfigureAxesDimensionAndTitle(IReadOnlyList<DataColumn> observationColumns, PredictedVsObservedChart chart)
      {
         var xAxis = chart.AxisBy(AxisTypes.X);
         var yAxis = chart.AxisBy(AxisTypes.Y);

         var defaultDimension = mostFrequentDimension(observationColumns);

         setAxisDimension(defaultDimension, chart, xAxis);
         xAxis.Caption = Captions.ParameterIdentification.ObservedChartAxis + defaultDimension;
         yAxis.Caption = Captions.ParameterIdentification.SimulatedChartAxis + defaultDimension;
         chart.UpdateAxesVisibility();
      }

      public IEnumerable<DataRepository> AddDeviationLine(float foldValue, List<DataColumn> observationColumns, PredictedVsObservedChart chart,
         int deviationLinesCount)
      {
         var deviationCurves = addDeviationLines(foldValue, observationColumns, chart, deviationLinesCount).ToList();
         chart.UpdateAxesVisibility();
         return deviationCurves;
      }

      private IEnumerable<DataRepository> addDeviationLines(float foldValue, IReadOnlyList<DataColumn> observationColumns,
         PredictedVsObservedChart chart, int deviationLinesCount)
      {
         var dataColumns = observationColumns;

         var uniqueDimensions = dataColumns.Select(dataColumn => _dimensionFactory.MergedDimensionFor(dataColumn))
            .DistinctBy(dimension =>
               dimension.DisplayName); //We are using display name here as it is the only way to identify unique merge dimensions

         foreach (var mergedDimension in uniqueDimensions)
         {
            yield return createFoldDeviationRepositories(foldValue, chart, dataColumns, mergedDimension, deviationLinesCount);
         }
      }

      private DataRepository createFoldDeviationRepositories(float foldValue, PredictedVsObservedChart chart,
         IReadOnlyList<DataColumn> dataColumns,
         IDimension mergedDimension, int deviationLinesCount)
      {
         var deviationLineRepository= createDeviationRepository(foldValue, dataColumns, mergedDimension);
         chart.AddCurvesFor(deviationLineRepository, x => x.Name, _dimensionFactory,
            (column, curve) => curve.UpdateDeviationCurve(column.Name, deviationLinesCount));

         return deviationLineRepository;
      }

      private DataRepository createDeviationRepository(float foldValue, IReadOnlyList<DataColumn> dataColumns,
         IDimension mergedDimension)
      {
         var columnsWithMatchingDimension = dataColumns.Where(x => x.Dimension.IsEquivalentTo(mergedDimension)).ToList();

         var columnsForIdentityRepository = columnsWithMatchingDimension.Where(column => columnNonZeroValues(column).Any()).ToList();
         if (!columnsForIdentityRepository.Any())
            return null;

         var identityMinimum = getIdentityMinimum(columnsForIdentityRepository, mergedDimension);
         var identityMaximum = getIdentityMaximum(columnsForIdentityRepository, mergedDimension);

         var baseGridDeviation = new BaseGrid(Captions.ParameterIdentification.Deviation, mergedDimension)
         {
            Values = new List<float>() { identityMinimum, identityMaximum }
         };

         var deviationDataRepository = new DataRepository { Name = Captions.ParameterIdentification.Deviation };
         var changePercentageForUpperCurve = foldValue - 1;
         var valuesUpper = new DataColumn(Captions.ParameterIdentification.DeviationLineNameUpper(foldValue), mergedDimension, baseGridDeviation)
         {
            Values = new List<float>() { (identityMinimum + (identityMinimum * changePercentageForUpperCurve)), (identityMaximum + (identityMaximum * changePercentageForUpperCurve)) },
            DataInfo = new DataInfo(ColumnOrigins.IdentityCurve, AuxiliaryType.Undefined, string.Empty, string.Empty, null)
         };
         deviationDataRepository.Add(valuesUpper);

         var valuesLower = new DataColumn(Captions.ParameterIdentification.DeviationLineNameLower(foldValue), mergedDimension, baseGridDeviation)
         {
            Values = new List<float>() { identityMinimum / foldValue, identityMaximum / foldValue },
            DataInfo = new DataInfo(ColumnOrigins.IdentityCurve, AuxiliaryType.Undefined, string.Empty, string.Empty, null)
         };
         deviationDataRepository.Add(valuesLower);


         return deviationDataRepository;
      }

      private void setAxisDimension(IDimension dimension, PredictedVsObservedChart chart, Axis axis)
      {
         if (dimension != null)
            axis.Dimension = dimension;

         axis.Scaling = chart.AxisBy(AxisTypes.Y).Scaling;
         axis.UnitName = chart.AxisBy(AxisTypes.Y).UnitName;
      }

      public IReadOnlyList<DataRepository> AddIdentityCurves(IEnumerable<DataColumn> observationColumns, PredictedVsObservedChart chart)
      {
         var identityCurves = addIdentityCurves(observationColumns, chart).ToList();
         chart.UpdateAxesVisibility();
         return identityCurves;
      }

      private IEnumerable<DataRepository> addIdentityCurves(IEnumerable<DataColumn> observationColumns, PredictedVsObservedChart chart)
      {
         var dataColumns = observationColumns.ToList();
         //We are using display name here as it is the only way to identify unique merge dimensions
         var uniqueDimensions = dataColumns.Select(dataColumn => _dimensionFactory.MergedDimensionFor(dataColumn))
            .DistinctBy(dimension => dimension.DisplayName);

         foreach (var mergedDimension in uniqueDimensions)
         {
            var identityRepository = createIdentityRepository(dataColumns, mergedDimension);
            if (identityRepository == null) continue;

            chart.AddCurvesFor(identityRepository, x => x.Name, _dimensionFactory, (column, curve) => curve.UpdateIdentityCurve(Identity));
            yield return identityRepository;
         }
      }

      private IDimension mostFrequentDimension(IReadOnlyList<DataColumn> columns)
      {
         var preferredDimension = mostFrequentDimensionFrom(columns.Where(isPreferredDimension).Select(mergedDimensionsFor));

         if (preferredDimension != null)
            return preferredDimension;

         var dimensions = columns.Select(mergedDimensionsFor).ToList();

         return mostFrequentDimensionFrom(dimensions);
      }

      private static bool isPreferredDimension(DataColumn column)
      {
         return column.IsConcentration() || column.IsFraction();
      }

      private static IDimension mostFrequentDimensionFrom(IEnumerable<IDimension> dimensions)
      {
         return dimensions.GroupBy(x => x.DisplayName).OrderByDescending(x => x.Count()).FirstOrDefault()?.FirstOrDefault();
      }

      private IDimension mergedDimensionsFor(DataColumn dataColumn)
      {
         return _dimensionFactory.MergedDimensionFor(dataColumn);
      }

      private void plotPredictedVsObserved(DataColumn observationColumn, DataColumn calculationColumn, PredictedVsObservedChart chart,
         Action<DataColumn, Curve> action)
      {
         if (chart.FindCurveWithSameData(observationColumn, calculationColumn) == null)
         {
            addResultCurves(observationColumn, calculationColumn, chart, action);
         }

         adjustAxes(calculationColumn, chart);
      }

      private void addResultCurves(DataColumn observationColumn, DataColumn simulationResultColumn, PredictedVsObservedChart chart,
         Action<DataColumn, Curve> action)
      {
         var curve = new Curve
         {
            Name = Captions.ParameterIdentification.CreateCurveNameForPredictedVsObserved(observationColumn.Repository.Name,
               simulationResultColumn.Repository.Name)
         };
         curve.SetxData(observationColumn, _dimensionFactory);
         curve.SetyData(simulationResultColumn, _dimensionFactory);
         adjustResultCurveDisplay(curve);
         action?.Invoke(simulationResultColumn, curve);
         chart.AddCurve(curve);
      }

      private void adjustResultCurveDisplay(Curve curve)
      {
         curve.LineStyle = LineStyles.None;
         curve.Symbol = Symbols.Diamond;
      }

      private void adjustAxes(DataColumn calculationColumn, PredictedVsObservedChart chart)
      {
         chart.AxisBy(AxisTypes.Y).UnitName = _displayUnitRetriever.PreferredUnitFor(calculationColumn).Name;
      }

      private static float getIdentityMinimum(IEnumerable<DataColumn> allObservationColumns, IDimension mergedDimension)
      {
         // Avoid selection of '0' values because they cannot be plotted in log scales
         return (float)allObservationColumns.Min(column =>
            mergedDimension.UnitValueToBaseUnitValue(column.Dimension.BaseUnit, columnNonZeroValues(column).Min()));
      }

      private static IEnumerable<float> columnNonZeroValues(DataColumn column)
      {
         return column.Values.Where(x => Math.Abs(x) > 0);
      }

      private static float getIdentityMaximum(IEnumerable<DataColumn> allObservationColumns, IDimension mergedDimension)
      {
         return (float)allObservationColumns.Max(column => mergedDimension.UnitValueToBaseUnitValue(column.Dimension.BaseUnit, column.Values.Max()));
      }
   }
}