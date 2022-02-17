using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace OSPSuite.Starter.Views
{
   public partial class DynamicTestView : BaseModalView, IDynamicTestView
   {
      private IDynamicTestPresenter _presenter;

      public DynamicTestView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IDynamicTestPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddCollectorView(IView view)
      {
         panel.FillWith(view);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnLoadViews.Click += (o, e) => _presenter.LoadViews();
      }
   }
}