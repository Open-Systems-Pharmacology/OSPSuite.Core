using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface ISimulationPredictedVsObservedChartService
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
      IReadOnlyList<DataRepository> AddIdentityCurves(IEnumerable<DataColumn> observationColumns, SimulationPredictedVsObservedChart chart);

      /// <summary>
      ///    Adds curves to the <paramref name="chart" /> for each combination of <paramref name="observationColumns" /> and
      ///    <paramref name="calculationColumn" />
      ///    <paramref name="action" /> will be run when the curve is created for the <paramref name="calculationColumn" />
      /// </summary>
      void AddCurvesFor(IEnumerable<DataColumn> observationColumns, DataColumn calculationColumn, SimulationPredictedVsObservedChart chart, Action<DataColumn, Curve> action = null);

      /// <summary>
      ///    Sets the <paramref name="chart" /> x axis dimension to the most occurring dimension from
      ///    <paramref name="observationColumns" />
      /// </summary>
      void SetXAxisDimension(IEnumerable<DataColumn> observationColumns, SimulationPredictedVsObservedChart chart);

      IEnumerable<DataRepository> AddDeviationLine(float foldValue, List<DataColumn> observationColumns, SimulationPredictedVsObservedChart chart);
   }

   public class SimulationPredictedVsObservedChartService : ISimulationPredictedVsObservedChartService
   {
      private const string IDENTITY = "Identity";
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public SimulationPredictedVsObservedChartService(IDimensionFactory dimensionFactory, IDisplayUnitRetriever displayUnitRetriever)
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
            Name = IDENTITY
         };
         return values;
      }

      public void AddCurvesFor(IEnumerable<DataColumn> observationColumns, DataColumn calculationColumn, SimulationPredictedVsObservedChart chart, Action<DataColumn, Curve> action = null)
      {
         observationColumns.Each(observationColumn => plotPredictedVsObserved(observationColumn, calculationColumn, chart, action));
      }

      public void SetXAxisDimension(IEnumerable<DataColumn> observationColumns, SimulationPredictedVsObservedChart chart)
      {
         var dataColumns = observationColumns.ToList();

         var defaultDimension = mostFrequentDimension(dataColumns);
         var xAxis = chart.AxisBy(AxisTypes.X);

         if (defaultDimension != null)
            xAxis.Dimension = defaultDimension;

         xAxis.Scaling = chart.AxisBy(AxisTypes.Y).Scaling;
         xAxis.UnitName = chart.AxisBy(AxisTypes.Y).UnitName;

         chart.UpdateAxesVisibility();
      }

      public IEnumerable<DataRepository> AddDeviationLine(float foldValue, List<DataColumn> observationColumns, SimulationPredictedVsObservedChart chart)
      {
         var deviationCurves = addDeviationLines(foldValue, observationColumns, chart).ToList();
         chart.UpdateAxesVisibility();
         return deviationCurves;
      }
      private IEnumerable<DataRepository> addDeviationLines(float foldValue, List<DataColumn> observationColumns, SimulationPredictedVsObservedChart chart)
      {
         var dataColumns = observationColumns.ToList();
         //We are using display name here as it is the only way to identify unique merge dimensions
         var uniqueDimensions = dataColumns.Select(dataColumn => _dimensionFactory.MergedDimensionFor(dataColumn))
            .DistinctBy(dimension => dimension.DisplayName);

         foreach (var mergedDimension in uniqueDimensions)
         {
            foreach (var p in createFoldDeviationRepository(foldValue, chart, dataColumns, mergedDimension)) yield return p;
         }
      }

      private IEnumerable<DataRepository> createFoldDeviationRepository(float foldValue, SimulationPredictedVsObservedChart chart, List<DataColumn> dataColumns,
         IDimension mergedDimension)
      {
         var deviationLineRepository = new List<DataRepository>();
         var deviationLineUpper = createDeviationRepository(foldValue, dataColumns, mergedDimension);
         deviationLineRepository.Add(deviationLineUpper);
         var deviationLineLower = createLowerDeviationRepository(foldValue, dataColumns, mergedDimension);
         deviationLineRepository.Add(deviationLineLower);
         chart.AddCurvesFor(deviationLineUpper, x => x.Name, _dimensionFactory, (column, curve) => curve.UpdateMarkerCurve(foldValue + "fold upper deviation line"));
         chart.AddCurvesFor(deviationLineLower, x => x.Name, _dimensionFactory, (column, curve) => curve.UpdateMarkerCurve(foldValue + "fold upper deviation line"));

         return deviationLineRepository;
      }

      private DataRepository createDeviationRepository(float foldValue, List<DataColumn> dataColumns, IDimension mergedDimension)
      {
         var dataRepository = new DataRepository { Name = "Deviation_Upper" };

         var columnsWithMatchingDimension = dataColumns.Where(x => x.Dimension.IsEquivalentTo(mergedDimension)).ToList();

         var columnsForIdentityRepository = columnsWithMatchingDimension.Where(column => columnNonZeroValues(column).Any()).ToList();
         if (!columnsForIdentityRepository.Any())
            return null;

         var identityMinimum = getIdentityMinimum(columnsForIdentityRepository, mergedDimension);
         var identityMaximum = getIdentityMaximum(columnsForIdentityRepository, mergedDimension);

         //======================
         var baseGrid = new BaseGrid(mergedDimension.Name, mergedDimension)
         {
            Values = new List<float>() { identityMinimum, identityMaximum }
         };

         var values = new DataColumn("Marker_Deviation_Upper", mergedDimension, baseGrid)
         {
            Values = new List<float>() { (identityMinimum + (identityMinimum * foldValue)), (identityMaximum + (identityMaximum * foldValue)) },
            Name = "Deviation"
         };

         dataRepository.Add(values);
         return dataRepository;
      }

      private DataRepository createLowerDeviationRepository(float foldValue, List<DataColumn> dataColumns, IDimension mergedDimension)
      {
         var dataRepository = new DataRepository { Name = "DeviationLower" };

         var columnsWithMatchingDimension = dataColumns.Where(x => x.Dimension.IsEquivalentTo(mergedDimension)).ToList();

         var columnsForIdentityRepository = columnsWithMatchingDimension.Where(column => columnNonZeroValues(column).Any()).ToList();
         if (!columnsForIdentityRepository.Any())
            return null;

         var identityMinimum = getIdentityMinimum(columnsForIdentityRepository, mergedDimension);
         var identityMaximum = getIdentityMaximum(columnsForIdentityRepository, mergedDimension);

         //======================
         var testValue = 0.0001.ToFloat();
         var baseGrid = new BaseGrid(mergedDimension.Name, mergedDimension)
         {
            Values = new List<float>() { (testValue + (testValue * foldValue)), (identityMinimum + (identityMinimum * foldValue)), (identityMaximum + (identityMaximum * foldValue)) }
         };

         var values = new DataColumn("Marker_Deviation_Upper", mergedDimension, baseGrid)
         {
            Values = new List<float>() { testValue, identityMinimum, identityMaximum },
            Name = "Deviation"
         };

         dataRepository.Add(values);
         return dataRepository;
      }


      public IReadOnlyList<DataRepository> AddIdentityCurves(IEnumerable<DataColumn> observationColumns, SimulationPredictedVsObservedChart chart)
      {
         var identityCurves = addIdentityCurves(observationColumns, chart).ToList();
         chart.UpdateAxesVisibility();
         return identityCurves;
      }

      private IEnumerable<DataRepository> addIdentityCurves(IEnumerable<DataColumn> observationColumns, SimulationPredictedVsObservedChart chart)
      {
         var dataColumns = observationColumns.ToList();
         //We are using display name here as it is the only way to identify unique merge dimensions
         var uniqueDimensions = dataColumns.Select(dataColumn => _dimensionFactory.MergedDimensionFor(dataColumn)).DistinctBy(dimension => dimension.DisplayName);

         foreach (var mergedDimension in uniqueDimensions)
         {
            var identityRepository = createIdentityRepository(dataColumns, mergedDimension);
            if (identityRepository == null) continue;

            chart.AddCurvesFor(identityRepository, x => x.Name, _dimensionFactory, (column, curve) => curve.UpdateMarkerCurve(Identity));
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

      private void plotPredictedVsObserved(DataColumn observationColumn, DataColumn calculationColumn, SimulationPredictedVsObservedChart chart, Action<DataColumn, Curve> action)
      {
         if (chart.FindCurveWithSameData(observationColumn, calculationColumn) == null)
         {
            addResultCurves(observationColumn, calculationColumn, chart, action);
         }
         adjustAxes(calculationColumn, chart);
      }

      private void addResultCurves(DataColumn observationColumn, DataColumn simulationResultColumn, SimulationPredictedVsObservedChart chart, Action<DataColumn, Curve> action)
      {
         var curve = new Curve { Name = Captions.ParameterIdentification.CreateCurveNameForPredictedVsObserved(observationColumn.Repository.Name, simulationResultColumn.Repository.Name) };
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

      private void adjustAxes(DataColumn calculationColumn, SimulationPredictedVsObservedChart chart)
      {
         chart.AxisBy(AxisTypes.Y).UnitName = _displayUnitRetriever.PreferredUnitFor(calculationColumn).Name;
      }

      private static float getIdentityMinimum(IEnumerable<DataColumn> allObservationColumns, IDimension mergedDimension)
      {
         // Avoid selection of '0' values because they cannot be plotted in log scales
         return (float)allObservationColumns.Min(column => mergedDimension.UnitValueToBaseUnitValue(column.Dimension.BaseUnit, columnNonZeroValues(column).Min()));
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