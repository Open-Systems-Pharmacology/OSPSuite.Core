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

         _observationsDropDownControl.Items.Add(new DXMenuItem("With Arithmetic Deviation", (o, e) => OnEvent(createObservationsWithArithmeticDeviation)));
         _observationsDropDownControl.Items.Add(new DXMenuItem("With Geometric Deviation", (o, e) => OnEvent(createObservationsWithGeometricDeviation)));

         _calculationsDropDownControl.Items.Add(new DXMenuItem("With Arithmetic Population Mean", (o, e) => OnEvent(createCalculationsWithArithmeticMean)));
         _calculationsDropDownControl.Items.Add(new DXMenuItem("With Geometric Population Mean", (o, e) => OnEvent(createCalculationsWithGeometricMean)));
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

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
         PointsPerCalculation = 200;
         PointsPerObservation = 15;

         _screenBinder.Bind(x => x.NumberOfCalculations).To(numberOfCalculationsTextEdit);
         _screenBinder.Bind(x => x.PointsPerCalculation).To(numberOfCalculationPointsTextEdit);
         _screenBinder.Bind(x => x.NumberOfObservations).To(numberOfObservationsTextEdit);
         _screenBinder.Bind(x => x.PointsPerObservation).To(numberOfObservationPointsTextEdit);
         _screenBinder.Bind(x => x.LLOQ).To(lloqForObservationsTextEdit);

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

      public double? LLOQ { get; set; }

      public int NumberOfObservations { get; set; }

      public int NumberOfCalculations { get; set; }

      public int PointsPerCalculation { set; get; }

      public int PointsPerObservation { set; get; }

      private void createCalculationsWithGeometricMean()
      {
         _presenter.AddCalculationsWithGeometricMean(NumberOfCalculations, PointsPerCalculation);
      }

      private void createCalculationsWithArithmeticMean()
      {
         _presenter.AddCalculationsWithArithmeticMean(NumberOfCalculations, PointsPerCalculation);
      }

      private void createObservationsWithArithmeticDeviation()
      {
         _presenter.AddObservationsWithArithmeticDeviation(NumberOfObservations, PointsPerObservation, LLOQ);
      }

      private void createObservationsWithGeometricDeviation()
      {
         _presenter.AddObservationsWithGeometricDeviation(NumberOfObservations, PointsPerObservation, LLOQ);
      }

      private void createObservations()
      {
         _presenter.AddObservations(NumberOfObservations, PointsPerObservation, LLOQ);
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