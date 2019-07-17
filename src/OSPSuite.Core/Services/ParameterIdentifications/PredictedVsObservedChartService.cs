using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services.ParameterIdentifications
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
      IReadOnlyList<DataRepository> AddIdentityCurves(IEnumerable<DataColumn> observationColumns, ParameterIdentificationPredictedVsObservedChart chart);

      /// <summary>
      ///    Adds curves to the <paramref name="chart" /> for each combination of <paramref name="observationColumns" /> and
      ///    <paramref name="calculationColumn" />
      ///    <paramref name="action" /> will be run when the curve is created for the <paramref name="calculationColumn" />
      /// </summary>
      void AddCurvesFor(IEnumerable<DataColumn> observationColumns, DataColumn calculationColumn, ParameterIdentificationPredictedVsObservedChart chart, Action<DataColumn, Curve> action = null);

      /// <summary>
      ///    Sets the <paramref name="chart" /> x axis dimension to the most occurring dimension from
      ///    <paramref name="observationColumns" />
      /// </summary>
      void SetXAxisDimension(IEnumerable<DataColumn> observationColumns, ParameterIdentificationPredictedVsObservedChart chart);
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
         var dataRepository = new DataRepository {Name = IDENTITY};

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

      public void AddCurvesFor(IEnumerable<DataColumn> observationColumns, DataColumn calculationColumn, ParameterIdentificationPredictedVsObservedChart chart, Action<DataColumn, Curve> action = null)
      {
         observationColumns.Each(observationColumn => plotPredictedVsObserved(observationColumn, calculationColumn, chart, action));
      }

      public void SetXAxisDimension(IEnumerable<DataColumn> observationColumns, ParameterIdentificationPredictedVsObservedChart chart)
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

      public IReadOnlyList<DataRepository> AddIdentityCurves(IEnumerable<DataColumn> observationColumns, ParameterIdentificationPredictedVsObservedChart chart)
      {
         var identityCurves = addIdentityCurves(observationColumns, chart).ToList();
         chart.UpdateAxesVisibility();
         return identityCurves;
      }

      private IEnumerable<DataRepository> addIdentityCurves(IEnumerable<DataColumn> observationColumns, ParameterIdentificationPredictedVsObservedChart chart)
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

      private void plotPredictedVsObserved(DataColumn observationColumn, DataColumn calculationColumn, ParameterIdentificationPredictedVsObservedChart chart, Action<DataColumn, Curve> action)
      {
         if (chart.FindCurveWithSameData(observationColumn, calculationColumn) == null)
         {
            addResultCurves(observationColumn, calculationColumn, chart, action);
         }
         adjustAxes(calculationColumn, chart);
      }

      private void addResultCurves(DataColumn observationColumn, DataColumn simulationResultColumn, ParameterIdentificationPredictedVsObservedChart chart, Action<DataColumn, Curve> action)
      {
         var curve = new Curve {Name = Captions.ParameterIdentification.CreateCurveNameForPredictedVsObserved(observationColumn.Repository.Name, simulationResultColumn.Repository.Name)};
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

      private void adjustAxes(DataColumn calculationColumn, ParameterIdentificationPredictedVsObservedChart chart)
      {
         chart.AxisBy(AxisTypes.Y).UnitName = _displayUnitRetriever.PreferredUnitFor(calculationColumn).Name;
      }

      private static float getIdentityMinimum(IEnumerable<DataColumn> allObservationColumns, IDimension mergedDimension)
      {
         // Avoid selection of '0' values because they cannot be plotted in log scales
         return (float) allObservationColumns.Min(column => mergedDimension.UnitValueToBaseUnitValue(column.Dimension.BaseUnit, columnNonZeroValues(column).Min()));
      }

      private static IEnumerable<float> columnNonZeroValues(DataColumn column)
      {
         return column.Values.Where(x => Math.Abs(x) > 0);
      }

      private static float getIdentityMaximum(IEnumerable<DataColumn> allObservationColumns, IDimension mergedDimension)
      {
         return (float) allObservationColumns.Max(column => mergedDimension.UnitValueToBaseUnitValue(column.Dimension.BaseUnit, column.Values.Max()));
      }
   }
}