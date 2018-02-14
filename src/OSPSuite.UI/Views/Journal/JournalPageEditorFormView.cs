using System.Drawing;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Journal
{
   public partial class JournalPageEditorFormView : BaseView, IJournalPageEditorFormView
   {
      private IJournalPageEditorFormPresenter _presenter;

      public JournalPageEditorFormView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IJournalPageEditorFormPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.Journal.JournalEditorView;
         Icon = ApplicationIcons.PageEdit.WithSize(IconSizes.Size32x32);
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

      public IJournalPageEditorFormView AttachWorkingJournalItemEditorView(IJournalPageEditorView view)
      {
         panelControl.FillWith(view);
         view.CaptionChanged += (o, e) => viewCaptionChanged(view);
         return this;
      }

      private void viewCaptionChanged(IView view)
      {
         Caption = view.Caption;
      }

      public void Display()
      {
         this.EnsureVisible();
      }

      public void ToggleVisibility()
      {
         if (Visible)
            Hide();
         else
            Display();
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