using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class SimulationRunAnalysisView : BaseUserControl, ISimulationRunAnalysisView
   {
      private ISimulationRunAnalysisPresenter _presenter;
      public SimulationRunAnalysisView()
      {
         InitializeComponent();
      }

      public void SetAnalysisView(IView view)
      {
         panelChart.FillWith(view);
      }

      public void AttachPresenter(ISimulationRunAnalysisPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
