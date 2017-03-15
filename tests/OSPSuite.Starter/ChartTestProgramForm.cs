using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Serialization;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Starter
{
   public partial class ChartTestProgramForm : Form, IShell
   {
      private readonly IDimensionFactory _dimensionFactory;
      private ICache<string, DataRepository> _dataRepositories;
      private ICurveChart _chart;
      private readonly IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      private readonly ChartTemplatePersistor _chartTemplatePersistor;
      private readonly IChartDisplayPresenter _chartDisplayPresenter;
      private readonly IChartEditorPresenter _chartEditorPresenter;
      private readonly IKeyPathMapper _keyPathMapper;
      private readonly IDialogCreator _dialogCreator;

      public ChartTestProgramForm()
      {
         InitializeComponent();
         _btnSaveChart.Click += onSaveClick;
         _btnSaveChartWithDataWithoutValues.Click += onSaveClick;
         _btnSaveSettings.Click += onSaveClick;
         _btnLoadChart.Click += onLoadClick;
         _btnLoadSettings.Click += onLoadClick;
         _btnLoadTemplate.Click += onLoadClick;
         CaptionChanged += delegate {  };
      }

      public ChartTestProgramForm(IDimensionFactory dimensionFactory, ICache<string, DataRepository> dataRepositories, ICurveChart chart,
                             IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter, ISimpleChartPresenter simpleChartPresenter)
         : this()
      {
         _simpleChartPresenter = simpleChartPresenter;
         _dimensionFactory = dimensionFactory;
         _dataRepositories = dataRepositories;
         _chart = chart;
         _chartEditorAndDisplayPresenter = chartEditorAndDisplayPresenter;
         _chartEditorAndDisplayPresenter.SetNoCurvesSelectedHint("No curves are selected");

         _chartDisplayPresenter = chartEditorAndDisplayPresenter.DisplayPresenter;
         _chartEditorPresenter = chartEditorAndDisplayPresenter.EditorPresenter;

         _spSuiteXmlSerializerRepository = new OSPSuiteXmlSerializerRepository();
         _spSuiteXmlSerializerRepository.PerformMapping();

         ICurveChartToCurveChartTemplateMapper curveChartToCurveChartTemplateMapper = new CurveChartToCurveChartTemplateMapper(null);

         var objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _keyPathMapper = new KeyPathMapper(new EntityPathResolver(objectPathFactory), objectPathFactory);
         var objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
         var withIdRepository = IoC.Resolve<IWithIdRepository>();
         _dialogCreator = IoC.Resolve<IDialogCreator>();
         _chartTemplatePersistor = new ChartTemplatePersistor(curveChartToCurveChartTemplateMapper, _spSuiteXmlSerializerRepository, _dimensionFactory, objectBaseFactory, withIdRepository);
         reloadMenus();
      }

      public static string NameDefinition(DataColumn column)
      {
         string name = column.Name + "(" + column.Repository.Name + ")";
         string path = "";
         for (int i = 2; i < column.QuantityInfo.Path.Count() - 1; i++) path += column.QuantityInfo.Path.ElementAt(i) + "/";
         return path + name;
      }

      private void btnAddRepositoryCalc1Click(object sender, EventArgs e)
      {
         _chartEditorPresenter.AddDataRepository(_dataRepositories["Rep Calc1"]);
      }

      private void btnAddRepositoryCalc2Click(object sender, EventArgs e)
      {
         _chartEditorPresenter.AddDataRepository(_dataRepositories["Rep Calc2"]);
      }

      private void btnAddRepositoriesFromImportClick(object sender, EventArgs e)
      {

      }

      private void btnClearRepositoryCalc2Click(object sender, EventArgs e)
      {
         _chart.RemoveCurvesForDataRepository(_dataRepositories["Rep Calc2"]); 
         _chartEditorPresenter.RemoveDataRepository(_dataRepositories["Rep Calc2"]);
         _dataRepositories["Rep Calc2"].Clear();
         _dataRepositories.Remove("Rep Calc2");
      }

      private void btnAddCurveToChartClick(object sender, EventArgs e)
      {
         var curve = _chart.CreateCurve(_dataRepositories["Rep Calc2"].ElementAt(0), _dataRepositories["Rep Calc2"].ElementAt(2), "RepCalc2 El(0,2)", _dimensionFactory);
         curve.Description = "This is a description";
         _chart.AddCurve(curve);
      }

      private void btnRemoveFirstCurveFromChartClick(object sender, EventArgs e)
      {
         _chart.Curves.Remove(_chart.Curves.First().Id);
      }

      private void btnChartRemoveDatalessCurvesClick(object sender, EventArgs e)
      {
         _chart.RemoveDatalessCurves();
      }

      private void btnRefreshDisplayClick(object sender, EventArgs e)
      {
        
         foreach (var col in _dataRepositories["Rep Calc2"])
         {
            if (!col.IsBaseGrid())
               col.InternalValues[4] = col.InternalValues[4] * 1000;
         }
         _chartDisplayPresenter.Refresh();
      }

      private void btnRefreshEditorClick(object sender, EventArgs e)
      {
         const string exSource = "Experimental Study 1A";
         var exDate = new DateTime(2009, 10, 26);

         var cols = _dataRepositories["Rep Ex3"].ToArray();
         var removeColumn = cols[1];
         var baseGrid = removeColumn.BaseGrid;
         _dataRepositories["Rep Ex3"].Remove(removeColumn);

         var updateColumn = cols[2];
         updateColumn.Name += "updated";

         var root = TestProgram.TestEnvironment.Model.Root;
         var organism = root.GetSingleChildByName<IContainer>("Organism");
         var bone = organism.GetSingleChildByName<IContainer>("Bone");
         var q = bone.GetSingleChildByName<IQuantity>("Q");


         var newColumn = new DataColumn("Spec1A" + DateTime.Now.Second, q.Dimension, baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, q.Dimension.DefaultUnitName, exDate, exSource, "Patient A", 320), 
            QuantityInfo = Helper.CreateQuantityInfo(q), Values = new[] {3F, 2F, 1F}
         };
         _dataRepositories["Rep Ex3"].Add(newColumn);
         _dataRepositories["Rep Ex3"].Name = "Rep Ex3 New";
         _chartEditorPresenter.RefreshDataRepository(_dataRepositories["Rep Ex3"]);
      }

      private void onSaveClick(object sender, EventArgs e)
      {
         var fileDialog = new SaveFileDialog
         {
            InitialDirectory = @"c:\Temp\DataChartTest", 
            Filter = "xml files (*.xml)|*.xml", 
            FilterIndex = 2, 
            RestoreDirectory = true, 
            AddExtension = true
         };

         if (fileDialog.ShowDialog() != DialogResult.OK) return;
         try
         {
            if (sender == _btnSaveChart)
            {
               var dataXmlPersister = new DataPersistor(_spSuiteXmlSerializerRepository);
               dataXmlPersister.Save(_dataRepositories, fileDialog.FileName.Replace(".", "_d."));

               var chartPersister = new DataPersistor(_spSuiteXmlSerializerRepository);
               chartPersister.Save(_chart, fileDialog.FileName);
            }
            else if (sender == _btnSaveSettings)
            {
               var settingsPersister = new DataPersistor(_spSuiteXmlSerializerRepository);
               settingsPersister.Save(_chartEditorAndDisplayPresenter.CreateSettings(), fileDialog.FileName);
            }
            else if (sender == _btnSaveChartWithDataWithoutValues)
            {
               _chartTemplatePersistor.SerializeToFileBasedOn(_chart, fileDialog.FileName);
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void onLoadClick(object sender, EventArgs e)
      {
         var fileDialog = new OpenFileDialog
         {
            InitialDirectory = @"c:\Temp\DataChartTest", 
            Filter = "xml files (*.xml)|*.xml", 
            FilterIndex = 2, 
            RestoreDirectory = true
         };

         if (fileDialog.ShowDialog() != DialogResult.OK) return;
         if (!fileDialog.CheckFileExists) return;

         try
         {
            if (sender == _btnLoadChart)
            {
               var dataPersitor = new DataPersistor (_spSuiteXmlSerializerRepository);
               _dataRepositories = dataPersitor.Load<ICache<string, DataRepository>>(fileDialog.FileName.Replace(".", "_d."), _dimensionFactory);

               _chart = dataPersitor.Load<ICurveChart>(fileDialog.FileName, _dimensionFactory, _dataRepositories);

               _chartEditorPresenter.DataSource = null;
               _chartEditorPresenter.ClearDataRepositories();

               _chartEditorPresenter.AddDataRepositories(_dataRepositories);
               _chartEditorPresenter.DataSource = _chart;

               _chartDisplayPresenter.DataSource = _chart;
            }
            else if (sender == _btnLoadSettings)
            {
               var settingsPersister = new DataPersistor(_spSuiteXmlSerializerRepository);
               var settings = settingsPersister.Load<ChartEditorAndDisplaySettings>(fileDialog.FileName);
               _chartEditorAndDisplayPresenter.CopySettingsFrom(settings);
            }
            else if (sender == _btnLoadTemplate)
            {
               var chartTemplate = _chartTemplatePersistor.DeserializeFromFile(fileDialog.FileName);
               _chartDisplayPresenter.DataSource = null;
               _chartEditorPresenter.DataSource = null;

               var chart = new CurveChart();
                     
               var chartFromTemplateService = new ChartFromTemplateService(_dimensionFactory,_keyPathMapper,_dialogCreator, null)
               {
                  CurveNameDefinition = TestProgram.NameDefinition
               };
               chartFromTemplateService.InitializeChartFromTemplate(chart, _dataRepositories["Rep Ex3"], chartTemplate, false);

               _chartEditorPresenter.DataSource = chart;
               _chartDisplayPresenter.DataSource = chart;
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void simpleChartTestClick(object sender, EventArgs e)
      {
         var presenter = IoC.Resolve<ISimpleChartPresenter>();
         presenter.LogLinSelectionEnabled = true;
         var repository = new DataRepository();
         var dimFactory = IoC.Resolve<IDimensionFactory>();

         var basegrid = new BaseGrid("basegrid", dimFactory.GetDimension("Time")) {Values = new[] {1.0f, 2.0f, 3.0f}};
         repository.Add(basegrid);
         var column = new DataColumn("conc", dimFactory.GetDimension("Dimensionless"), basegrid)
         {
            Values = new[] {1.0f, 10.0f, 100.0f}, 
            DataInfo = {Origin = ColumnOrigins.Observation}
         };

         var errorColumn = new DataColumn("error", dimFactory.GetDimension("Dimensionless"), basegrid)
         {
            Values = new[] {0.5f, 0.2f, 0.3f},
            DataInfo = {Origin = ColumnOrigins.ObservationAuxiliary, AuxiliaryType = AuxiliaryType.GeometricStdDev}
         };

         column.AddRelatedColumn(errorColumn);
         repository.Add(column);
         repository.Add(errorColumn);

         presenter.PlotObservedData(repository);

         var form = new Form();
         
         form.FillWith(presenter.View);
         
         form.Show();
      }

      private void btnReloadMenusClick(object sender, EventArgs e)
      {
         reloadMenus();
      }

      private void reloadMenus()
      {
         _chartEditorPresenter.ClearButtons();
         var showSelected = new TestProgramButton("Show Selected DataColumns", _chartEditorPresenter.SelectDataColumns);
         var saveAsTemplate = new TestProgramButton("Save as Chart Template", onChartSave);

         _chartEditorPresenter.AddButton(showSelected);
         _chartEditorPresenter.AddButton(saveAsTemplate);


         var groupMenu = CreateSubMenu.WithCaption("Layouts");
         EnumHelper.AllValuesFor<Layouts>().Each(
            layout => groupMenu.AddItem(new TestProgramButton(layout.ToString(), onLayouts)));

         groupMenu.AddItem(new TestProgramButton("Dummy", () => { }).AsGroupStarter());

         _chartEditorPresenter.AddButton(groupMenu);

         _chartEditorPresenter.AddUsedInMenuItem();

         _chartEditorPresenter.AddChartTemplateMenu(new SimulationSettings(), template => { });
      }

         

      private static void onLayouts()
      {
         Console.WriteLine("Item =  selected.");
      }

      private static void onChartSave()
      {
         Console.WriteLine("OnChartSave");
      }

      public void InitializeBinding()
      {
         
      }

      public void InitializeResources()
      {
         
      }

      private string _caption;
      private readonly ISimpleChartPresenter _simpleChartPresenter;
      private OSPSuiteXmlSerializerRepository _spSuiteXmlSerializerRepository;

      public string Caption
      {
         get { return _caption; }
         set
         {
            _caption = value;
           CaptionChanged(this, new EventArgs()); 
         } 
      }

      public event EventHandler CaptionChanged;
      public bool HasError { get; private set; }
      public void AttachPresenter(IPresenter presenter)
      {
         
      }

      public ApplicationIcon ApplicationIcon { get; set; }
      public IMdiChildView ActiveView { get; private set; }
      public void InWaitCursor(bool hourGlassVisible, bool forceCursorChange)
      {
         
      }

      public void Initialize()
      {
         
      }

      public void ShowHelp()
      {
         
      }

      public void DisplayNotification(string caption, string notification, string url)
      {
         
      }

      private void simpleButton1Click(object sender, EventArgs e)
      {
         var presenter = IoC.Resolve<IModalChartTemplateManagerPresenter>();
         presenter.Initialize();
         presenter.EditTemplates(new List<CurveChartTemplate> {generateCurveChartTemplate()});
         presenter.Display();
      }

      private CurveChartTemplate generateCurveChartTemplate()
      {
         var curveChartTemplate = new CurveChartTemplate();

         curveChartTemplate.Axes.Add(new Axis(AxisTypes.X));
         var curveTemplate = new CurveTemplate
         {
            Name = "curveName",
            CurveOptions =
            {
               LineStyle = LineStyles.None,
               Color = Color.Red
            }
         };
         curveTemplate.yData.Path = "Time";
         curveTemplate.xData.Path = "Time";

         curveChartTemplate.Curves.Add(curveTemplate);

         return curveChartTemplate;
      }

      private void simpleChartButtonClick(object sender, EventArgs e)
      {
         var simpleChartForm = new Form();

         simpleChartForm.FillWith(_simpleChartPresenter.View);
         if(_dataRepositories.Any())
            _simpleChartPresenter.Plot(_dataRepositories.First());
         simpleChartForm.Show();
      }
   }
}