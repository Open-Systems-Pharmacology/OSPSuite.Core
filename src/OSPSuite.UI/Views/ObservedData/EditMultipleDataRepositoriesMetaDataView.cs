using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ObservedData
{
   public partial class EditMultipleDataRepositoriesMetaDataView : BaseModalView, IEditMultipleDataRepositoriesMetaDataView
   {

      public EditMultipleDataRepositoriesMetaDataView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditMultipleDataRepositoriesMetaDataPresenter presenter)
      {
      }

      public void SetDataEditor(IView view)
      {
         panelControl.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.EditMultipleObservedDataMetaData;
         layoutControlGroup.Padding = new Padding(0);
      }
   }
}