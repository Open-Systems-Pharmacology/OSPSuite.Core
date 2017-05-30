using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   public partial class SensitivityAnalysisStarterView : BaseUserControl, ISensitivityAnalysisStarterView
   {
      private ISensitivityAnalysisStarterPresenter _presenter;

      public SensitivityAnalysisStarterView()
      {
         InitializeComponent();
      }

  

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnSensitivityAnalysis.Click += (o, e) => OnEvent(() => _presenter.StartSensitivityAnalysisTest());
      }

      public void AttachPresenter(ISensitivityAnalysisStarterPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnSensitivityAnalysis.Text = "Start Sensitiity Analysis";
      }
   }
}
