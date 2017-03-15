using System.Drawing;
using System.Windows.Forms;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class BaseToggleableView : BaseView, IToggleableView
   {
      protected IToogleablePresenter _presenter;

      public BaseToggleableView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IToogleablePresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ShowInTaskbar = true;
         StartPosition = FormStartPosition.Manual;
      }

      protected override void OnFormClosing(FormClosingEventArgs e)
      {
         base.OnFormClosing(e);
         if (e.CloseReason != CloseReason.UserClosing)
            return;

         e.Cancel = true;

         _presenter.FormClosing(Location, Size);
      }

      public void ToggleVisibility()
      {
         if (Visible)
            Hide();
         else
            Display();
      }

      public void Display()
      {
         this.EnsureVisible();
      }

      public void SetFormLayout(Point location, Size size)
      {
         var desiredFormArea = new Rectangle(location, size);
         this.FitToScreen(desiredFormArea, getClosestWorkingArea(desiredFormArea));
      }

      private static Rectangle getClosestWorkingArea(Rectangle desiredFormArea)
      {
         return Screen.GetWorkingArea(desiredFormArea);
      }
   }
}
