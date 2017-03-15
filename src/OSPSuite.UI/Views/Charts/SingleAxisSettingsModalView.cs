using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class SingleAxisSettingsModalView : BaseModalView, ISingleAxisSettingsModalView
   {
      public SingleAxisSettingsModalView(IShell shell) : base(shell)
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISingleAxisSettingsModalPresenter presenter)
      {
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         CancelVisible = false;
         Icon = ApplicationIcons.DefaultIcon;
         FormBorderStyle = FormBorderStyle.SizableToolWindow;
      }

      public void SetSummaryView(IView view)
      {
         summaryPanel.FillWith(view);
      }
   }
}