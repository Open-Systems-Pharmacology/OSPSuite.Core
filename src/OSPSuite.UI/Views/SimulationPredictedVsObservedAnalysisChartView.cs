using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class SimulationPredictedVsObservedAnalysisChartView : BaseUserControl, ISimulationPredictedVsObservedAnalysisChartView
   {
      private ISimulationPredictedVsObservedChartPresenter _presenter;
      public SimulationPredictedVsObservedAnalysisChartView()
      {
         InitializeComponent();
      }

      public void SetAnalysisView(IView view)
      {
         panelChart.FillWith(view);
      }

      public void AttachPresenter(ISimulationPredictedVsObservedChartPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
