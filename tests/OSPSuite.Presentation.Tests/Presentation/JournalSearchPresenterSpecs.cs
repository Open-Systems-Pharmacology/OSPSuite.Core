using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_JournalSearchPresenter : ContextSpecification<IJournalSearchPresenter>
   {
      protected IEventPublisher _eventPublisher;
      protected IJournalSearchView _view;
      protected IJournalSearchTask _searchTask;
      protected JournalSearchDTO _journalSearchDTO;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _view = A.Fake<IJournalSearchView>();
         _searchTask = A.Fake<IJournalSearchTask>();

         A.CallTo(() => _view.BindTo(A<JournalSearchDTO>._))
            .Invokes(x => _journalSearchDTO = x.GetArgument<JournalSearchDTO>(0));

         sut = new JournalSearchPresenter(_view, _searchTask, _eventPublisher);
      }
   }

   public class When_creating_the_search_presenter : concern_for_JournalSearchPresenter
   {
      [Observation]
      public void shoudl_set_the_default_search_to_not_use_advanced_options()
      {
         _journalSearchDTO.ShowAdvancedOptions.ShouldBeFalse();
      }
   }

   public class When_starting_the_search_in_a_journal : concern_for_JournalSearchPresenter
   {
      protected override void Because()
      {
         sut.StartSearch();
      }

      [Observation]
      public void should_leverage_the_search_task_to_perform_the_search()
      {
         A.CallTo(() => _searchTask.PerformSearch(_journalSearchDTO)).MustHaveHappened();
      }
   }

   public class When_the_search_presenter_is_being_activated : concern_for_JournalSearchPresenter
   {
      protected override void Because()
      {
         sut.Activate();
      }

      [Observation]
      public void should_resize_the_view()
      {
         A.CallTo(() => _view.AdjustHeight()).MustHaveHappened();
      }

      [Observation]
      public void should_activate_the_view()
      {
         A.CallTo(() => _view.Activate()).MustHaveHappened();
      }
   }

   public class When_clearing_the_search : concern_for_JournalSearchPresenter
   {
      protected override void Because()
      {
         sut.ClearSearch();
      }

      [Observation]
      public void should_leverage_the_search_task_to_clear_the_search()
      {
         A.CallTo(() => _searchTask.ClearSearch()).MustHaveHappened();
      }
   }

   public class When_closing_the_search : concern_for_JournalSearchPresenter
   {
      private JournalCloseSearchEvent _closeEvent;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _eventPublisher.PublishEvent(A<JournalCloseSearchEvent>._))
            .Invokes(x => _closeEvent = x.GetArgument<JournalCloseSearchEvent>(0));
      }

      protected override void Because()
      {
         sut.CloseSearch();
      }

      [Observation]
      public void should_leverage_the_search_task_to_clear_the_search()
      {
         A.CallTo(() => _searchTask.ClearSearch()).MustHaveHappened();
      }

      [Observation]
      public void should_publish_the_close_search_event()
      {
         _closeEvent.ShouldNotBeNull();
      }
   }
}