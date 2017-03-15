using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_JournalPageDTO : ContextSpecification<JournalPageDTO>
   {
      protected JournalPage _journalPage;

      protected override void Context()
      {
         _journalPage = new JournalPage();
         sut = new JournalPageDTO(_journalPage);
      }
   }

   public class validating_new_tags_for_journal_page_dto : concern_for_JournalPageDTO
   {
      [Observation]
      public void should_reject_changes_that_are_only_separator_character()
      {
         sut.ValidateTag(JournalPageDTO.Separator).ShouldBeFalse();
      }

      [Observation]
      public void should_reject_tags_that_are_empty()
      {
         sut.ValidateTag("").ShouldBeFalse();
      }
   }
}