﻿using System;
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
   public interface ISimulationVsObservedDataPresenter : ISimulationAnalysisPresenter
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

         if (ChartIsBeingUpdated)
         {
            UpdateTemplateFromChart();
            ClearChartAndDataRepositories();
         }
         else
            UpdateCacheColor();

         if (!_simulation.ResultsDataRepository.IsNull())
         {
            UpdateAnalysis();
            ChartEditorPresenter.SetOutputMappings(_simulation.OutputMappings);
         }

         updateRunResults();
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

      protected void AddUsedObservedDataToChart()
      {
         _simulation.OutputMappings.All.GroupBy(x => x.FullOutputPath).Each(AddObservedDataForOutput);
      }
      private void clearRunResults()
      {
         RemoveDataRepositoriesFromEditor(new List<DataRepository>() { _resultsRepository });
         _resultsRepository.Clear();
      }

      private void updateRunResults()
      {
         clearRunResults();
         AddRunResultToChart();
      }

      protected abstract void AddRunResultToChart();
   }
}