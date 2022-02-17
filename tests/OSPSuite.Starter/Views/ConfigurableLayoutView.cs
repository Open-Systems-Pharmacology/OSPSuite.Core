using DevExpress.XtraLayout.Utils;
using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   public partial class ConfigurableLayoutView : BaseContainerUserControl, IConfigurableLayoutView
   {
      public ConfigurableLayoutView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IConfigurableLayoutPresenter presenter)
      {
         /*nothing to do*/
      }

      public void Clear()
      {
         layoutMainControl.Clear(clearHiddenItems: true, disposeControls: true);
      }

      public void SetView(IView view)
      {
         layoutMainControl.SuspendLayout();
         var layoutItem = AddViewToGroup(layoutMainControl.Root, view);
         layoutItem.Padding = new Padding(0);
         layoutMainControl.ResumeLayout();
      }
   }
}