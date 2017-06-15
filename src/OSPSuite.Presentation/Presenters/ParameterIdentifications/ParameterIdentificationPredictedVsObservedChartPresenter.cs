using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Services.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationPredictedVsObservedChartPresenter : IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

   public class ParameterIdentificationPredictedVsObservedChartPresenter : ParameterIdentificationSingleRunAnalysisPresenter<ParameterIdentificationPredictedVsObservedChart>, IParameterIdentificationPredictedVsObservedChartPresenter
   {
      private readonly IPredictedVsObservedChartService _predictedVsObservedChartService;
      private readonly List<DataRepository> _identityRepositories;

      public ParameterIdentificationPredictedVsObservedChartPresenter(IParameterIdentificationSingleRunAnalysisView view, ChartPresenterContext chartPresenterContext,IPredictedVsObservedChartService predictedVsObservedChartService) :
            base(view, chartPresenterContext,  ApplicationIcons.PredictedVsObservedAnalysis, PresenterConstants.PresenterKeys.ParameterIdentificationPredictedVsActualChartPresenter)
      {
         _predictedVsObservedChartService = predictedVsObservedChartService;
         view.HelpId = HelpId.Tool_PI_Analysis_PredictedVsObserved;
         _identityRepositories = new List<DataRepository>();
      }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults)
      {
         base.UpdateAnalysisBasedOn(parameterIdentificationResults);
         if (!_parameterIdentification.AllObservedData.Any())
            return;

         var observationColumns = _parameterIdentification.AllObservedData.Select(x => x.FirstDataColumn()).ToList();

         _identityRepositories.AddRange(_predictedVsObservedChartService.AddIdentityCurves(observationColumns, Chart));

         if (ChartIsBeingCreated)
            _predictedVsObservedChartService.SetXAxisDimension(observationColumns, Chart);

         _identityRepositories.Each(AddDataRepositoryToEditor);

         AddDataRepositoriesToEditor(_parameterIdentification.AllObservedData);
         UpdateChartFromTemplate();
      }

      protected override void AddRunResultToChart(ParameterIdentificationRunResult runResult)
      {
         runResult.BestResult.SimulationResults.Each(simulationResult => addPredictedVsObservedToChart(simulationResult, (column, curve) =>
         {
            curve.Description = CurveDescription(curve.Name, runResult);
            curve.Name = column.PathAsString;
         }));
      }

      private void addPredictedVsObservedToChart(DataRepository simulationResult, Action<DataColumn, Curve> action)
      {
         AddResultRepositoryToEditor(simulationResult);
         plotAllCalculations(simulationResult, action);
      }

      private void plotAllCalculations(DataRepository simulationResult, Action<DataColumn, Curve> action)
      {
         var calculationColumns = calculationColumnsToPlot(simulationResult);

         calculationColumns.Each(calculationColumn =>
         {
            if (!_parameterIdentification.AllObservationColumnsFor(calculationColumn.PathAsString).Any())
               Chart.RemoveCurvesForColumn(calculationColumn);

            SelectColorForCalculationColumn(calculationColumn);

            _predictedVsObservedChartService.AddCurvesFor(_parameterIdentification.AllObservationColumnsFor(calculationColumn.PathAsString),
               calculationColumn, Chart, (column, curve) =>
               {
                  UpdateColorForCalculationColumn(curve, calculationColumn);
                  curve.VisibleInLegend = !chartAlreadyContainsCurveFor(calculationColumn);
                  action(calculationColumn, curve);
               });
         });
      }

      private IEnumerable<DataColumn> calculationColumnsToPlot(DataRepository dataRepository)
      {
         var calculationColumns = dataRepository.Where(isPlottableCalculation).ToList();

         if (calculationColumns.Any())
            return calculationColumns;

         return dataRepository.Where(x => columnIsCalculationWithDimension(x, _parameterIdentification.AllOutputMappings.First().Dimension)).ToList();
      }

      private bool chartAlreadyContainsCurveFor(DataColumn calculationColumn)
      {
         return Chart.Curves.Any(x => Equals(x.yData, calculationColumn));
      }

      private static bool columnIsCalculationWithDimension(DataColumn column, IDimension dimension)
      {
         return isCalculationOrCalculationAuxiliaryColumn(column) && column.Dimension.IsEquivalentTo(dimension);
      }

      private static bool isPlottableCalculation(DataColumn column)
      {
         return isCalculationOrCalculationAuxiliaryColumn(column) && (column.IsConcentration() || column.IsFraction());
      }

      public override void Clear()
      {
         base.Clear();
         _identityRepositories.Each(x => Chart.RemoveCurvesForDataRepository(x));
      }

      private static bool isCalculationOrCalculationAuxiliaryColumn(DataColumn column)
      {
         return column.IsCalculationColumn() || column.IsCalculationAuxiliaryColumn();
      }
   }
}