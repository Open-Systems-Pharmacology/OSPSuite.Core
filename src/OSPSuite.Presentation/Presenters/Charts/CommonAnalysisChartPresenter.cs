using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ICommonAnalysisChartPresenter : ISimulationAnalysisPresenter
   {
   }

   public abstract class CommonAnalysisChartPresenter<TChart, TView, TPresenter> : SimulationAnalysisChartPresenter<TChart, TView, TPresenter>, ICommonAnalysisChartPresenter
   where TChart : ChartWithObservedData, ISimulationAnalysis
   where TView : class, IView<TPresenter>
   where TPresenter : ISimulationAnalysisPresenter
   {
      protected readonly Cache<string, Color> _colorCache = new Cache<string, Color>(onMissingKey: x => Color.Black);
      protected CurveChartTemplate _chartTemplate;

      protected CommonAnalysisChartPresenter(TView view, ChartPresenterContext chartPresenterContext) : base(view, chartPresenterContext)
      {
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
         SetColumnGroupingsAndVisibility();
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

      protected void SetColumnGroupingsAndVisibility()
      {
         ChartEditorPresenter.SetGroupRowFormat(GridGroupRowFormats.HideColumnName);
         showSimulationColumn();
         groupByCategoryColumn();
      }

      protected void UpdateCacheColor()
      {
         _colorCache.Clear();
         Chart.Curves.Each(x => _colorCache[x.yData.PathAsString] = x.Color);
      }

      protected virtual void ClearChartAndDataRepositories()
      {
         Chart.Clear();
         ChartEditorPresenter.RemoveAllDataRepositories();
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

      protected void AddObservedDataForOutput(IGrouping<string, OutputMapping> outputMappingsByOutput)
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

   }
}
