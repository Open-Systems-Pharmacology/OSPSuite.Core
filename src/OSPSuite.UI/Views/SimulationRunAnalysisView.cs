using DevExpress.XtraLayout.Utils;
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
         layoutControlItem2.TextVisible = true;
         layoutControlItem2.Visibility = LayoutVisibility.Never;
         layoutControlItem2.Text = "Total error";
         totalErrorTextEdit.ReadOnly = true;
      }

      public void SetAnalysisView(IView view)
      {
         panelChart.FillWith(view);
      }

      public void SetTotalError(double totalError)
      {
         layoutControlItem2.Visibility = LayoutVisibility.Always;
         totalErrorTextEdit.Text = totalError.ToString();
      }

      public void AttachPresenter(ISimulationRunAnalysisPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
