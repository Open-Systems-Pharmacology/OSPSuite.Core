using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IJournalDiagramMainView : IView<IJournalDiagramMainPresenter>
   {
      void InsertDiagram(IJournalDiagramView view);
   }
}
