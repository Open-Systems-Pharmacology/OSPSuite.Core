using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationTimeProfileChartPresenter : IParameterIdentificationAnalysisPresenter
   {
   }

   public class ParameterIdentificationTimeProfileChartPresenter : ParameterIdentificationAnalysisChartPresenter<ParameterIdentificationTimeProfileChart, IParameterIdentificationMultipleRunsAnalysisView, IParameterIdentificationAnalysisPresenter>, IParameterIdentificationTimeProfileChartPresenter
   {
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public ParameterIdentificationTimeProfileChartPresenter(IParameterIdentificationMultipleRunsAnalysisView view, ChartPresenterContext chartPresenterContext, IDisplayUnitRetriever displayUnitRetriever) :
         base(view, chartPresenterContext, ApplicationIcons.TimeProfileAnalysis, PresenterConstants.PresenterKeys.ParameterIdentificationTimeProfileChartPresenter)
      {
         _displayUnitRetriever = displayUnitRetriever;
     }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults)
      {
         AddUsedObservedDataToChart();
         if (_parameterIdentification.IsRandomizedStartValueRunMode())
         {
            var bestResult = parameterIdentificationResults.MinimumBy(x => x.TotalError);
            parameterIdentificationResults.OrderBy(x => x.Index).Each(result => addSimulationOutputsForRunResult(result, curveVisible: result == bestResult));
         }
         else
         {
            parameterIdentificationResults.OrderBy(x => x.Index).Each(result => addSimulationOutputsForRunResult(result, curveVisible: true));
         }

         if (ChartIsBeingCreated)
            updateLegendVisibilityBasedOn(parameterIdentificationResults.Count);

         UpdateChartFromTemplate();
      }

      private void updateLegendVisibilityBasedOn(int numberOfRuns)
      {
         Chart.ChartSettings.LegendPosition = numberOfRuns > Constants.NUMBER_OF_RUNS_WITH_VISIBLE_LEGEND ? LegendPositions.None : LegendPositions.RightInside;
      }

      private void addSimulationOutputsForRunResult(ParameterIdentificationRunResult runResult, bool curveVisible)
      {
         addSimulationResult(runResult.BestResult.SimulationResults, runResult, curveVisible);
      }

      private void addSimulationResult(IReadOnlyList<DataRepository> simulationResults, ParameterIdentificationRunResult runResult, bool curveVisible)
      {
         AddDataRepositoriesToEditor(simulationResults);
         simulationResults.Each(simulationResult => addCurvesFor(runResult, curveVisible, simulationResult));
      }

      private void addCurvesFor(ParameterIdentificationRunResult runResult, bool curveVisible, DataRepository simulationResult)
      {
         AddCurvesFor(simulationResult, (column, curve) =>
         {
            UpdateColorForCalculationColumn(curve, column);
            curve.Visible = curveVisible;
            curve.Description = CurveDescription(curve.Name, runResult);
            adjustAxes(column);

            if (!_isMultipleRun) return;

            curve.Name = $"{runResult.Index}-{curve.Name}";
         });
      }

      private void adjustAxes(DataColumn calculatedColumn)
      {
         Chart.AxisBy(AxisTypes.Y).UnitName = _displayUnitRetriever.PreferredUnitFor(calculatedColumn).Name;
      }
   }
}