using System;
using System.Text.RegularExpressions;
using OSPSuite.Assets;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO.Journal;

namespace OSPSuite.Presentation.Mappers
{
   public interface IJournalPageToJournalPageDTOMapper
   {
      JournalPageDTO MapFrom(JournalPage journalPage, Func<JournalPage, string> descriptionFunc = null);
      void Update(JournalPageDTO journalPageDTO, JournalPage journalPage, Func<JournalPage, string> descriptionFunc = null);
   }

   public class JournalPageToJournalPageDTOMapper : IJournalPageToJournalPageDTOMapper
   {
      private readonly DateTimeFormatter _dateTimeFormatter;

      public JournalPageToJournalPageDTOMapper()
      {
         _dateTimeFormatter = new DateTimeFormatter();
      }

      public JournalPageDTO MapFrom(JournalPage journalPage, Func<JournalPage, string> descriptionFunc = null)
      {
         var workingJournalItemDTO = new JournalPageDTO(journalPage);
         Update(workingJournalItemDTO, journalPage, descriptionFunc);
         return workingJournalItemDTO;
      }

      public void Update(JournalPageDTO journalPageDTO, JournalPage journalPage, Func<JournalPage, string> descriptionFunc = null)
      {
         if (descriptionFunc == null)
            descriptionFunc = x => x.Description;

         journalPageDTO.Title = journalPage.Title;
         journalPageDTO.CreatedAt = journalPage.CreatedAt;
         journalPageDTO.UpdatedAt = journalPage.UpdatedAt;
         journalPageDTO.Description = descriptionFunc(journalPage);
         journalPageDTO.CreatedBy = journalPage.CreatedBy;
         journalPageDTO.UpdatedBy = journalPage.UpdatedBy;
         journalPageDTO.Tags = journalPage.Tags;
         journalPageDTO.UniqueIndex = journalPage.UniqueIndex;
         journalPageDTO.Origin = journalPage.Origin;
         journalPageDTO.CreatedAtBy = Captions.Journal.CreatedAtBy(_dateTimeFormatter.Format(journalPage.CreatedAt), journalPage.CreatedBy);
         journalPageDTO.UpdatedAtBy = Captions.Journal.UpdatedAtBy(_dateTimeFormatter.Format(journalPage.UpdatedAt), journalPage.UpdatedBy);
         journalPageDTO.LineCount = lineCountFor(journalPageDTO.Description);
      }

      private int lineCountFor(string description)
      {
         if (string.IsNullOrEmpty(description))
            return 0;

         return Math.Max(Regex.Matches(description, "<br>", RegexOptions.IgnoreCase).Count + 1, 1);
      }
   }

}