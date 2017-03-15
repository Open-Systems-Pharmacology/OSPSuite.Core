using System.Collections.Generic;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IJournalSearchPresenter : IPresenter<IJournalSearchView>
   {
      /// <summary>
      ///    Starts the actual search
      /// </summary>
      void StartSearch();

      /// <summary>
      ///    Clear the search but does not close the view
      /// </summary>
      void ClearSearch();

      /// <summary>
      ///    Activates the view
      /// </summary>
      void Activate();

      /// <summary>
      ///    Close the search (clear and hide the view)
      /// </summary>
      void CloseSearch();
   }

   public class JournalSearchPresenter : AbstractPresenter<IJournalSearchView, IJournalSearchPresenter>, IJournalSearchPresenter
   {
      private readonly IJournalSearchTask _searchTask;
      private readonly IEventPublisher _eventPublisher;
      private readonly JournalSearchDTO _searchDTO;
      private readonly List<string> _allSearchTerms;

      public JournalSearchPresenter(IJournalSearchView view, IJournalSearchTask searchTask, IEventPublisher eventPublisher) : base(view)
      {
         _searchTask = searchTask;
         _eventPublisher = eventPublisher;
         _allSearchTerms = new List<string>();
         _searchDTO = new JournalSearchDTO {ShowAdvancedOptions = false};
         _view.BindTo(_searchDTO);
         updateSearchTermsList();
      }

      private void updateSearchTermsList()
      {
         _view.AvailableSearchTerms = _allSearchTerms;
      }

      public void StartSearch()
      {
         _allSearchTerms.Remove(_searchDTO.Search);
         _allSearchTerms.Insert(0, _searchDTO.Search);
         _searchTask.PerformSearch(_searchDTO);
         updateSearchTermsList();
      }

      public void ClearSearch()
      {
         _searchTask.ClearSearch();
      }

      public void Activate()
      {
         _view.AdjustHeight();
         _view.Activate();
      }

      public void CloseSearch()
      {
         ClearSearch();
         _eventPublisher.PublishEvent(new JournalCloseSearchEvent());
      }
   }
}