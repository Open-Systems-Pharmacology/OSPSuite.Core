using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Settings;
using OSPSuite.UI.Extensions;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IChartTestStarter : ITestStarter
   {
   }

   public class ChartTestStarter : IChartTestStarter
   {
      private readonly IChartEditorAndDisplayPresenter _chartEditorAndDisplayPresenter;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly ISimpleChartPresenter _simpleChartPresenter;
      private readonly IContainer _model;

      public ChartTestStarter(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter, IDimensionFactory dimensionFactory, ISimpleChartPresenter simpleChartPresenter)
      {
         _chartEditorAndDisplayPresenter = chartEditorAndDisplayPresenter;
         var testEnvironment = new TestEnvironment();

         _dimensionFactory = dimensionFactory;
         _simpleChartPresenter = simpleChartPresenter;
         _model = testEnvironment.Model.Root;
      }

      public void Start()
      {
         var dataRepositories = createDataRepositories(_dimensionFactory, _model);

         // Initialize ChartEditor

         _chartEditorAndDisplayPresenter.InitializeWith(new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>());
         var chartEditorPresenter = _chartEditorAndDisplayPresenter.EditorPresenter.DowncastTo<ChartEditorPresenter>();
         var chartDisplayPresenter = _chartEditorAndDisplayPresenter.DisplayPresenter;

         chartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.RepositoryName).GroupIndex = 0;
         chartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.DimensionName).GroupIndex = 1;
         chartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.Simulation).GroupIndex = -1;
         chartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.TopContainer).Caption = "TopC";
         chartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.Container).Caption = "Container";
         chartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.BottomCompartment).VisibleIndex = 2;
         chartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.Molecule).SortColumnName = BrowserColumns.OrderIndex.ToString();

         var pathToPathElementValuesMapper = new PathToPathElementsMapper(new EntityPathResolver(new ObjectPathFactory(new AliasCreator())));
         var dataColumnToPathColumnValuesMapper = new DataColumnToPathElementsMapper(pathToPathElementValuesMapper);
         chartEditorPresenter.SetDisplayQuantityPathDefinition(setQuantityPathDefinitions(_model, dataColumnToPathColumnValuesMapper));
         chartEditorPresenter.SetShowDataColumnInDataBrowserDefinition(showDataColumnInDataBrowserDefinition);


         // Set DataSource
         ICurveChart chart = new CurveChart();
         chart.OriginText = Captions.ChartFingerprintDataFrom("Project Name", "Simulation Name", DateTime.Now.ToIsoFormat());
         chartEditorPresenter.DataSource = chart;

         chart.Changed += onChartChanged;


         chart.Title = "Franz";
         chart.ChartSettings.BackColor = Color.LightYellow;

         // Add Eventhandler to changes of Chart
         chart.PropertyChanged += onChartPropertyChanged;

         chartEditorPresenter.ColumnSettingsChanged += onColumnSettingsChanged;

         // Configure DataBrowser by code
         chartEditorPresenter.GetDataBrowserColumnSettings(BrowserColumns.Origin).Visible = true;

         // Configure CurveOptions by code
         chartEditorPresenter.SetCurveNameDefinition(TestProgram.NameDefinition);
         chartEditorPresenter.GetCurveOptionsColumnSettings(CurveOptionsColumns.InterpolationMode).Visible = false;

         // Configure AxisOptions by code
         chartEditorPresenter.GetAxisOptionsColumnSettings(AxisOptionsColumns.NumberMode).Caption = "Number Representation";
         chartEditorPresenter.GetAxisOptionsColumnSettings(AxisOptionsColumns.NumberMode).VisibleIndex = 1;

         chartDisplayPresenter.DataSource = chart;
         chartEditorPresenter.AddDataRepositories(dataRepositories);
         chart.ChartSettings.DiagramBackColor = Color.LightCoral;
         chartDisplayPresenter.DragOver += onChartDisplayDragOver;
         chartDisplayPresenter.DragDrop += onChartDisplayDropped;

         chartEditorPresenter.DragOver += onChartDisplayDragOver;
         chartEditorPresenter.DragDrop += onChartDisplayDropped;


         // Create and Initialize Forms
         var frmChartEditorAndDisplay = new Form {Size = new Size(700, 500)};
         frmChartEditorAndDisplay.Controls.Add(_chartEditorAndDisplayPresenter.Control);
         _chartEditorAndDisplayPresenter.Control.Dock = DockStyle.Fill;

         // Show Forms
         frmChartEditorAndDisplay.Show();

         //  Separate Windows

         // Create and Initialize Forms
         var frmChartEditor = new Form {Size = new Size(460, 700)};
         //frmChartEditor.Size = new Size(960, 500);
         var frmChartDisplay = new Form {Size = new Size(600, 400)};

         // embed ChartEditorControl and ChartDisplayControl into available Forms
         var chartEditorView = chartEditorPresenter.View;
         frmChartEditor.FillWith(chartEditorView);

         var chartDisplayView = chartDisplayPresenter.Control;
         frmChartDisplay.Controls.Add(chartDisplayView);
         chartDisplayView.Dock = DockStyle.Fill;

         // Show Forms
         frmChartDisplay.Show();
         frmChartEditor.Show();

         var testProgramForm = new ChartTestProgramForm(_dimensionFactory, dataRepositories, chart, _chartEditorAndDisplayPresenter, _simpleChartPresenter); // prsChartDisplay, prsChartEditor);
         testProgramForm.Show();
      }

      private static bool showDataColumnInDataBrowserDefinition(DataColumn dataColumn)
      {
         if (dataColumn.Name.StartsWith("B")) return false;

         return true;
      }

      private static Func<DataColumn, PathElements> setQuantityPathDefinitions(IContainer model, DataColumnToPathElementsMapper dataColumnToPathColumnValuesMapper)
      {
         return col => dataColumnToPathColumnValuesMapper.MapFrom(col, model.RootContainer);
      }

      private static void onColumnSettingsChanged(object obj)
      {
         var columnSettings = obj as GridColumnSettings;
         if (columnSettings == null) return;
         Console.WriteLine("GridColumnSettings " + columnSettings.ColumnName + " has changed.");
      }

      private static void onChartChanged(object obj)
      {
         var chart = obj as ICurveChart;
         if (chart == null) return;
         Console.WriteLine("Chart " + chart.Name + " has changed.");
      }

      private static void onChartPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         Console.WriteLine("OnChartPropertyChanged: " + e.PropertyName);
         var chart = (IChartManagement) sender;
         var value = chart.GetType().GetProperty(e.PropertyName).GetValue(chart, null);
         Console.WriteLine("New value = " + value);
      }

      private static void onChartDisplayDragOver(object sender, DragEventArgs e)
      {
         e.Effect = e.Data.GetDataPresent(typeof(string)) ? DragDropEffects.Move : DragDropEffects.None;
      }

      private static void onChartDisplayDropped(object sender, DragEventArgs e)
      {
         if (!e.Data.GetDataPresent(typeof(String))) Console.WriteLine("OnChartDisplayDropped: No String dropped");
         else Console.WriteLine("OnChartDisplayDropped: " + e.Data.GetData(typeof(string)));
      }

      private static ICache<string, DataRepository> createDataRepositories(IDimensionFactory dimensionFactory, IContainer model)
      {
         ICache<string, DataRepository> dataRepositories = new Cache<string, DataRepository>(dr => dr.Name);
         var rep1 = new DataRepository().WithName("Rep Calc1");
         var rep2 = new DataRepository().WithName("Rep Calc2");
         var rep3 = new DataRepository().WithName("Rep Ex3");

         var rep4 = new DataRepository().WithName("Rep Dashed Exception");
         dataRepositories.Add(rep1);
         dataRepositories.Add(rep2);
         dataRepositories.Add(rep3);
         dataRepositories.Add(rep4);

         var baseGrid1 = new BaseGrid("BaseGrid1", dimensionFactory.GetDimension("Time"));
         baseGrid1.DisplayUnit = baseGrid1.Dimension.Unit(Constants.Dimension.Units.Weeks);

         var basegrid1Values = new float[1000]; //10000
         for (int i = 0; i < 1000; i++) basegrid1Values[i] = i / 100F; //10000
         baseGrid1.Values = basegrid1Values;
         var baseGridPath = new List<string> {rep1.Name, baseGrid1.Name};
         baseGrid1.QuantityInfo = new QuantityInfo(baseGrid1.Name, baseGridPath, QuantityType.Time);

         var baseGrid2 = new BaseGrid("BaseGrid2", dimensionFactory.GetDimension("Time"));
         baseGrid2.DisplayUnit = baseGrid1.Dimension.Unit(Constants.Dimension.Units.Days);
         baseGridPath = new List<string> {rep2.Name, baseGrid2.Name};
         baseGrid2.QuantityInfo = new QuantityInfo(baseGrid2.Name, baseGridPath, QuantityType.Time);
         baseGrid2.Values = new[] {0.0F * 24 * 3600, 1.0F * 24 * 3600, 2.0F * 24 * 3600, 3.0F * 24 * 3600, 4.0F * 24 * 3600};

         var baseGrid3 = new BaseGrid("BaseGrid3", dimensionFactory.GetDimension("Time"));
         baseGridPath = new List<string> {rep3.Name, baseGrid3.Name};
         baseGrid3.QuantityInfo = new QuantityInfo(baseGrid3.Name, baseGridPath, QuantityType.Time);
         baseGrid3.Values = new[] {0.0F, 2.0F, 4.0F};

         var baseGrid4 = new BaseGrid("BaseGrid4", dimensionFactory.GetDimension("Time"));
         baseGridPath = new List<string> {rep4.Name, baseGrid4.Name};
         baseGrid4.QuantityInfo = new QuantityInfo(baseGrid3.Name, baseGridPath, QuantityType.Time);
         baseGrid4.Values = new[] {0.0F, 0.00001F, 0.00002F, 2.0F, 4.0F};

         //create calculated data
         var calculation1 = new CreateDataForQuantityVisitor(rep1, baseGrid1, new DateTime(2010, 1, 15), "Calculation No 1");
         calculation1.Run(model);

         var calculation2 = new CreateDataForQuantityVisitor(rep2, baseGrid2, new DateTime(2010, 2, 15), "Calculation No 2");
         calculation2.Run(model);

         //create measurement data
         const string exSource = "Experimental Study 1";
         var exDate = new DateTime(2009, 10, 26);

         var organism = model.GetSingleChildByName<IContainer>("Organism");
         var lung = organism.GetSingleChildByName<IContainer>("Lung");
         var q1 = lung.GetSingleChildByName<IQuantity>("Q");
         var arterialBlood = organism.GetSingleChildByName<IContainer>("ArterialBlood");
         var q2 = arterialBlood.GetSingleChildByName<IQuantity>("Q");


         var dc1A = new DataColumn("Spec1", q1.Dimension, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, q1.Dimension.DefaultUnitName, exDate, exSource, "Patient A", 320) {LLOQ = 1000},
            QuantityInfo = Helper.CreateQuantityInfo(q1),
            Values = new[] {1.5F, 0.8F, Single.PositiveInfinity}
         };
         rep3.Add(dc1A);

         var dc2AStdDevA = new DataColumn("Spec2DevA", q2.Dimension, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.ArithmeticStdDev, q2.Dimension.DefaultUnitName, exDate, exSource, "Patient A", 320),
            QuantityInfo = Helper.CreateQuantityInfo(q2),
            Values = new[] {0.6F, 0.2F, 0.8F}
         };

         var dc2A = new DataColumn("Spec2", q2.Dimension, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, q2.Dimension.DefaultUnitName, exDate, exSource, "Patient A", 320) {LLOQ = 1000},
            QuantityInfo = Helper.CreateQuantityInfo(q2),
            Values = new[] {2.3F, 0.8F, 0.4F}
         };
         dc2A.AddRelatedColumn(dc2AStdDevA);
         rep3.Add(dc2A);

         var dc1B = new DataColumn("Spec1", q1.Dimension, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, q1.Dimension.DefaultUnitName, exDate, exSource, "Patient B", 320) {LLOQ = 1000},
            QuantityInfo = Helper.CreateQuantityInfo(q1),
            Values = new[] {1.6F, 1.1F, 3.9F}
         };
         rep3.Add(dc1B);

         var dimless = dimensionFactory.GetDimension("Dimensionless");
         var dc2BStdDevG = new DataColumn("Spec2DevG", dimless, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.GeometricStdDev, dimless.DefaultUnitName, exDate, exSource, "Patient B", 320),
            QuantityInfo = Helper.CreateQuantityInfo(q2),
            Values = new[] {0.1F, 0.1F, 0.15F}
         };

         var dc2B = new DataColumn("Spec2", q2.Dimension, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, q2.Dimension.DefaultUnitName, exDate, exSource, "Patient B", 320) {LLOQ = 1000},
            QuantityInfo = Helper.CreateQuantityInfo(q2),
            Values = new[] {2.1F, 0.4F, 3.3F}
         };
         dc2B.AddRelatedColumn(dc2BStdDevG);
         rep3.Add(dc2B);

         var length = dimensionFactory.GetDimension("Length");
         var dc3L1 = new DataColumn("Length1", length, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, length.DefaultUnitName, exDate, exSource, "Patient A", 320) {LLOQ = 1000},
            QuantityInfo = Helper.CreateQuantityInfo(q1),
            Values = new[] {1.1F, 1.8F, 1.4F}
         };
         rep3.Add(dc3L1);

         var dc3L2 = new DataColumn("Length2", length, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, length.DefaultUnitName, exDate, exSource, "Patient A", 320) {LLOQ = 1000},
            QuantityInfo = Helper.CreateQuantityInfo(q1),
            Values = new[] {2.1F, 2.8F, 2.4F}
         };
         rep3.Add(dc3L2);

         var dc0 = new DataColumn("ValuesWith0", length, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, length.DefaultUnitName, exDate, exSource, "Patient A", 320) {LLOQ = 1000},
            QuantityInfo = Helper.CreateQuantityInfo(q1),
            Values = new[] {0F, 0F, 1F}
         };
         rep3.Add(dc0);

         var dc10 = new DataColumn("MidZero", q1.Dimension, baseGrid2)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, q1.Dimension.DefaultUnitName, exDate, exSource, "MidZero", 320) {LLOQ = 1000},
            QuantityInfo = Helper.CreateQuantityInfo(q1),
            Values = new[] {2F, 1F, 0F, 1F, 2F}
         };
         rep2.Add(dc10);

         var dcX1ArithMeanPop = new DataColumn("X1_ArithMeanPop", length, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation, AuxiliaryType.ArithmeticMeanPop, length.DefaultUnitName, exDate, exSource, "Patient A", 320),
            QuantityInfo = Helper.CreateQuantityInfo(q2),
            Values = new[] {2.3F, 0.8F, 3.4F}
         };
         rep3.Add(dcX1ArithMeanPop);

         var dcX1ArithMeanStdDev = new DataColumn("X1_ArithMeanStdDev", length, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation, AuxiliaryType.Undefined, length.DefaultUnitName, exDate, exSource, "Patient A", 320),
            QuantityInfo = Helper.CreateQuantityInfo(q2),
            Values = new[] {0.4F, 0.2F, 0.3F}
         };
         dcX1ArithMeanStdDev.AddRelatedColumn(dcX1ArithMeanPop);
         rep3.Add(dcX1ArithMeanStdDev);

         var dcX1GeomMeanPop = new DataColumn("X1_GeomMeanPop", length, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation, AuxiliaryType.GeometricMeanPop, length.DefaultUnitName, exDate, exSource, "Patient A", 320),
            QuantityInfo = Helper.CreateQuantityInfo(q2),
            Values = new[] {2.3F, 0.8F, 3.4F}
         };
         rep3.Add(dcX1GeomMeanPop);

         var dcX1GeomMeanStdDev = new DataColumn("X1_GeomMeanStdDev", dimless, baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation, AuxiliaryType.Undefined, dimless.DefaultUnitName, exDate, exSource, "Patient A", 320),
            QuantityInfo = Helper.CreateQuantityInfo(q2),
            Values = new[] {0.4F, 0.2F, 0.3F}
         };
         dcX1GeomMeanStdDev.AddRelatedColumn(dcX1GeomMeanPop);
         rep3.Add(dcX1GeomMeanStdDev);

         //Dashed Exception at high y-Resolution
         var dc4 = new DataColumn("Spec1", q1.Dimension, baseGrid4)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, q1.Dimension.DefaultUnitName, exDate, exSource, "Patient A", 320),
            QuantityInfo = Helper.CreateQuantityInfo(q1),
            Values = new[] {0F, 1F, 1F, 1F, 1F}
         };
         rep4.Add(dc4);


         return dataRepositories;
      }
   }
}