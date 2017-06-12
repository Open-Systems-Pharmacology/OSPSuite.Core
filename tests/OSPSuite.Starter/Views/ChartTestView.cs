using DevExpress.Utils.Menu;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Starter.Views
{
   public partial class ChartTestView : BaseUserControl, IChartTestView
   {
      private IChartTestPresenter _presenter;
      private readonly ScreenBinder<ChartTestView> _screenBinder;
      private DXPopupMenu _observationsDropDownControl;
      private DXPopupMenu _calculationsDropDownControl;

      public ChartTestView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ChartTestView>();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         _observationsDropDownControl = new DXPopupMenu();
         _calculationsDropDownControl = new DXPopupMenu();
         addObservationsButton.DropDownControl = _observationsDropDownControl;
         addCalculationsButton.DropDownControl = _calculationsDropDownControl;

         _observationsDropDownControl.Items.Add(new DXMenuItem("With Arithmetic Deviation", (o, e) => OnEvent(createObservationsWithArithmenticDeviation)));
         _observationsDropDownControl.Items.Add(new DXMenuItem("With Geometric Deviation", (o, e) => OnEvent(createObservationsWithGeometricDeviation)));

         _calculationsDropDownControl.Items.Add(new DXMenuItem("With Arithmetic Population Mean", (o, e) => OnEvent(createCalculationsWithArithmeticMean)));
         _calculationsDropDownControl.Items.Add(new DXMenuItem("With Geometric Population Mean", (o, e) => OnEvent(createCalculationsWithGeometricMean)));
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         saveChartButton.Click += (o, e) => OnEvent(saveChart);
         loadChartButton.Click += (o, e) => OnEvent(loadChart);
         saveSettingsButton.Click += (o, e) => OnEvent(saveSettings);
         loadSettingsButton.Click += (o, e) => OnEvent(loadSettings);
         refreshDisplayButton.Click += (o, e) => OnEvent(refreshDisplay);
         reloadMenusButton.Click += (o, e) => OnEvent(reloadMenus);
         removeDatalessCurvesButton.Click += (o, e) => OnEvent(removeDatalessCurves);
         originalConfigurationButton.Click += (o, e) => OnEvent(originalConfiguration);
         addCalculationsButton.Click += (o, e) => OnEvent(createCalculations);
         addObservationsButton.Click += (o, e) => OnEvent(createObservations);
         clearChartButton.Click += (o, e) => OnEvent(clearChart);
         redrawChartButton.Click += (o, e) => OnEvent(redrawChart);

         NumberOfObservations = 10;
         NumberOfCalculations = 10;
         PointsPerCalculation = 30;

         _screenBinder.Bind(x => x.NumberOfCalculations).To(numberOfCalculationsTextEdit);
         _screenBinder.Bind(x => x.PointsPerCalculation).To(numberOfPointsTextEdit);
         _screenBinder.Bind(x => x.NumberOfObservations).To(numberOfObservationsTextEdit);

         _screenBinder.BindToSource(this);
      }

      private void redrawChart()
      {
         _presenter.RefreshDisplay();
      }

      private void clearChart()
      {
         _presenter.ClearChart();
      }

      public int NumberOfObservations { get; set; }

      public int NumberOfCalculations { get; set; }

      public int PointsPerCalculation { set; get; }

      private void createCalculationsWithGeometricMean()
      {
         _presenter.AddCalculationsWithGeometricMean(NumberOfCalculations, PointsPerCalculation);
      }

      private void createCalculationsWithArithmeticMean()
      {
         _presenter.AddCalculationsWithArithmeticMean(NumberOfCalculations, PointsPerCalculation);
      }

      private void createObservationsWithArithmenticDeviation()
      {
         _presenter.AddObservationsWithArithmeticDeviation(NumberOfObservations, PointsPerCalculation);
      }

      private void createObservationsWithGeometricDeviation()
      {
         _presenter.AddObservationsWithGeometricDeviation(NumberOfObservations, PointsPerCalculation);
      }

      private void createObservations()
      {
         _presenter.AddObservations(NumberOfObservations, PointsPerCalculation);
      }

      private void createCalculations()
      {
         _presenter.AddCalculations(NumberOfCalculations, PointsPerCalculation);
      }

      private void originalConfiguration()
      {
         _presenter.InitializeRepositoriesWithOriginalData();
      }

      private void removeDatalessCurves()
      {
         _presenter.RemoveDatalessCurves();
      }

      private void reloadMenus()
      {
         _presenter.ReloadMenus();
      }

      private void refreshDisplay()
      {
         _presenter.RefreshDisplay();
      }

      private void saveSettings()
      {
         _presenter.SaveSettings();
      }

      private void loadSettings()
      {
         _presenter.LoadSettings();
      }

      private void loadChart()
      {
         _presenter.LoadChart();
      }

      private void saveChart()
      {
         _presenter.SaveChart();
      }

      public void AttachPresenter(IChartTestPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddChartEditorView(IChartEditorView editorPresenterView)
      {
         chartEditorPanel.FillWith(editorPresenterView);
      }

      public void AddChartDisplayView(IChartDisplayView displayPresenterView)
      {
         chartDisplayPanel.FillWith(displayPresenterView);
      }
   }
}