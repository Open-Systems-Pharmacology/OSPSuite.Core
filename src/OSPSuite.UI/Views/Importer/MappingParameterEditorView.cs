using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class MappingParameterEditorView : BaseUserControl, IMappingParameterEditorView
   {
      private IMappingParameterEditorPresenter _presenter;

      public MappingParameterEditorView()
      {
         InitializeComponent();
      }

      public void ShowUnits()
      {
         unitsLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
      }

      public void ShowLloq()
      {
         lloqLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
      }

      public void ShowErrorTypes()
      {
         errorTypeLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
      }

      public void HideAll()
      {
         unitsLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
         lloqLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
         errorTypeLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
      }

      public void AttachPresenter(IMappingParameterEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      public void FillUnitsView(IView view)
      {
         unitsPanelControl.FillWith(view);
      }

      public void FillLloqView(IView view)
      {
         lloqPanelControl.FillWith(view);
      }

      public void FillErrorView(IView view)
      {
         errorTypePanelControl.FillWith(view);
      }
   }
}