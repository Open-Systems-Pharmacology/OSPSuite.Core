using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationRunAnalysisPresenter : ISimulationAnalysisPresenter
   {
   }

   public abstract class SimulationRunAnalysisPresenter<TChart> : CommonAnalysisChartPresenter<TChart, ISimulationRunAnalysisView, ISimulationRunAnalysisPresenter>, ISimulationRunAnalysisPresenter 
      where TChart : ChartWithObservedData, ISimulationAnalysis
   {
      protected ISimulation _simulation;
      public IReadOnlyList<IndividualResults> AllRunResults { get; private set; }
      protected List<DataRepository> _resultsRepositories = new List<DataRepository>();

      protected SimulationRunAnalysisPresenter(ISimulationRunAnalysisView view, ChartPresenterContext chartPresenterContext, ApplicationIcon icon, string presentationKey) : base(view, chartPresenterContext)
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
         _simulation = analysable.DowncastTo<ISimulation>();

         if (ChartIsBeingUpdated)
         {
            UpdateTemplateFromChart();
            ClearChartAndDataRepositories();
         }
         else
            UpdateCacheColor();

         if (_simulation.Results.Any())
         {
            UpdateAnalysisBasedOn(_simulation.Results.IndividualResultsAsArray());
            ChartEditorPresenter.SetOutputMappings(_simulation.OutputMappings);
         }

         updateAnalysis();
         Refresh();
      }

      protected virtual void UpdateAnalysisBasedOn(IReadOnlyList<IndividualResults> simulationResults)
      {
         AllRunResults = _simulation.Results.AllIndividualResults.ToList();//.OrderBy(x => x.TotalError).ToList();

         if (!AllRunResults.Any()) return;

         //updateSelectedRunResults(AllRunResults.First());

         //_view.BindToSelectedRunResult();
      }

      protected void AddResultRepositoriesToEditor(IReadOnlyList<DataRepository> dataRepositories)
      {
         _resultsRepositories.AddRange(dataRepositories);
         AddDataRepositoriesToEditor(dataRepositories);
      }

      protected override ISimulation SimulationFor(DataColumn dataColumn)
      {
         if (string.IsNullOrEmpty(dataColumn.PathAsString))
            return null;

         return _simulation; //here we only have one simulation - so nothing more to search for.
      }

      protected void AddCurvesFor(DataRepository dataRepository, Action<DataColumn, Curve> action)
      {
         Chart.AddCurvesFor(dataRepository.AllButBaseGrid(), NameForColumn, _chartPresenterContext.DimensionFactory, action);
      }

      protected void AddUsedObservedDataToChart()
      {
         _simulation.OutputMappings.All.GroupBy(x => x.FullOutputPath).Each(AddObservedDataForOutput);
      }
      private void clearRunResults()
      {
         RemoveDataRepositoriesFromEditor(_resultsRepositories);
         _resultsRepositories.Clear();
      }

      private void updateAnalysis()
      {
         clearRunResults();
         AddRunResultToChart();
      }

      protected abstract void AddRunResultToChart();
   }
}
