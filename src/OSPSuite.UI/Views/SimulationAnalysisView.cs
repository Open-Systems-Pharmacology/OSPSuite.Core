using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class SimulationAnalysisView : BaseUserControl, ISimulationAnalysisView
   {
      public SimulationAnalysisView()
      {
         InitializeComponent();
      }

      public void SetAnalysisView(IView view)
      {
         panelChart.FillWith(view);
      }
   }
}
