using OSPSuite.Presentation.Presenters.Parameters;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Parameters;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Parameters
{
   public partial class EditTableParameterView : BaseModalView, IEditTableParameterView
   {
      public EditTableParameterView()
      {
      }

      public EditTableParameterView(BaseShell shell) : base(shell)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditTableParameterPresenter presenter)
      {
      }

      public void AddView(IView baseView)
      {
         splitContainer.Panel1.FillWith(baseView);
      }

      public void AddChart(IView baseView)
      {
         splitContainer.Panel2.FillWith(baseView);
      }
   }
}