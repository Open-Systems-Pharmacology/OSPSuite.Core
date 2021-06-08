using System.Windows.Forms;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ModalImporterView : BaseModalView, IModalImporterView
   {
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
      }

      public void FillImporterPanel(IView view)
      {
         importerPanelControl.FillWith(view);
      }

      public void AttachImporterPresenter(IImporterPresenter presenter)
      {
         presenter.OnTriggerImport += (s, d) => { DialogResult = DialogResult.OK; };
      }

      public void SetBaseView(IView baseView)
      {
         importerPanelControl.FillWith(baseView);
      }
   }
}