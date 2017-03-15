using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class ModalChartTemplateManagerView : BaseModalView, IModalChartTemplateManagerView
   {
      public ModalChartTemplateManagerView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         CancelVisible = true;
         Caption = Captions.ManageChartTemplates;
      }

      public void AttachPresenter(IModalChartTemplateManagerPresenter presenter)
      {

      }

      public void SetBaseView(IView baseView)
      {
         panelControl.FillWith(baseView);
      }
   }
}
