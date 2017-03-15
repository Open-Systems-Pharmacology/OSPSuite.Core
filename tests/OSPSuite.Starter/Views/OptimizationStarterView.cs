using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   public partial class OptimizationStarterView : BaseUserControl, IOptimizationStarterView
   {
      private IOptimizationStarterPresenter _presenter;

      public OptimizationStarterView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnMatrix.Click += (o, e) => OnEvent(() => _presenter.StartMatrixTest());
      }

      public void AttachPresenter(IOptimizationStarterPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
