using System.Collections.Generic;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IJournalSearchView : IView<IJournalSearchPresenter>, IResizableView
   {
      void BindTo(JournalSearchDTO searchDTO);
      void Activate();
      IEnumerable<string> AvailableSearchTerms{set;}
   }
}