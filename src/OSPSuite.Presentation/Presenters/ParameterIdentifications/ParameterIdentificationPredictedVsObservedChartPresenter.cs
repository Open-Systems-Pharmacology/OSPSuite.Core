﻿using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationPredictedVsObservedChartPresenter : IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

   public class ParameterIdentificationPredictedVsObservedChartPresenter :
      ParameterIdentificationSingleRunAnalysisPresenter<ParameterIdentificationPredictedVsObservedChart>,
      IParameterIdentificationPredictedVsObservedChartPresenter
   {
      private readonly IPredictedVsObservedChartService _predictedVsObservedChartService;
      private readonly List<DataRepository> _identityRepositories;
      private readonly List<DataRepository> _deviationLineRepositories;

      public ParameterIdentificationPredictedVsObservedChartPresenter(IParameterIdentificationSingleRunAnalysisView view,
         ChartPresenterContext chartPresenterContext, IPredictedVsObservedChartService predictedVsObservedChartService) :
         base(view, chartPresenterContext, ApplicationIcons.PredictedVsObservedAnalysis,
            PresenterConstants.PresenterKeys.ParameterIdentificationPredictedVsObservedChartPresenter)
      {
         _predictedVsObservedChartService = predictedVsObservedChartService;
         _identityRepositories = new List<DataRepository>();
         _deviationLineRepositories = new List<DataRepository>();

         ChartDisplayPresenter.AddDeviationLinesEvent += (o, e) => addDeviationLines(e.FoldValue);
      }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults)
      {
         base.UpdateAnalysisBasedOn(parameterIdentificationResults);
         if (!_parameterIdentification.AllObservedData.Any())
            return;

         var observationColumns = _parameterIdentification.AllObservedData.Select(x => x.FirstDataColumn()).ToList();

         _identityRepositories.AddRange(_predictedVsObservedChartService.AddIdentityCurves(observationColumns, Chart));

         if (ChartIsBeingCreated)
            _predictedVsObservedChartService.ConfigureAxesDimensionAndTitle(observationColumns, Chart);

         //plot the already added and saved deviation lines
         Chart.DeviationFoldValues.Each(addDeviationLineWithoutCheck);

         AddDataRepositoriesToEditor(_identityRepositories.Union(_parameterIdentification.AllObservedData).Union(_deviationLineRepositories));
         UpdateChartFromTemplate();
      }

      protected override void ChartChanged()
      {
         base.ChartChanged();
         Chart.UpdateAxesVisibility();
         _chartPresenterContext.DisplayPresenter.Refresh();
         _chartPresenterContext.EditorPresenter.Refresh();
      }

      protected override void ClearChartAndDataRepositories()
      {
         base.ClearChartAndDataRepositories();
         _identityRepositories.Clear();
         _deviationLineRepositories.Clear();
      }

      protected override void AddRunResultToChart(ParameterIdentificationRunResult runResult)
      {
         addPredictedVsObservedToChart(runResult.BestResult.SimulationResults, (column, curve) =>
         {
            curve.Description = CurveDescription(curve.Name, runResult);
            curve.Name = NameForColumn(column);
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

      private void addDeviationLines(float foldValue)
      {
         if (Chart.HasDeviationCurveFor(foldValue))
            return;

         addDeviationLineWithoutCheck(foldValue);
      }

      private void addDeviationLineWithoutCheck(float foldValue)
      {
         var observationColumns = _parameterIdentification.AllObservedData.Select(x => x.FirstDataColumn()).ToList();
         _deviationLineRepositories.AddRange(
            _predictedVsObservedChartService.AddDeviationLine(foldValue, observationColumns, Chart));
         AddDataRepositoriesToEditor(_deviationLineRepositories);
         ChartChanged();
      }

      private void plotCalculationColumn(DataColumn calculationColumn, Action<DataColumn, Curve> action)
      {
         var allObservationsForOutput = _parameterIdentification.AllObservationColumnsFor(calculationColumn.PathAsString).ToList();

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
         _deviationLineRepositories.Each(x => Chart.RemoveCurvesForDataRepository(x));
      }

      private static bool isCalculationOrCalculationAuxiliaryColumn(DataColumn column)
      {
         return column.IsCalculation() || column.IsCalculationAuxiliary();
      }
   }
}