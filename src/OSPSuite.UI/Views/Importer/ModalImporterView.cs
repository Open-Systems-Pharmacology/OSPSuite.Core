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
      }

      public void AttachPresenter(IModalImporterPresenter presenter)
      {
         _modalImporterPresenter = presenter;
      }

      protected override void OnFormClosing(FormClosingEventArgs e)
      {
         e.Cancel = !_modalImporterPresenter.ShouldClose;
      }

      public void FillImporterPanel(IView view)
      {
         importerPanelControl.FillWith(view);
      }

      public void CloseOnImport()
      {
         DialogResult = DialogResult.OK;
      }

      public void SetBaseView(IView baseView)
      {
         importerPanelControl.FillWith(baseView);
      }
   }
}