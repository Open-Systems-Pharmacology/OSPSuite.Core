using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public abstract class ParameterIdentificationAnalysisChartPresenter<TChart, TView, TPresenter> : SimulationAnalysisChartPresenter<TChart, TView, TPresenter>,
      IParameterIdentificationAnalysisPresenter
      where TChart : ChartWithObservedData, ISimulationAnalysis where
      TView : class, IParameterIdentificationAnalysisView, IView<TPresenter> where TPresenter : ISimulationAnalysisPresenter
   {
      protected ParameterIdentification _parameterIdentification;
      protected bool _isMultipleRun;

      protected ParameterIdentificationAnalysisChartPresenter(TView view, ChartPresenterContext chartPresenterContext, ApplicationIcon icon,
         string presentationKey) :
         base(view, chartPresenterContext)
      {
         _view.SetAnalysisView(chartPresenterContext.EditorAndDisplayPresenter.BaseView);
         _view.ApplicationIcon = icon;
         PresentationKey = presentationKey;
         PostEditorLayout = SetColumnGroupingsAndVisibility;
         AddAllButtons();
         ChartEditorPresenter.SetLinkSimDataMenuItemVisibility(true);
      }

      public override void UpdateAnalysisBasedOn(IAnalysable analysable)
      {
         _parameterIdentification = analysable.DowncastTo<ParameterIdentification>();

         if (ChartIsBeingUpdated)
         {
            UpdateTemplateFromChart();
            ClearChartAndDataRepositories();
         }
         else
            UpdateCacheColor();

         if (_parameterIdentification.Results.Any())
         {
            _isMultipleRun = _parameterIdentification.Results.Count > 1;
            UpdateAnalysisBasedOn(_parameterIdentification.Results);
            ChartEditorPresenter.AddOutputMappings(_parameterIdentification.OutputMappings);
         }

         Refresh();
      }

      protected string CurveDescription(string name, ParameterIdentificationRunResult runResult)
      {
         int? runIndex = null;
         if (_isMultipleRun)
            runIndex = runResult.Index;

         return Captions.ParameterIdentification.CreateCurveDescription(name, runIndex, runResult.Description);
      }

      protected abstract void UpdateAnalysisBasedOn(IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults);

      protected override ISimulation SimulationFor(DataColumn dataColumn)
      {
         if (string.IsNullOrEmpty(dataColumn.PathAsString))
            return null;

         var simulationName = dataColumn.PathAsString.ToPathArray().ElementAt(0);
         return _parameterIdentification.AllSimulations.FindByName(simulationName);
      }

      protected void AddCurvesFor(DataRepository dataRepository, Action<DataColumn, Curve> action)
      {
         Chart.AddCurvesFor(dataRepository.AllButBaseGrid(), NameForColumn, _chartPresenterContext.DimensionFactory, action);
      }

      protected ParameterIdentificationRunResult RunResultWithBestError(
         IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults)
      {
         return parameterIdentificationResults.MinimumBy(x => x.TotalError);
      }

      protected void AddUsedObservedDataToChart()
      {
         _parameterIdentification.AllOutputMappings.GroupBy(x => x.FullOutputPath).Each(AddObservedDataForOutput);
      }
   }
}