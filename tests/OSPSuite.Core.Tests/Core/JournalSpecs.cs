using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;

namespace OSPSuite.Core
{
   public abstract class concern_for_Journal : ContextSpecification<Journal.Journal>
   {
      protected override void Context()
      {
         sut = new Journal.Journal();
      }
   }

   public class When_retrieving_a_jounral_page_by_id : concern_for_Journal
   {
      private JournalPage _journalPage;

      protected override void Context()
      {
         base.Context();
         _journalPage = new JournalPage().WithId("id");
         sut.AddJournalPage(_journalPage);
      }

      [Observation]
      public void should_return_the_working_jounral_item_with_the_given_id_if_defined()
      {
         sut.JournalPageById("id").ShouldBeEqualTo(_journalPage);
      }

      [Observation]
      public void should_return_null_if_no_journal_page_is_defined_with_the_givn_id()
      {
         sut.JournalPageById("DOES NOT EXIST").ShouldBeNull();
      }
   }

   public class When_retrieving_a_jounral_page_by_index : concern_for_Journal
   {
      private JournalPage _journalPage1;
      private JournalPage _journalPage2;

      protected override void Context()
      {
         base.Context();
         _journalPage1 = new JournalPage().WithId("Page1");
         _journalPage2 = new JournalPage().WithId("Page2");
         sut.AddJournalPages(new []{_journalPage1,_journalPage2});
      }

      [Observation]
      public void should_return_the_page_defined_at_the_given_index_if_the_index_is_withing_the_correct_bound()
      {
         sut.JournalPageByIndex(sut.JournalPageIndexFor(_journalPage1)).ShouldBeEqualTo(_journalPage1);
         sut.JournalPageByIndex(sut.JournalPageIndexFor(_journalPage2)).ShouldBeEqualTo(_journalPage2);
      }

      [Observation]
      public void should_return_null_if_the_index_is_out_of_bounds()
      {
         sut.JournalPageByIndex(-10).ShouldBeEqualTo(null);
         sut.JournalPageByIndex(3).ShouldBeEqualTo(null);
      }
   }

   public class When_removing_the_edited_page : concern_for_Journal
   {
      private JournalPage _editedPage;

      protected override void Context()
      {
         base.Context();
         _editedPage = new JournalPage();
         sut.AddJournalPage(_editedPage);
         sut.Edited = _editedPage;
      }

      protected override void Because()
      {
         sut.Remove(_editedPage);
      }

      [Observation]
      public void should_set_the_edited_page_to_null()
      {
         sut.Edited.ShouldBeNull();
      }
   }

   public class When_retrieving_the_journal_page_containing_a_related_item : concern_for_Journal
   {
      private JournalPage _journalPage;
      private RelatedItem _relatedItem;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem().WithId("id");
         sut.AddJournalPage(new JournalPage().WithId("2"));
         _journalPage = new JournalPage().WithId("1");
         sut.AddJournalPage(_journalPage);
         _journalPage.AddRelatedItem(_relatedItem);
      }

      [Observation]
      public void should_return_the_journal_containing_the_related_item()
      {
         sut.JournalPageContaining(_relatedItem).ShouldBeEqualTo(_journalPage);
      }

      [Observation]
      public void should_throw_an_exception_otherwise()
      {
         The.Action(() => sut.JournalPageContaining(new RelatedItem())).ShouldThrowAn<Exception>();
      }
   }

   public class When_retrieving_a_related_item_by_id : concern_for_Journal
   {
      private JournalPage _journalPage;
      private RelatedItem _relatedItem;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem().WithId("id");
         sut.AddJournalPage(new JournalPage().WithId("2"));
         _journalPage = new JournalPage().WithId("1");
         sut.AddJournalPage(_journalPage);
         _journalPage.AddRelatedItem(_relatedItem);
      }

      [Observation]
      public void should_return_the_related_item_with_the_given_id_if_defined()
      {
         sut.RelatedItemdById("id").ShouldBeEqualTo(_relatedItem);
      }

      [Observation]
      public void should_return_null_if_no_related_item_is_defined_with_the_givn_id()
      {
         sut.RelatedItemdById("DOES NOT EXIST").ShouldBeNull();
      }
   }

   public class When_retrieving_the_last_created_journal_page : concern_for_Journal
   {
      private JournalPage _lastJournal;

      protected override void Context()
      {
         base.Context();
         _lastJournal = new JournalPage {UniqueIndex = 3};
         sut.AddJournalPage(new JournalPage {UniqueIndex = 1});
         sut.AddJournalPage(_lastJournal);
         sut.AddJournalPage(new JournalPage {UniqueIndex = 2});
      }

      [Observation]
      public void should_return_the_journal_page_with_the_highest_unique_index()
      {
         sut.LastCreatedJournalPage.ShouldBeEqualTo(_lastJournal);
      }
   }

   public class When_retrieving_the_parent_journal_page_of_a_given_journal_page : concern_for_Journal
   {
      private JournalPage _journalPageWithParent;
      private JournalPage _journalPageWithUnknownParent;
      private JournalPage _journalPageWithoutParent;
      private JournalPage _parentJournalPage;

      protected override void Context()
      {
         base.Context();
         _parentJournalPage = new JournalPage().WithId("parent");
         _journalPageWithoutParent = new JournalPage().WithId("childWithoutParent");
         _journalPageWithUnknownParent = new JournalPage {ParentId = "UNKNOWN"}.WithId("childWithUnknownParent");
         _journalPageWithParent = new JournalPage {ParentId = _parentJournalPage.Id}.WithId("childWithParent");

         sut.AddJournalPage(_parentJournalPage);
         sut.AddJournalPage(_journalPageWithParent);
         sut.AddJournalPage(_journalPageWithoutParent);
         sut.AddJournalPage(_journalPageWithUnknownParent);
      }

      [Observation]
      public void should_return_null_if_the_journal_page_has_no_parent()
      {
         sut.ParentOf(_journalPageWithoutParent).ShouldBeNull();
      }

      [Observation]
      public void should_return_null_if_the_journal_page_has_an_unknown_parent()
      {
         sut.ParentOf(_journalPageWithUnknownParent).ShouldBeNull();
      }

      [Observation]
      public void should_return_the_parent_if_the_journal_has_a_defined_parent()
      {
         sut.ParentOf(_journalPageWithParent).ShouldBeEqualTo(_parentJournalPage);
      }
   }

   public class When_removing_a_journal_page_that_is_parent_of_other_pages : concern_for_Journal
   {
      private JournalPage _parentPage;
      private JournalPage _childPage;

      protected override void Context()
      {
         base.Context();
         _parentPage = new JournalPage {Id = "XX"};
         _childPage = new JournalPage {ParentId = _parentPage.Id};
         sut.AddJournalPage(_parentPage);
         sut.AddJournalPage(_childPage);
      }

      protected override void Because()
      {
         sut.Remove(_parentPage);
      }

      [Observation]
      public void should_rmeove_the_reference_to_the_parent_in_all_children_pages()
      {
         _childPage.ParentId.ShouldBeNull();
      }
   }
}