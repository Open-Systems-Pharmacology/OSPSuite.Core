using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationRunAnalysisPresenter : ISimulationAnalysisPresenter
   {
   }

   public abstract class SimulationRunAnalysisPresenter<TChart> : SimulationAnalysisChartPresenter<TChart, ISimulationRunAnalysisView, ISimulationRunAnalysisPresenter>, ISimulationRunAnalysisPresenter where TChart : ChartWithObservedData, ISimulationAnalysis
   {
      protected ISimulation _simulation;
      private readonly Cache<string, Color> _colorCache = new Cache<string, Color>(onMissingKey: x => Color.Black);
      protected CurveChartTemplate _chartTemplate;
      public IReadOnlyList<IndividualResults> AllRunResults { get; private set; }
      protected List<DataRepository> _resultsRepositories = new List<DataRepository>();

      protected SimulationRunAnalysisPresenter(ISimulationRunAnalysisView view, ChartPresenterContext chartPresenterContext, ApplicationIcon icon, string presentationKey) : base(view, chartPresenterContext)
      {
         _view.SetAnalysisView(chartPresenterContext.EditorAndDisplayPresenter.BaseView);
         _view.ApplicationIcon = icon;
         PresentationKey = presentationKey;
         PostEditorLayout = setColumnGroupingsAndVisibility;
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
            updateCacheColor();

         if (_simulation.Results.Any())
         {
            UpdateAnalysisBasedOn(_simulation.Results.IndividualResultsAsArray());
            ChartEditorPresenter.SetOutputMappings(_simulation.OutputMappings);
         }

         Refresh();
      }

      protected virtual void UpdateTemplateFromChart()
      {
         _chartTemplate = _chartPresenterContext.TemplatingTask.TemplateFrom(Chart, validateTemplate: false);
      }

      protected virtual void UpdateChartFromTemplate()
      {
         if (_chartTemplate == null)
            return;

         LoadTemplate(_chartTemplate, warnIfNumberOfCurvesAboveThreshold: false);
      }

      protected override void ConfigureColumns()
      {
         base.ConfigureColumns();
         setColumnGroupingsAndVisibility();
      }

      private void showSimulationColumn()
      {
         var simulationColumnSettings = Column(BrowserColumns.Simulation);
         simulationColumnSettings.Visible = true;
         simulationColumnSettings.VisibleIndex = 0;
         ChartEditorPresenter.ApplyColumnSettings(simulationColumnSettings);
      }

      private void groupByCategoryColumn()
      {
         var categoryColumnSettings = Column(BrowserColumns.Category);
         categoryColumnSettings.Visible = false;
         categoryColumnSettings.GroupIndex = 1;
         ChartEditorPresenter.ApplyColumnSettings(categoryColumnSettings);
      }

      private void setColumnGroupingsAndVisibility()
      {
         ChartEditorPresenter.SetGroupRowFormat(GridGroupRowFormats.HideColumnName);
         showSimulationColumn();
         groupByCategoryColumn();
      }

      private void updateCacheColor()
      {
         _colorCache.Clear();
         Chart.Curves.Each(x => _colorCache[x.yData.PathAsString] = x.Color);
      }

      protected virtual void ClearChartAndDataRepositories()
      {
         Chart.Clear();
         ChartEditorPresenter.RemoveAllDataRepositories();
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

      protected void AddCurvesFor(IEnumerable<DataColumn> columns, Action<DataColumn, Curve> action)
      {
         Chart.AddCurvesFor(columns, NameForColumn, _chartPresenterContext.DimensionFactory, action);
      }


      protected void SelectColorForCalculationColumn(DataColumn calculationColumn)
      {
         SelectColorForPath(calculationColumn.PathAsString);
      }

      protected void SelectColorForPath(string path)
      {
         if (!_colorCache.Contains(path))
            _colorCache[path] = Chart.SelectNewColor();
      }

      protected void UpdateColorForCalculationColumn(Curve curve, DataColumn calculationColumn)
      {
         UpdateColorForPath(curve, calculationColumn.PathAsString);
      }

      protected void UpdateColorForPath(Curve curve, string path)
      {
         curve.Color = _colorCache[path];
      }

      private void addObservedDataForOutput(IGrouping<string, OutputMapping> outputMappingsByOutput)
      {
         var outputPath = outputMappingsByOutput.Key;
         SelectColorForPath(outputPath);

         AddDataRepositoriesToEditor(outputMappingsByOutput.Select(x => x.WeightedObservedData.ObservedData));

         AddCurvesFor(outputMappingsByOutput.SelectMany(x => x.WeightedObservedData.ObservedData.ObservationColumns()),
            (column, curve) =>
            {
               UpdateColorForPath(curve, outputPath);
               curve.VisibleInLegend = false;
            });
      }

      protected void AddUsedObservedDataToChart()
      {
         _simulation.OutputMappings.All.GroupBy(x => x.FullOutputPath).Each(addObservedDataForOutput);
      }

      protected abstract void AddRunResultToChart();
   }
}
