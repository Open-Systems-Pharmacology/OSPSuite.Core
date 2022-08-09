using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationPredictedVsObservedChartPresenter : IChartPresenter<SimulationPredictedVsObservedChart>,
      ISimulationAnalysisPresenter
   {
   }
   
   public class SimulationPredictedVsObservedChartPresenter : SimulationRunAnalysisPresenter<SimulationPredictedVsObservedChart>, 
      ISimulationPredictedVsObservedChartPresenter
   {
      private readonly IPredictedVsObservedChartService _predictedVsObservedChartService;
      private readonly List<DataRepository> _identityRepositories;
      private readonly IObservedDataRepository _observedDataRepository;

      public SimulationPredictedVsObservedChartPresenter(ISimulationRunAnalysisView view, ChartPresenterContext chartPresenterContext, IPredictedVsObservedChartService predictedVsObservedChartService, IObservedDataRepository observedDataRepository) 
         : base(view, chartPresenterContext, ApplicationIcons.PredictedVsObservedAnalysis, PresenterConstants.PresenterKeys.SimulationPredictedVsActualChartPresenter)
      {
         _predictedVsObservedChartService = predictedVsObservedChartService;
         _identityRepositories = new List<DataRepository>();
         _observedDataRepository = observedDataRepository;
      }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<IndividualResults> simulationResults)
      {
         base.UpdateAnalysisBasedOn(simulationResults);
         if (!getAllAvailableObservedData().Any())
            return;

         var observationColumns = getAllAvailableObservedData().Select(x => x.FirstDataColumn()).ToList();

         _identityRepositories.AddRange(_predictedVsObservedChartService.AddIdentityCurves(observationColumns, Chart));

         if (ChartIsBeingCreated)
            _predictedVsObservedChartService.SetXAxisDimension(observationColumns, Chart);

         AddDataRepositoriesToEditor(_identityRepositories.Union(getAllAvailableObservedData()));
         UpdateChartFromTemplate();
      }

      private IEnumerable<DataRepository> getAllAvailableObservedData()
      {
         return _observedDataRepository.AllObservedDataUsedBy(_simulation)
            .Distinct()
            .OrderBy(x => x.Name);
      }
      protected override void ChartChanged()
      {
         base.ChartChanged();
         Chart.UpdateAxesVisibility();
         _chartPresenterContext.DisplayPresenter.Refresh();
      }

      protected override void ClearChartAndDataRepositories()
      {
         base.ClearChartAndDataRepositories();
         _identityRepositories.Clear();
      }

      protected override void AddRunResultToChart()
      {
         addPredictedVsObservedToChart(new List<DataRepository>() {_simulation.ResultRepository}, (column, curve) =>
         {
            curve.Description = curve.Name;
            curve.Name = column.PathAsString;
         });
      }

      private void addPredictedVsObservedToChart(IReadOnlyList<DataRepository> simulationResults, Action<DataColumn, Curve> action)
      {
         AddResultRepositoriesToEditor(simulationResults);
         simulationResults.Each(x => plotAllCalculations(x, action));
      }

      private void plotAllCalculations(DataRepository simulationResult, Action<DataColumn, Curve> action)
      {
         var calculationColumns = calculationColumnsToPlot(simulationResult);
         calculationColumns.Each(x => plotCalculationColumn(x, action));
      }

      private void plotCalculationColumn(DataColumn calculationColumn, Action<DataColumn, Curve> action)
      {
         var allObservationsForOutput = _simulation.OutputMappings.All.Where(x => string.Equals(x.FullOutputPath, calculationColumn.PathAsString))
            .SelectMany(x => x.WeightedObservedData.ObservedData.ObservationColumns()).ToList();

         if (!allObservationsForOutput.Any())
            Chart.RemoveCurvesForColumn(calculationColumn);
               
         SelectColorForCalculationColumn(calculationColumn);

         _predictedVsObservedChartService.AddCurvesFor(allObservationsForOutput, calculationColumn, Chart, (column, curve) =>
         {
            UpdateColorForCalculationColumn(curve, calculationColumn);
            curve.VisibleInLegend = !chartAlreadyContainsCurveFor(calculationColumn);
            action(calculationColumn, curve);
         });
      }

      private IEnumerable<DataColumn> calculationColumnsToPlot(DataRepository dataRepository)
      {
         return dataRepository.Where(isPlottableCalculation).ToList();
      }

      private bool chartAlreadyContainsCurveFor(DataColumn calculationColumn)
      {
         return Chart.Curves.Any(x => Equals(x.yData, calculationColumn));
      }

      private static bool isPlottableCalculation(DataColumn column)
      {
         return isCalculationOrCalculationAuxiliaryColumn(column);
      }

      public override void Clear()
      {
         base.Clear();
         _identityRepositories.Each(x => Chart.RemoveCurvesForDataRepository(x));
      }

      private static bool isCalculationOrCalculationAuxiliaryColumn(DataColumn column)
      {
         return column.IsCalculation() || column.IsCalculationAuxiliary();
      }
   }
}