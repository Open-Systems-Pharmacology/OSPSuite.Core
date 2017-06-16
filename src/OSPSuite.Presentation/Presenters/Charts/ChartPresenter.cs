using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartPresenter<TChart> : IPresenter,
      IPresenterWithSettings,
      IListener<ChartTemplatesChangedEvent>
   {
      /// <summary>
      ///    return underlying chart object
      /// </summary>
      TChart Chart { get; }

      void InitializeAnalysis(TChart chart);
   }

   public abstract class ChartPresenter<TChart, TView, TPresenter> : AbstractPresenter<TView, TPresenter>, IChartPresenter<TChart>
      where TView : class, IView<TPresenter>
      where TPresenter : IPresenter
      where TChart : ChartWithObservedData
   {
      protected readonly ChartPresenterContext _chartPresenterContext;
      protected DefaultPresentationSettings _settings;
      public TChart Chart { get; private set; }
      private IWithChartTemplates _withChartTemplates;
      public string PresentationKey { get; protected set; }
      protected IChartEditorPresenter ChartEditorPresenter => _chartPresenterContext.ChartEditorAndDisplayPresenter.EditorPresenter;
      protected IChartDisplayPresenter ChartDisplayPresenter => _chartPresenterContext.ChartEditorAndDisplayPresenter.DisplayPresenter;

      protected ChartPresenter(TView view, ChartPresenterContext chartPresenterContext) : base(view)
      {
         _chartPresenterContext = chartPresenterContext;
         AddSubPresenters(_chartPresenterContext.ChartEditorAndDisplayPresenter);
         _settings = new DefaultPresentationSettings();
         ChartEditorPresenter.SetCurveNameDefinition(NameForColumn);
         ChartEditorPresenter.SetDisplayQuantityPathDefinition(displayQuantityPathDefinition);
         ChartEditorPresenter.ColumnSettingsChanged += columnSettingsChanged;
         ChartEditorPresenter.ChartChanged += ChartChanged;
         ConfigureColumns();
      }

      public virtual void LoadSettingsForSubject(IWithId subject)
      {
         _settings = _chartPresenterContext.PresenterSettingsTask.PresentationSettingsFor<DefaultPresentationSettings>(this, subject);
      }

      public virtual void InitializeAnalysis(TChart chart)
      {
         Chart = chart;

         AddDataRepositoriesToEditor(Chart.AllObservedData());

         updateViewCaptionFromChart();

         InitEditorLayout();
      }

      protected void BindChartToEditors()
      {
         ChartDisplayPresenter.Edit(Chart);
         ChartEditorPresenter.Edit(Chart);
      }

      private void addTemplateButtons()
      {
         var chartLayoutButton = _chartPresenterContext.ChartEditorAndDisplayPresenter.ChartLayoutButton;

         ChartEditorPresenter.AddButton(chartLayoutButton);
      }

      protected CurveChartTemplate DefaultChartTemplate => _withChartTemplates?.DefaultChartTemplate;

      protected void UpdateTemplatesBasedOn(IWithChartTemplates withChartTemplates)
      {
         _withChartTemplates = withChartTemplates;
         resetMenus();
      }

      private void resetMenus()
      {
         ClearButtons();
         if (_withChartTemplates != null)
            ChartEditorPresenter.AddChartTemplateMenu(_withChartTemplates, template => LoadTemplate(template));

         AddAllButtons();
      }

      protected void AddAllButtons()
      {
         addTemplateButtons();

         ChartEditorPresenter.AddUsedInMenuItem();
      }

      public void Handle(ChartTemplatesChangedEvent eventToHandle)
      {
         if (canHandle(eventToHandle))
            resetMenus();
      }

      private bool canHandle(ChartTemplatesChangedEvent eventToHandle)
      {
         return Equals(eventToHandle.WithChartTemplates, _withChartTemplates);
      }

      protected void LoadTemplate(CurveChartTemplate curveChartTemplate, bool warnIfNumberOfCurvesAboveThreshold = true)
      {
         _chartPresenterContext.TemplatingTask.InitializeChartFromTemplate(
            Chart,
            ChartEditorPresenter.AllDataColumns,
            curveChartTemplate,
            NameForColumn, warnIfNumberOfCurvesAboveThreshold);
      }

      /// <summary>
      ///    Add obsverved Data to be displayed on the graph. if the showData flag is set to true, the data will be displayed as
      ///    well
      /// </summary>
      protected virtual void AddObservedData(IReadOnlyList<DataRepository> observedData, bool asResultOfDragAndDrop)
      {
      }

      protected virtual void ChartChanged()
      {
         updateViewCaptionFromChart();
         NotifyProjectChanged();
      }

      private void updateViewCaptionFromChart()
      {
         _view.Caption = Chart.Name;
      }

      protected void AddDataRepositoriesToEditor(IEnumerable<DataRepository> dataRepositories) => ChartEditorPresenter.AddDataRepositories(dataRepositories);

      protected void RemoveDataRepositoriesFromEditor(IEnumerable<DataRepository> dataRepositories) => ChartEditorPresenter.RemoveDataRepositories(dataRepositories);

      private PathElements displayQuantityPathDefinition(DataColumn dataColumn)
      {
         var simulationForDataColumn = SimulationFor(dataColumn);
         return _chartPresenterContext.DataColumnToPathElementsMapper.MapFrom(dataColumn, simulationForDataColumn?.Model.Root);
      }

      protected abstract ISimulation SimulationFor(DataColumn dataColumn);

      protected virtual string NameForColumn(DataColumn dataColumn)
      {
         return _chartPresenterContext.CurveNamer.CurveNameForColumn(SimulationFor(dataColumn), dataColumn);
      }

      public virtual void Clear()
      {
         ChartEditorPresenter.Clear();
         ChartDisplayPresenter.Clear();
         ChartEditorPresenter.ColumnSettingsChanged -= columnSettingsChanged;
         ChartEditorPresenter.ChartChanged -= ChartChanged;
         ChartEditorPresenter.SetCurveNameDefinition(null);
         ChartEditorPresenter.SetDisplayQuantityPathDefinition(null);
         ChartEditorPresenter.ClearButtons();
         _withChartTemplates = null;

         //necessary to dispose view here that was added dynamically to the container view and might not be disposed
         View.Dispose();
      }

      protected virtual void ConfigureColumns()
      {
         Column(BrowserColumns.RepositoryName).GroupIndex = -1;
         Column(BrowserColumns.Simulation).Visible = false;
         Column(BrowserColumns.TopContainer).Visible = false;
         Column(BrowserColumns.Container).Visible = false;
         Column(BrowserColumns.BottomCompartment).Visible = false;
         Column(BrowserColumns.Molecule).Visible = false;
         Column(BrowserColumns.Name).Visible = false;
         Column(BrowserColumns.ColumnId).Visible = false;
         Column(BrowserColumns.BaseGridName).Visible = false;
         Column(BrowserColumns.RepositoryName).Visible = false;
         Column(BrowserColumns.Category).Visible = false;
         Column(BrowserColumns.Date).Visible = false;
         Column(BrowserColumns.DimensionName).Visible = false;
         Column(BrowserColumns.Origin).Visible = false;
         Column(BrowserColumns.QuantityName).Visible = false;

         Column(CurveOptionsColumns.ShowLowerLimitOfQuantification).Visible = false;
         Column(CurveOptionsColumns.InterpolationMode).Visible = false;
         Column(CurveOptionsColumns.xData).Visible = false;
         Column(CurveOptionsColumns.yData).Visible = false;
         Column(CurveOptionsColumns.yAxisType).Visible = true;
         Column(CurveOptionsColumns.Name).VisibleIndex = 0;
         Column(CurveOptionsColumns.Color).VisibleIndex = 1;
         Column(CurveOptionsColumns.LineStyle).VisibleIndex = 2;
         Column(CurveOptionsColumns.Symbol).VisibleIndex = 3;
         Column(CurveOptionsColumns.yAxisType).VisibleIndex = 4;
         Column(CurveOptionsColumns.Visible).VisibleIndex = 5;

         Column(AxisOptionsColumns.GridLines).Visible = true;
         Column(AxisOptionsColumns.Dimension).Visible = false;
         Column(AxisOptionsColumns.UnitName).Visible = true;
      }

      protected virtual void InitEditorLayout()
      {
         _chartPresenterContext.EditorLayoutTask.InitFromUserSettings(_chartPresenterContext.ChartEditorAndDisplayPresenter);
         AddAllButtons();
      }

      private void columnSettingsChanged(IReadOnlyCollection<GridColumnSettings> gridColumnSettings)
      {
         NotifyProjectChanged();
      }

      protected virtual void NotifyProjectChanged()
      {
         _chartPresenterContext.ProjectRetriever.CurrentProject.HasChanged = true;
      }

      protected GridColumnSettings Column(BrowserColumns browserColumns) => ChartEditorPresenter.DataBrowserColumnSettingsFor(browserColumns);

      protected GridColumnSettings Column(CurveOptionsColumns curveOptionsColumns) => ChartEditorPresenter.CurveOptionsColumnSettingsFor(curveOptionsColumns);

      protected GridColumnSettings Column(AxisOptionsColumns axisOptionsColumns) => ChartEditorPresenter.AxisOptionsColumnSettingsFor(axisOptionsColumns);

      protected void ClearButtons()
      {
         ChartEditorPresenter.ClearButtons();
      }
   }
}