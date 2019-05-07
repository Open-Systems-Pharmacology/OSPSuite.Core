using System.Collections.Generic;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IJournalView : IView<IJournalPresenter>
   {
      void BindTo(IEnumerable<JournalPageDTO> allWorkingJournalItemDTOs);

      /// <summary>
      /// Redraw the view content to ensure that rendering is updated
      /// </summary>
      void UpdateLayout();

      void AddPreviewView(IView view);

      void AddSearchView(IView view);

      /// <summary>
      /// Returns/set the selected <see cref="JournalPageDTO"/> 
      /// </summary>
      JournalPageDTO SelectedJournalPage { get; set; }

      bool SearchVisible { get; set; }

      /// <summary>
      /// Returns the pages which are currently visible in the view
      /// </summary>
      IEnumerable<JournalPage> VisibleJournalPages();
   }
}