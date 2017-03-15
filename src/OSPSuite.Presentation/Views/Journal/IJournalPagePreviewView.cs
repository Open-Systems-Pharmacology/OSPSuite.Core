using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IJournalPagePreviewView : IView<IJournalPagePreviewPresenter>
   {
      void BindTo(JournalPageDTO journalPageDTO);
      void DeleteBinding();
      void AddRelatedItemsView(IView view);
   }
}