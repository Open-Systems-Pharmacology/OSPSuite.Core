using System.Drawing;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IJournalPageEditorFormView : IView<IJournalPageEditorFormPresenter>
   {
      /// <summary>
      /// Attaches the <paramref name="view"/>to the form
      /// </summary>
      IJournalPageEditorFormView AttachWorkingJournalItemEditorView(IJournalPageEditorView view);

      /// <summary>
      /// Shows the form without an owner
      /// </summary>
      void Display();

      void Hide();

      void ToggleVisibility();

      /// <summary>
      /// Moves and resizes the form according to <paramref name="location"/> and <paramref name="size"/>
      /// </summary>
      void SetFormLayout(Point location, Size size);
   }
}