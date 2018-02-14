using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_JournalPresenter : ContextSpecification<IJournalPresenter>
   {
      protected IRegionResolver _regionResolver;
      protected IJournalPageToJournalPageDTOMapper _mapper;
      protected IJournalTask _journalTask;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IJournalRetriever _journalRetriever;
      protected IJournalPagePreviewPresenter _previewPresenter;
      protected IJournalView _view;
      protected IJournalExportTask _journalExportTask;

      protected IRegion _region;
      protected IList<JournalPageDTO> _allWorkingJournalItemDTOs;

      protected Journal _journal;
      protected JournalPage _journalPage;
      protected JournalPageDTO _journalPageDTO;
      protected IJournalSearchPresenter _searchPresenter;
      protected JournalSearch _journalSearch;

      protected override void Context()
      {
         _view = A.Fake<IJournalView>();
         _regionResolver = A.Fake<IRegionResolver>();
         _mapper = A.Fake<IJournalPageToJournalPageDTOMapper>();
         _journalTask = A.Fake<IJournalTask>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _journalRetriever = A.Fake<IJournalRetriever>();
         _previewPresenter = A.Fake<IJournalPagePreviewPresenter>();
         _journalExportTask = A.Fake<IJournalExportTask>();
         _searchPresenter = A.Fake<IJournalSearchPresenter>();
         _journalSearch = new JournalSearch();
         A.CallTo(() => _view.BindTo(A<IEnumerable<JournalPageDTO>>._))
            .Invokes(x => _allWorkingJournalItemDTOs = x.GetArgument<IEnumerable<JournalPageDTO>>(0).DowncastTo<IList<JournalPageDTO>>());

         _region = A.Fake<IRegion>();
         A.CallTo(_regionResolver).WithReturnType<IRegion>().Returns(_region);

         sut = new JournalPresenterForSpecs(_view, _regionResolver, _mapper, _journalTask, _viewItemContextMenuFactory,
            _journalRetriever, _previewPresenter, _searchPresenter);

         _journal = new Journal();
         A.CallTo(() => _journalRetriever.Current).Returns(_journal);

         _journalPage = new JournalPage();
         _journalPageDTO = new JournalPageDTO(_journalPage);
         A.CallTo(_mapper).WithReturnType<JournalPageDTO>().Returns(_journalPageDTO);
      }
   }

   public class When_initializing_the_journal_presenter : concern_for_JournalPresenter
   {
      [Observation]
      public void should_add_the_view_in_the_predefined_journal_region()
      {
         A.CallTo(() => _region.Add(_view)).MustHaveHappened();
      }

      [Observation]
      public void should_bind_to_the_local_list_of_journal_page()
      {
         _allWorkingJournalItemDTOs.ShouldNotBeNull();
      }
   }

   public class When_the_journal_presenter_is_notified_that_the_journal_is_closed : concern_for_JournalPresenter
   {
      protected override void Context()
      {
         base.Context();
         _allWorkingJournalItemDTOs.Add(_journalPageDTO);
      }

      protected override void Because()
      {
         sut.Handle(new JournalClosedEvent());
      }

      [Observation]
      public void should_clear_the_list_of_journal_page_shown_in_the_view()
      {
         _allWorkingJournalItemDTOs.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_clear_the_preview()
      {
         A.CallTo(() => _previewPresenter.ClearPreview()).MustHaveHappened();
      }
   }

   public class When_the_journal_presenter_is_showing_the_context_menu_for_an_undefined_view_item : concern_for_JournalPresenter
   {
      private Point _location;
      private IViewItem _viewItem;

      protected override void Context()
      {
         base.Context();
         _location = new Point();
         A.CallTo(() => _viewItemContextMenuFactory.CreateFor(A<IViewItem>._, sut))
            .Invokes(x => _viewItem = x.GetArgument<IViewItem>(0))
            .Returns(A.Fake<IContextMenu>());
      }

      protected override void Because()
      {
         sut.ShowContextMenu(null, _location);
      }

      [Observation]
      public void should_create_the_context_menu_for_the_whole_journal()
      {
         _viewItem.ShouldBeAnInstanceOf<JournalDTO>();
      }
   }

   public class When_the_journal_presenter_is_notified_that_the_journal_page_is_loaded : concern_for_JournalPresenter
   {
      protected override void Context()
      {
         base.Context();
         _journal.AddJournalPage(_journalPage);
         _journal.Edited = _journalPage;
      }

      protected override void Because()
      {
         sut.Handle(new JournalLoadedEvent(_journal));
      }

      [Observation]
      public void should_add_all_existing_journal_pages_to_the_view()
      {
         _allWorkingJournalItemDTOs.Contains(_journalPageDTO).ShouldBeTrue();
      }

      [Observation]
      public void should_select_the_edited_journal_page_if_available()
      {
         A.CallTo(() => _journalTask.Edit(_journalPage, false, null)).MustHaveHappened();
      }
   }

   public class When_the_journal_presenter_is_notified_that_a_journal_page_was_deleted : concern_for_JournalPresenter
   {
      protected override void Context()
      {
         base.Context();
         _allWorkingJournalItemDTOs.Add(_journalPageDTO);
      }

      protected override void Because()
      {
         sut.Handle(new JournalPageDeletedEvent(_journalPage));
      }

      [Observation]
      public void should_remove_the_journal_page_from_the_view()
      {
         _allWorkingJournalItemDTOs.ShouldNotContain(_journalPageDTO);
      }
   }

   public class When_the_journal_presenter_is_notified_that_a_journal_page_was_added : concern_for_JournalPresenter
   {
      protected override void Because()
      {
         sut.Handle(new JournalPageAddedEvent(_journalPage));
      }

      [Observation]
      public void should_add_the_journal_page_into_the_view()
      {
         _allWorkingJournalItemDTOs.ShouldContain(_journalPageDTO);
      }

      [Observation]
      public void should_preview_the_newly_added_journal_page()
      {
         A.CallTo(() => _previewPresenter.Preview(_journalPageDTO)).MustHaveHappened();
      }

      [Observation]
      public void should_select_the_added_journal_page_in_the_view()
      {
         _view.SelectedJournalPage.ShouldBeEqualTo(_journalPageDTO);
      }
   }

   public class When_the_journal_presenter_is_notified_that_the_journal_is_requiested_to_be_displayed : concern_for_JournalPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ShowJournalEvent());
      }

      [Observation]
      public void should_display_the_view()
      {
         A.CallTo(() => _region.Show()).MustHaveHappened();
      }
   }

   public class When_the_user_triggers_the_deletion_of_an_existing_journal_page : concern_for_JournalPresenter
   {
      protected override void Context()
      {
         base.Context();
         _journalPage.Title = "ABC";
      }

      protected override void Because()
      {
         sut.Delete(_journalPageDTO);
      }

      [Observation]
      public void should_leverage_the_journal_task_to_delete_the_journal_page()
      {
         A.CallTo(() => _journalTask.DeleteJournalPage(_journalPage)).MustHaveHappened();
      }
   }

   public class When_the_journal_presenter_is_notified_that_a_journal_page_is_being_edited : concern_for_JournalPresenter
   {
      private JournalPageDTO _anotherJournalPageBeingEdited;

      protected override void Context()
      {
         base.Context();
         _anotherJournalPageBeingEdited = new JournalPageDTO(new JournalPage()) {Edited = true};
         _allWorkingJournalItemDTOs.Add(_journalPageDTO);
         _allWorkingJournalItemDTOs.Add(_anotherJournalPageBeingEdited);
      }

      protected override void Because()
      {
         sut.Handle(new EditJournalPageStartedEvent(_journalPage, true));
      }

      [Observation]
      public void should_mark_the_journal_page_dto_as_edited()
      {
         _journalPageDTO.Edited.ShouldBeTrue();
      }

      [Observation]
      public void should_update_the_edited_journal_page_in_the_journal()
      {
         _journal.Edited.ShouldBeEqualTo(_journalPage);
      }

      [Observation]
      public void should_mark_the_previously_edited_journal_page()
      {
         _anotherJournalPageBeingEdited.Edited.ShouldBeFalse();
      }

      [Observation]
      public void should_update_the_layout_in_the_view_to_reflect_the_change()
      {
         A.CallTo(() => _view.UpdateLayout()).MustHaveHappened();
      }
   }

   public class When_the_journal_presenter_is_notified_that_a_journal_page_was_updated : concern_for_JournalPresenter
   {
      protected override void Context()
      {
         base.Context();
         _allWorkingJournalItemDTOs.Add(_journalPageDTO);
         _view.SelectedJournalPage = new JournalPageDTO(new JournalPage());
      }

      protected override void Because()
      {
         sut.Handle(new JournalPageUpdatedEvent(_journalPage));
      }

      [Observation]
      public void should_update_the_view_with_the_corresponding_information()
      {
         A.CallTo(() => _mapper.Update(_journalPageDTO, _journalPage, A<Func<JournalPage, string>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_preview_with_the_currently_selected_journal_item()
      {
         A.CallTo(() => _previewPresenter.Preview(_view.SelectedJournalPage)).MustHaveHappened();
      }
   }

   public class When_the_journal_presenter_is_starting_the_edition_of_a_given_journal_page : concern_for_JournalPresenter
   {
      protected override void Context()
      {
         base.Context();
         _allWorkingJournalItemDTOs.Add(_journalPageDTO);
      }

      protected override void Because()
      {
         sut.Edit(_journalPageDTO);
      }

      [Observation]
      public void should_start_the_edit_workflow()
      {
         A.CallTo(() => _journalTask.Edit(_journalPage, true, null)).MustHaveHappened();
      }
   }

   public class When_the_journal_presenter_is_notified_that_a_journal_page_should_be_edited_and_the_journal_page_is_not_currently_displayed : concern_for_JournalPresenter
   {
      [Observation]
      public void should_not_crash()
      {
         sut.Handle(new EditJournalPageStartedEvent(new JournalPage {Id = "XYZ"}, showEditor: false));
      }
   }

   public class When_the_user_selects_a_working_jounral_item : concern_for_JournalPresenter
   {
      protected override void Context()
      {
         base.Context();
         _allWorkingJournalItemDTOs.Add(_journalPageDTO);
      }

      protected override void Because()
      {
         sut.Select(_journalPageDTO);
      }

      [Observation]
      public void should_start_the_edit_workflow_but_not_show_the_editor()
      {
         A.CallTo(() => _journalTask.Edit(_journalPage, false, null)).MustHaveHappened();
      }
   }

   public class When_the_journal_presenter_is_showing_the_search : concern_for_JournalPresenter
   {
      protected override void Because()
      {
         sut.ShowSearch();
      }

      [Observation]
      public void should_activate_the_search_presenter()
      {
         A.CallTo(() => _searchPresenter.Activate()).MustHaveHappened();
      }

      [Observation]
      public void should_show_the_journal_view()
      {
         A.CallTo(() => _region.Show()).MustHaveHappened();
      }

      [Observation]
      public void should_show_the_search()
      {
         _view.SearchVisible.ShouldBeTrue();
      }
   }

   public class When_notify_that_the_search_is_being_closed : concern_for_JournalPresenter
   {
      protected override void Because()
      {
         sut.Handle(new JournalCloseSearchEvent());
      }

      [Observation]
      public void should_hide_the_search_view()
      {
         _view.SearchVisible.ShouldBeFalse();
      }
   }

   public class When_notify_that_a_search_was_performed : concern_for_JournalPresenter
   {
      private List<JournalSearchItem> _searchResults;
      private JournalPage _journalItem1;
      private JournalPage _journalItem2;
      private JournalPageDTO _dto1;
      private JournalPageDTO _dto2;

      protected override void Context()
      {
         base.Context();
         _journalItem1 = new JournalPage();
         _journalItem2 = new JournalPage();

         _searchResults = new List<JournalSearchItem>
         {
            new JournalSearchItem {JournalPage = _journalItem1, Context = "Context1"},
            new JournalSearchItem {JournalPage = _journalItem2, Context = "Context2"}
         };

         _dto1 = new JournalPageDTO(_journalItem1);
         _dto2 = new JournalPageDTO(_journalItem2);
         A.CallTo(() => _mapper.MapFrom(_journalItem1, A<Func<JournalPage, string>>._)).Returns(_dto1);
         A.CallTo(() => _mapper.MapFrom(_journalItem2, A<Func<JournalPage, string>>._)).Returns(_dto2);
      }

      protected override void Because()
      {
         sut.Handle(new JournalSearchPerformedEvent(_searchResults, _journalSearch));
      }

      [Observation]
      public void should_bind_to_the_search_results()
      {
         _allWorkingJournalItemDTOs.ShouldOnlyContain(_dto1, _dto2);
      }
   }

   public class When_editing_a_jounrla_page_after_a_seach_was_performed : concern_for_JournalPresenter
   {
      private List<JournalSearchItem> _searchResults;
      private JournalPage _journalItem1;
      private JournalPageDTO _dto1;

      protected override void Context()
      {
         base.Context();
         _journalItem1 = new JournalPage();

         _searchResults = new List<JournalSearchItem>
         {
            new JournalSearchItem {JournalPage = _journalItem1, Context = "Context1"},
         };

         _dto1 = new JournalPageDTO(_journalItem1);
         A.CallTo(() => _mapper.MapFrom(_journalItem1, A<Func<JournalPage, string>>._)).Returns(_dto1);
         sut.Handle(new JournalSearchPerformedEvent(_searchResults, _journalSearch));
      }

      protected override void Because()
      {
         sut.Edit(_dto1);
      }

      [Observation]
      public void should_edit_the_page_and_start_the_search_result()
      {
         A.CallTo(() => _journalTask.Edit(_journalItem1, true, _journalSearch)).MustHaveHappened();
      }
   }

   public class When_retrieving_all_tags_available_for_the_journal_pages_displayed : concern_for_JournalPresenter
   {
      private JournalPage _journalItem1;
      private JournalPage _journalItem2;
      private JournalPageDTO _dto1;
      private JournalPageDTO _dto2;

      protected override void Context()
      {
         base.Context();
         _journalItem1 = new JournalPage();
         _journalItem2 = new JournalPage();
         _dto1 = new JournalPageDTO(_journalItem1);
         _dto2 = new JournalPageDTO(_journalItem2);
         A.CallTo(() => _mapper.MapFrom(_journalItem1, A<Func<JournalPage, string>>._)).Returns(_dto1);
         A.CallTo(() => _mapper.MapFrom(_journalItem2, A<Func<JournalPage, string>>._)).Returns(_dto2);

         _dto1.Tags = new List<string> {"B", "C"};
         _dto2.Tags = new List<string> {"A", "C"};
         _journal.AddJournalPage(_journalItem1);
         _journal.AddJournalPage(_journalItem2);
         sut.Handle(new JournalLoadedEvent(_journal));
      }

      [Observation]
      public void should_filter_out_all_tags_from_the_journal_page_currently_displayed_and_sort_them()
      {
         sut.AvailableTags.ShouldOnlyContain("A", "B", "C");
      }
   }

   public class When_checking_if_all_displayed_have_the_same_source : concern_for_JournalPresenter
   {
      private JournalPage _journalItem1;
      private JournalPage _journalItem2;
      private JournalPageDTO _dto1;
      private JournalPageDTO _dto2;

      protected override void Context()
      {
         base.Context();
         _journalItem1 = new JournalPage();
         _journalItem2 = new JournalPage();
         _dto1 = new JournalPageDTO(_journalItem1) {Origin = Origins.PKSim};
         _dto2 = new JournalPageDTO(_journalItem2) {Origin = Origins.PKSim};
         A.CallTo(() => _mapper.MapFrom(_journalItem1, A<Func<JournalPage, string>>._)).Returns(_dto1);
         A.CallTo(() => _mapper.MapFrom(_journalItem2, A<Func<JournalPage, string>>._)).Returns(_dto2);

         _journal.AddJournalPage(_journalItem1);
         _journal.AddJournalPage(_journalItem2);
         sut.Handle(new JournalLoadedEvent(_journal));
      }

      [Observation]
      public void should_return_true_if_all_items_have_the_same_source()
      {
         sut.AllItemsHaveTheSameOrigin.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_items_have_different_source()
      {
         _dto2.Origin = Origins.Matlab;
         sut.AllItemsHaveTheSameOrigin.ShouldBeFalse();
      }
   }

   internal class JournalPresenterForSpecs : JournalPresenter
   {
      public JournalPresenterForSpecs(IJournalView view, IRegionResolver regionResolver, IJournalPageToJournalPageDTOMapper mapper,
         IJournalTask journalTask, IViewItemContextMenuFactory viewItemContextMenuFactory, IJournalRetriever journalRetriever,
         IJournalPagePreviewPresenter previewPresenter, IJournalSearchPresenter searchPresenter) :
            base(view, regionResolver, mapper, journalTask, viewItemContextMenuFactory, journalRetriever,
               previewPresenter, searchPresenter, new RegionName("XX", "XX", ApplicationIcons.EmptyIcon))
      {
      }
   }
}