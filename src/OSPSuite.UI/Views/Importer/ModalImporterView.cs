using System.Windows.Forms;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ModalImporterView : BaseModalView, IModalImporterView
   {
      private IModalImporterPresenter _modalImporterPresenter;
      public ModalImporterView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         CancelVisible = false;
         layoutControlBase.Visible = false;
         MaximizeBox = true;
      }

      public void AttachPresenter(IModalImporterPresenter presenter)
      {
         _modalImporterPresenter = presenter;
      }

      protected override bool ShouldClose => _modalImporterPresenter.ShouldClose;

      public void FillImporterPanel(IView view)
      {
         importerPanelControl.FillWith(view);
      }

      public void SetBaseView(IView baseView)
      {
         importerPanelControl.FillWith(baseView);
      }
   }
}