using System;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Commands;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;

namespace OSPSuite.UI.Views.Journal
{
   public class CustomSaveDocumentCommand : SaveDocumentCommand
   {
      private readonly RichEditControl _control;
      private readonly IJournalTask _journalTask;
      private readonly Func<JournalPageDTO> _journalPageDTORetriever;

      public CustomSaveDocumentCommand(
         RichEditControl control,
         IJournalTask journalTask,
         Func<JournalPageDTO> journalPageDTORetriever)
         : base(control)
      {
         _control = control;
         _journalTask = journalTask;
         _journalPageDTORetriever = journalPageDTORetriever;
      }

      protected override void ExecuteCore()
      {
         var journalPageDTO = _journalPageDTORetriever();
         _control.Modified = false;

         if (journalPageDTO == null) return;

         updateWorkingJournalItem(journalPageDTO, _control.Document.Range);
         _journalTask.SaveJournalPage(journalPageDTO.JournalPage);
         journalPageDTO.Edited = false;
      }

      private void updateWorkingJournalItem(JournalPageDTO journalPageDTO, DocumentRange documentRange)
      {
         var journalPage = journalPageDTO.JournalPage;
         var fullText = _control.Document.GetText(documentRange);
         journalPage.Description = createItemDescription(fullText);
         journalPage.Title = journalPageDTO.Title;
         journalPage.Origin = journalPageDTO.Origin;
         journalPage.FullText = fullText;
         journalPage.Content.Data = _control.Document.GetOpenXmlBytes(documentRange);
         journalPage.UpdateTags(journalPageDTO.Tags);
      }

      private string createItemDescription(string fullText)
      {
         return _journalTask.CreateItemDescriptionFromContent(fullText);
      }
   }
}