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
using static OSPSuite.Assets.Captions.ParameterIdentification;

namespace OSPSuite.Core.Services
{
   public enum DeviationLineType
   {
      Identity,
      Deviation
   }

   public class DeviationLineSettings
   {
      public DeviationLineType LineType;
      public string RepositoryName;
   }

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
      IReadOnlyList<DataRepository> AddIdentityCurves(IReadOnlyList<DataColumn> observationColumns, PredictedVsObservedChart chart);

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
      IEnumerable<DataRepository> AddDeviationLine(float foldValue, List<DataColumn> observationColumns, PredictedVsObservedChart chart);
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
            DataInfo = new DataInfo(ColumnOrigins.DeviationLine, AuxiliaryType.Undefined, string.Empty, string.Empty, null)
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
         var xAxis = chart.XAxis;
         var yAxis = chart.YAxis;

         var defaultDimension = mostFrequentDimension(observationColumns);

         setAxisDimension(defaultDimension, chart, xAxis);
         //we should also check this for consistency
         xAxis.Caption = $"{ObservedChartAxis} {defaultDimension}";
         yAxis.Caption = $"{SimulatedChartAxis}  {defaultDimension}";
         chart.UpdateAxesVisibility();
      }

      public IEnumerable<DataRepository> AddDeviationLine(float foldValue, List<DataColumn> observationColumns, PredictedVsObservedChart chart)
      {
         var settings = new DeviationLineSettings
         {
            LineType = DeviationLineType.Deviation,
            RepositoryName = Deviation
         };

         var deviationCurves = createDeviationRepositories(foldValue, observationColumns, settings);

         // only count the plotted folds that are needed to select the next line type. For that reason, we don't want to count the unity fold, nor the fold just plotted
         // That's foldValue and 1.0f are not counted
         chart.AddDeviationCurvesForFoldValue(foldValue, _dimensionFactory, deviationCurves,
            (column, curve) => curve.UpdateDeviationCurve(column.Name, chart.PlottedFolds().Except(new[] { foldValue, 1.0f }).Count()));


         //adding one of the deviation lines to the legend. Workaround for now until the presenter is adjusted for Predicted vs. Observed Chart.
         var upperDeviationLine = chart.Curves.FindByName(Captions.Chart.DeviationLines.DeviationLineNameUpper(foldValue));
         if (upperDeviationLine != null)
            upperDeviationLine.VisibleInLegend = true;

         chart.UpdateAxesVisibility();
         chart.AddToDeviationFoldValue(foldValue);
         return deviationCurves;
      }

      public IReadOnlyList<DataRepository> AddIdentityCurves(IReadOnlyList<DataColumn> observationColumns, PredictedVsObservedChart chart)
      {
         var settings = new DeviationLineSettings
         {
            LineType = DeviationLineType.Identity,
            RepositoryName = Identity
         };

         var deviationCurves = createDeviationRepositories(foldValue: 1, observationColumns, settings);
         chart.AddDeviationCurvesForFoldValue(foldValue: 1, _dimensionFactory, deviationCurves, (column, curve) => curve.UpdateIdentityCurve(Identity));

         chart.UpdateAxesVisibility();
         return deviationCurves;
      }

      private IReadOnlyList<DataRepository> createDeviationRepositories(float foldValue, IReadOnlyList<DataColumn> observationColumns,
         DeviationLineSettings settings)
      {
         var uniqueDimensions = observationColumns.Select(dataColumn => _dimensionFactory.MergedDimensionFor(dataColumn))
            .DistinctBy(dimension =>
               dimension.DisplayName); //We are using display name here as it is the only way to identify unique merge dimensions

         return uniqueDimensions.Select(dim => createDeviationRepository(foldValue, observationColumns, dim, settings))
            .Where(x => x != null).ToList();
      }

      private DataRepository createDeviationRepository(float foldValue, IReadOnlyList<DataColumn> dataColumns,
         IDimension mergedDimension, DeviationLineSettings settings)
      {
         var columnsWithMatchingDimension = dataColumns.Where(x => x.Dimension.IsEquivalentTo(mergedDimension)).ToList();

         var columnsForIdentityRepository = columnsWithMatchingDimension.Where(column => columnNonZeroValues(column).Any()).ToList();
         if (!columnsForIdentityRepository.Any())
            return null;

         var identityMinimum = getIdentityMinimum(columnsForIdentityRepository, mergedDimension);
         var identityMaximum = getIdentityMaximum(columnsForIdentityRepository, mergedDimension);

         var deviationDataRepository = createRepositoryBasedOnSettings(foldValue, mergedDimension, settings, identityMinimum, identityMaximum);
         return deviationDataRepository;
      }

      private DataRepository createRepositoryBasedOnSettings(float foldValue, IDimension mergedDimension, DeviationLineSettings settings,
         float identityMinimum, float identityMaximum)
      {
         var deviationDataRepository = new DataRepository {Name = settings.RepositoryName};
         var changePercentageForUpperCurve = foldValue - 1;

         switch (settings.LineType)
         {
            case DeviationLineType.Deviation:
               var baseGridDeviation = new BaseGrid(settings.RepositoryName, mergedDimension)
               {
                  Values = new List<float>() {identityMinimum, identityMaximum}
               };

               var valuesUpper = new DataColumn(Captions.Chart.DeviationLines.DeviationLineNameUpper(foldValue), mergedDimension,
                  baseGridDeviation)
               {
                  Values = new List<float>()
                  {
                     (identityMinimum + (identityMinimum * changePercentageForUpperCurve)),
                     (identityMaximum + (identityMaximum * changePercentageForUpperCurve))
                  },
                  DataInfo = new DataInfo(ColumnOrigins.DeviationLine, AuxiliaryType.Undefined, string.Empty, string.Empty, null)
               };
               deviationDataRepository.Add(valuesUpper);

               var valuesLower = new DataColumn(Captions.Chart.DeviationLines.DeviationLineNameLower(foldValue), mergedDimension,
                  baseGridDeviation)
               {
                  Values = new List<float>() {identityMinimum / foldValue, identityMaximum / foldValue},
                  DataInfo = new DataInfo(ColumnOrigins.DeviationLine, AuxiliaryType.Undefined, string.Empty, string.Empty, null)
               };
               deviationDataRepository.Add(valuesLower);
               break;
            case DeviationLineType.Identity:
               deviationDataRepository.Add(Math.Abs(identityMaximum - identityMinimum) > float.Epsilon
                  ? createIdentityColumn(mergedDimension, identityMinimum, identityMaximum)
                  : createIdentityColumn(mergedDimension, identityMinimum));
               break;

            default:
               throw new ArgumentOutOfRangeException();
         }

         return deviationDataRepository;
      }

      private void setAxisDimension(IDimension dimension, PredictedVsObservedChart chart, Axis axis)
      {
         if (dimension != null)
            axis.Dimension = dimension;

         axis.Scaling = chart.YAxis.Scaling;
         axis.UnitName = chart.YAxis.UnitName;
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
            Name = CreateCurveNameForPredictedVsObserved(observationColumn.Repository.Name,
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
         chart.YAxis.UnitName = _displayUnitRetriever.PreferredUnitFor(calculationColumn).Name;
      }

      private static float getIdentityMinimum(IEnumerable<DataColumn> allObservationColumns, IDimension mergedDimension)
      {
         // Avoid selection of '0' values because they cannot be plotted in log scales
         return (float) allObservationColumns.Min(column =>
            mergedDimension.UnitValueToBaseUnitValue(column.Dimension.BaseUnit, columnNonZeroValues(column).Min()));
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