using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public abstract class ParameterIdentificationSingleRunAnalysisPresenter<TChart> : ParameterIdentificationAnalysisChartPresenter<TChart, IParameterIdentificationSingleRunAnalysisView, IParameterIdentificationSingleRunAnalysisPresenter>, IParameterIdentificationSingleRunAnalysisPresenter where TChart : ChartWithObservedData, ISimulationAnalysis
   {
      private ParameterIdentificationRunResult _selectedRunResults;
      public IReadOnlyList<ParameterIdentificationRunResult> AllRunResults { get; private set; }
      protected List<DataRepository> _resultsRepositories = new List<DataRepository>();

      protected ParameterIdentificationSingleRunAnalysisPresenter(IParameterIdentificationSingleRunAnalysisView view, ChartPresenterContext chartPresenterContext, ApplicationIcon icon, string presentationKey) :
         base(view, chartPresenterContext, icon, presentationKey)
      {
      }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults)
      {
         AllRunResults = _parameterIdentification.Results.OrderBy(x => x.TotalError).ToList();
         _view.CanSelectRunResults = _isMultipleRun;

         if (!AllRunResults.Any()) return;

         updateSelectedRunResults(AllRunResults.First());

         _view.BindToSelectedRunResult();
      }

      public ParameterIdentificationRunResult SelectedRunResults
      {
         get => _selectedRunResults;
         set
         {
            UpdateTemplateFromChart();
            updateSelectedRunResults(value);
            UpdateChartFromTemplate();
         }
      }

      private void updateSelectedRunResults(ParameterIdentificationRunResult runResult)
      {
         _selectedRunResults = runResult;
         updateAnalysis();
      }

      protected void AddResultRepositoryToEditor(DataRepository dataRepository)
      {
         AddResultRepositoriesToEditor(new[] {dataRepository});
      }

      protected void AddResultRepositoriesToEditor(IReadOnlyList<DataRepository> dataRepositories)
      {
         _resultsRepositories.AddRange(dataRepositories);
         AddDataRepositoriesToEditor(dataRepositories);
      }

      private void clearRunResults()
      {
         RemoveDataRepositoriesFromEditor(_resultsRepositories);
         _resultsRepositories.Clear();
      }

      private void updateAnalysis()
      {
         clearRunResults();
         AddRunResultToChart(SelectedRunResults);
      }

      protected abstract void AddRunResultToChart(ParameterIdentificationRunResult selectedRunResults);
   }
}