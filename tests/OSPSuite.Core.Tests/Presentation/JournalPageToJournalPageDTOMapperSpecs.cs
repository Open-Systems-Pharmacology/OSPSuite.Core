using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Mappers;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_JournalPageToJournalPageDTOMapper : ContextSpecification<IJournalPageToJournalPageDTOMapper>
   {
      protected JournalPageDTO _dto;
      protected JournalPage _journalPage;

      protected override void Context()
      {
         sut = new JournalPageToJournalPageDTOMapper();

         _journalPage = new JournalPage
         {
            CreatedAt = new DateTime(2010, 03, 24),
            UpdatedAt = new DateTime(2007, 03, 30),
            CreatedBy = "XX",
            UpdatedBy = "YY",
            Title = "BLAH",
            Description = "TOTO",
            UniqueIndex = 5
         };
      }
   }

   public class When_mapping_a_journal_item_to_a_journal_item_dto : concern_for_JournalPageToJournalPageDTOMapper
   {
      protected override void Because()
      {
         _dto = sut.MapFrom(_journalPage);
      }

      [Observation]
      public void should_return_a_dto_having_the_expected_values_mapped_from_the_journal_item()
      {
         _dto.CreatedAt.ShouldBeEqualTo(_journalPage.CreatedAt);
         _dto.Title.ShouldBeEqualTo(_journalPage.Title);
         _dto.UniqueIndex.ShouldBeEqualTo(_journalPage.UniqueIndex);
         _dto.UpdatedAt.ShouldBeEqualTo(_journalPage.UpdatedAt);
         _dto.CreatedBy.ShouldBeEqualTo(_journalPage.CreatedBy);
         _dto.UpdatedBy.ShouldBeEqualTo(_journalPage.UpdatedBy);
         _dto.CreatedAtBy.ShouldNotBeNull();
         _dto.UpdatedAtBy.ShouldNotBeNull();
      }

      [Observation]
      public void should_use_the_description_field_for_description_if_value_was_not_provided()
      {
         _dto.Description.ShouldBeEqualTo(_journalPage.Description);
      }
   }

   public class When_mapping_a_journal_item_to_a_journal_item_dto_using_a_description_func : concern_for_JournalPageToJournalPageDTOMapper
   {
      protected override void Because()
      {
         _dto = sut.MapFrom(_journalPage, x => "NEW DESC");
      }

      [Observation]
      public void should_use_the_description_field_for_description_if_value_was_not_provided()
      {
         _dto.Description.ShouldBeEqualTo("NEW DESC");
      }
   }
}