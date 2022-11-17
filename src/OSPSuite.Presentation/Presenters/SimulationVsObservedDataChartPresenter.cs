using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationVsObservedDataPresenter : ISimulationAnalysisPresenter, IListener<SimulationOutputMappingsChangedEvent>
   {
   }

   public abstract class SimulationVsObservedDataChartPresenter<TChart> : SimulationAnalysisChartPresenter<TChart, ISimulationVsObservedDataView, ISimulationVsObservedDataPresenter>, ISimulationVsObservedDataPresenter 
      where TChart : ChartWithObservedData, ISimulationAnalysis
   {
      protected ISimulation _simulation;
      protected DataRepository _resultsRepository = new DataRepository();

      protected SimulationVsObservedDataChartPresenter(ISimulationVsObservedDataView view, ChartPresenterContext chartPresenterContext, ApplicationIcon icon, string presentationKey) : base(view, chartPresenterContext)
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

         ClearChartAndDataRepositories();
         UpdateCacheColor();

         if (!_simulation.ResultsDataRepository.IsNull())
         {
            UpdateAnalysis();
            ChartEditorPresenter.SetOutputMappings(_simulation.OutputMappings);
            AddRunResultToChart();
         }

         Refresh();
      }

      protected abstract void UpdateAnalysis();

      protected void AddResultRepositoryToEditor(DataRepository dataRepository)
      {
         _resultsRepository = dataRepository;
         AddDataRepositoriesToEditor(new List<DataRepository>() { dataRepository });
      }

      protected override ISimulation SimulationFor(DataColumn dataColumn)
      {
         return string.IsNullOrEmpty(dataColumn.PathAsString) ? null : _simulation;
      }

      protected void AddCurvesFor(DataRepository dataRepository, Action<DataColumn, Curve> action)
      {
         Chart.AddCurvesFor(dataRepository.AllButBaseGrid(), NameForColumn, _chartPresenterContext.DimensionFactory, action);
      }

      protected override string NameForColumn(DataColumn dataColumn)
      {
         // In the case of these charts that are for a single simulation, we just need to override the default implementation to not include the simulation name
         return _chartPresenterContext.CurveNamer.CurveNameForColumn(SimulationFor(dataColumn), dataColumn, addSimulationName: false);
      }

      protected void AddUsedObservedDataToChart()
      {
         _simulation.OutputMappings.All.GroupBy(x => x.FullOutputPath).Each(AddObservedDataForOutput);
      }

      protected abstract void AddRunResultToChart();
      
      public void Handle(SimulationOutputMappingsChangedEvent eventToHandle)
      {
         updateForAnalysableEvents(eventToHandle);
      }

      private void updateForAnalysableEvents(AnalysableEvent eventToHandle)
      {
         if (Equals(eventToHandle.Analysable, _simulation))
            UpdateAnalysisBasedOn(_simulation);
      }
   }
}
