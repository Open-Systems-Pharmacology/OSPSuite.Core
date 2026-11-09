using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IErrorTypeColumnEditorView : IView<IErrorTypeColumnEditorPresenter>
   {
      void FillErrorTypeColumnSelector(IView view);
      void SetErrorTypeColumnToggle(bool errorTypeFromColumn);
      bool IsErrorTypeColumnToggleOn();
   }
}
