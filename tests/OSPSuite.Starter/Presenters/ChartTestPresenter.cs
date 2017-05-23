using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Serialization;
using OSPSuite.Presentation.Settings;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Views;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Starter.Presenters
{
   public interface IChartTestPresenter : IPresenter<IChartTestView>, ICommandCollectorPresenter
   {
      void LoadChart();
      void SaveChart();
      void SaveSettings();
      void SaveChartWithDataWithoutValues();
      void LoadSettings();
      void LoadTemplate();
      void RefreshDisplay();
      void ReloadMenus();
      void RemoveDatalessCurves();
      void InitializeRepositoriesWithOriginalData();
      void AddObservations(int numberOfObservations);
      void AddCalculations(int numberOfCalculations);
      void ClearChart();
   }

   public class ChartTestPresenter : AbstractCommandCollectorPresenter<IChartTestView, IChartTestPresenter>, IChartTestPresenter
   {
      private readonly IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      private readonly IContainer _model;
      private readonly IDataRepositoryCreator _dataRepositoryCreator;
      private readonly IOSPSuiteXmlSerializerRepository _ospSuiteXmlSerializerRepository;
      private readonly DataPersistor _dataPersistor;
      private readonly IChartFromTemplateService _chartFromTemplateService;
      private readonly IChartTemplatePersistor _chartTemplatePersistor;
      private readonly IDimensionFactory _dimensionFactory;
      private ICache<string, DataRepository> _dataRepositories;

      public ChartTestPresenter(IChartTestView view, IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter, TestEnvironment testEnvironment, IDataColumnToPathElementsMapper dataColumnToPathColumnValuesMapper,
         IDataRepositoryCreator dataRepositoryCreator, IOSPSuiteXmlSerializerRepository ospSuiteXmlSerializerRepository, DataPersistor dataPersistor, IChartFromTemplateService chartFromTemplateService,
         IChartTemplatePersistor chartTemplatePersistor, IDimensionFactory dimensionFactory) : base(view)
      {
         _model = testEnvironment.Model.Root;
         _dataRepositories = new Cache<string, DataRepository>(repository => repository.Name);
         _chartEditorAndDisplayPresenter = chartEditorAndDisplayPresenter;
         _dataRepositoryCreator = dataRepositoryCreator;
         _ospSuiteXmlSerializerRepository = ospSuiteXmlSerializerRepository;
         _dataPersistor = dataPersistor;
         _chartFromTemplateService = chartFromTemplateService;
         _chartTemplatePersistor = chartTemplatePersistor;
         _dimensionFactory = dimensionFactory;
         _view.AddChartEditorView(chartEditorAndDisplayPresenter.EditorPresenter.View);
         _view.AddChartDisplayView(chartEditorAndDisplayPresenter.DisplayPresenter.View);

         AddSubPresenters(chartEditorAndDisplayPresenter);

         configureChartEditorPresenter(dataColumnToPathColumnValuesMapper);

         bindCurveChartToEditor();

         configureChartEditorEvents();
      }

      private void configureChartEditorEvents()
      {
         ChartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.Origin).Visible = true;

         ChartEditorPresenter.SetCurveNameDefinition(TestProgram.NameDefinition);
         ChartEditorPresenter.GetCurveOptionsColumnSettings(CurveOptionsColumns.InterpolationMode).Visible = false;

         ChartEditorPresenter.GetAxisOptionsColumnSettings(AxisOptionsColumns.NumberMode).Caption = "Number Representation";
         ChartEditorPresenter.GetAxisOptionsColumnSettings(AxisOptionsColumns.NumberMode).VisibleIndex = 1;

         ChartDisplayPresenter.DataSource = Chart;
         Chart.ChartSettings.DiagramBackColor = Color.White;
         ChartDisplayPresenter.DragOver += onChartDisplayDragOver;
         ChartEditorPresenter.DragOver += onChartDisplayDragOver;

         ReloadMenus();
      }

      public void InitializeRepositoriesWithOriginalData()
      {
         var newRepositories = _dataRepositoryCreator.CreateOriginalDataRepositories(_model).ToList();
         ChartEditorPresenter.AddDataRepositories(newRepositories);
         addNewRepositories(newRepositories);
      }

      private void addNewRepositories(IEnumerable<DataRepository> newRepositories)
      {
         _dataRepositories.AddRange(newRepositories);
      }

      public void AddObservations(int numberOfObservations)
      {
         var newRepositories = _dataRepositoryCreator.CreateObservationRepositories(numberOfObservations, _model, _dataRepositories.Count).ToList();
         ChartEditorPresenter.AddDataRepositories(newRepositories);
         addNewRepositories(newRepositories);
         addNewCurvesToChart(newRepositories);
         ChartEditorPresenter.SelectDataColumns();
      }

      private void addNewCurvesToChart(List<DataRepository> newRepositories)
      {
         newRepositories.Each(repository => repository.AllButBaseGrid().Each(column => ChartEditorPresenter.AddCurveForColumn(column.Id)));
      }

      public void AddCalculations(int numberOfCalculations)
      {
         var newRepositories = _dataRepositoryCreator.CreateCalculationRepositories(numberOfCalculations, _model, _dataRepositories.Count).ToList();
         ChartEditorPresenter.AddDataRepositories(newRepositories);
         addNewCurvesToChart(newRepositories);
         addNewRepositories(newRepositories);
      }

      public void ClearChart()
      {
         _dataRepositories.Each(repository =>
         {
            Chart.RemoveCurvesForDataRepository(repository);
            ChartEditorPresenter.RemoveDataRepository(repository);
         });
         _dataRepositories.Clear();
      }

      public IChartDisplayPresenter ChartDisplayPresenter => _chartEditorAndDisplayPresenter.DisplayPresenter;

      private void bindCurveChartToEditor()
      {

         Chart = new CurveChart
         {
            OriginText = Captions.ChartFingerprintDataFrom("Test Chart Project", "Test Chart Simulation", DateTime.Now.ToIsoFormat())
         };

         ChartEditorPresenter.DataSource = Chart;

         Chart.Title = "The Chart Title";
         Chart.ChartSettings.BackColor = Color.White;
      }

      public ICurveChart Chart { get; set; }

      private void configureChartEditorPresenter(IDataColumnToPathElementsMapper dataColumnToPathColumnValuesMapper)
      {
         ChartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.RepositoryName).GroupIndex = 0;
         ChartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.DimensionName).GroupIndex = 1;
         ChartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.Simulation).GroupIndex = -1;
         ChartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.TopContainer).Caption = "TopC";
         ChartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.Container).Caption = "Container";
         ChartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.BottomCompartment).VisibleIndex = 2;
         ChartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.Molecule).SortColumnName = BrowserColumns.OrderIndex.ToString();
         ChartEditorPresenter.SetDisplayQuantityPathDefinition(setQuantityPathDefinitions(_model, dataColumnToPathColumnValuesMapper));
         ChartEditorPresenter.SetShowDataColumnInDataBrowserDefinition(showDataColumnInDataBrowserDefinition);
      }

      public IChartEditorPresenter ChartEditorPresenter => _chartEditorAndDisplayPresenter.EditorPresenter;

      private static Func<DataColumn, PathElements> setQuantityPathDefinitions(IContainer model, IDataColumnToPathElementsMapper dataColumnToPathColumnValuesMapper)
      {
         return col => dataColumnToPathColumnValuesMapper.MapFrom(col, model.RootContainer);
      }

      private static bool showDataColumnInDataBrowserDefinition(DataColumn dataColumn)
      {
         if (dataColumn.Name.StartsWith("B")) return false;

         return true;
      }

      private static void onChartDisplayDragOver(object sender, DragEventArgs e)
      {
         e.Effect = e.Data.GetDataPresent(typeof(string)) ? DragDropEffects.Move : DragDropEffects.None;
      }

      public void SaveChart()
      {
         var fileName = getFileName(new SaveFileDialog());
         if (string.IsNullOrEmpty(fileName)) return;

         _dataPersistor.Save(_dataRepositories, fileName.Replace(".", "_d."));

         _dataPersistor.Save(Chart, fileName);

      }

      public void SaveSettings()
      {
         var fileName = getFileName(new SaveFileDialog());
         if (string.IsNullOrEmpty(fileName)) return;

         _dataPersistor.Save(_chartEditorAndDisplayPresenter.CreateSettings(), fileName);
      }

      public void SaveChartWithDataWithoutValues()
      {
         var fileName = getFileName(new SaveFileDialog());
         if (string.IsNullOrEmpty(fileName)) return;

         _chartTemplatePersistor.SerializeToFileBasedOn(Chart, fileName);
      }

      public void LoadSettings()
      {
         var fileName = getFileName(new OpenFileDialog());
         if (string.IsNullOrEmpty(fileName)) return;

         var settingsPersister = new DataPersistor(_ospSuiteXmlSerializerRepository);
         var settings = settingsPersister.Load<ChartEditorAndDisplaySettings>(fileName);
         _chartEditorAndDisplayPresenter.CopySettingsFrom(settings);
      }

      public void LoadTemplate()
      {
         var fileName = getFileName(new OpenFileDialog());
         if (string.IsNullOrEmpty(fileName)) return;

         var chartTemplate = _chartTemplatePersistor.DeserializeFromFile(fileName);
         ChartDisplayPresenter.DataSource = null;
         ChartEditorPresenter.DataSource = null;

         var chart = new CurveChart();
         _chartFromTemplateService.CurveNameDefinition = TestProgram.NameDefinition;
         _chartFromTemplateService.InitializeChartFromTemplate(chart, _dataRepositories["Rep Ex3"], chartTemplate, false);

         ChartEditorPresenter.DataSource = chart;
         ChartDisplayPresenter.DataSource = chart;
      }

      public void RefreshDisplay()
      {
         ChartDisplayPresenter.Refresh();
      }


      public void ReloadMenus()
      {
         ChartEditorPresenter.ClearButtons();
         var showSelected = new TestProgramButton("Show Selected DataColumns", ChartEditorPresenter.SelectDataColumns);
         var saveAsTemplate = new TestProgramButton("Save as Chart Template", onChartSave);

         ChartEditorPresenter.AddButton(showSelected);
         ChartEditorPresenter.AddButton(saveAsTemplate);


         var groupMenu = CreateSubMenu.WithCaption("Layouts");
         EnumHelper.AllValuesFor<Layouts>().Each(
            layout => groupMenu.AddItem(new TestProgramButton(layout.ToString(), onLayouts)));

         groupMenu.AddItem(new TestProgramButton("Dummy", () => { }).AsGroupStarter());

         ChartEditorPresenter.AddButton(groupMenu);

         ChartEditorPresenter.AddUsedInMenuItem();

         ChartEditorPresenter.AddChartTemplateMenu(new SimulationSettings(), template => { });
      }

      public void RemoveDatalessCurves()
      {
         Chart.RemoveDatalessCurves();
      }

      private void onLayouts()
      {
         
      }

      private void onChartSave()
      {
         
      }

      public void LoadChart()
      {
         var fileName = getFileName(new OpenFileDialog());
         if (string.IsNullOrEmpty(fileName)) return;

         var dataPersitor = new DataPersistor(_ospSuiteXmlSerializerRepository);
         _dataRepositories = dataPersitor.Load<ICache<string, DataRepository>>(fileName.Replace(".", "_d."), _dimensionFactory);

         Chart = dataPersitor.Load<ICurveChart>(fileName, _dimensionFactory, _dataRepositories);

         ChartEditorPresenter.DataSource = null;
         ChartEditorPresenter.ClearDataRepositories();

         ChartEditorPresenter.AddDataRepositories(_dataRepositories);
         ChartEditorPresenter.DataSource = Chart;

         ChartEditorPresenter.DataSource = Chart;
      }

      private string getFileName(FileDialog fileDialog)
      {
         fileDialog.InitialDirectory = @"c:\Temp\DataChartTest";
         fileDialog.Filter = "xml files (*.xml)|*.xml";
         fileDialog.FilterIndex = 2;
         fileDialog.RestoreDirectory = true;

         var fileName = fileDialog.FileName;

         if (fileDialog.ShowDialog() != DialogResult.OK) fileName = string.Empty;
         if (!fileDialog.CheckFileExists) fileName = string.Empty;

         return fileName;
      }
   }
}
