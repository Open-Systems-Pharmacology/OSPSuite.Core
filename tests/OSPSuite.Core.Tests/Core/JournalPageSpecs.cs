using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;

namespace OSPSuite.Core
{
   public abstract class concern_for_JournalPage : ContextSpecification<JournalPage>
   {
      protected override void Context()
      {
         sut = new JournalPage();
      }
   }

   public class When_setting_the_tags_in_the_journal_page_using_the_same_list_as_the_existing_tags : concern_for_JournalPage
   {
      protected override void Context()
      {
         base.Context();
         sut.AddTag("A");
         sut.AddTag("B");
      }

      protected override void Because()
      {
         sut.UpdateTags(sut.Tags);
      }

      [Observation]
      public void should_contain_the_expected_tags()
      {
         sut.Tags.ShouldOnlyContain("A", "B");
      }
   }

   public class When_setting_the_tags_in_the_journal_page_using_a_new_list_of_tags : concern_for_JournalPage
   {
      protected override void Context()
      {
         base.Context();
         sut.AddTag("A");
         sut.AddTag("B");
      }

      protected override void Because()
      {
         sut.UpdateTags(new[] {"B", "C"});
      }

      [Observation]
      public void should_contain_the_expected_tags()
      {
         sut.Tags.ShouldOnlyContain("B", "C");
      }
   }
}