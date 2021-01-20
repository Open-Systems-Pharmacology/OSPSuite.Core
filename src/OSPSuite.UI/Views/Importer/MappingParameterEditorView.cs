using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
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
         UnitsLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
      }

      public void ShowLloq()
      {
         LloqLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
      }

      public void ShowErrorTypes()
      {
         ErrorLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
      }

      public void HideAll()
      {
         UnitsLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
         LloqLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
         ErrorLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
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