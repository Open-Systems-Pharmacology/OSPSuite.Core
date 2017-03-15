using System;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Journal
{
   public class CustomRichEditCommandFactoryService : IRichEditCommandFactoryService
   {
      readonly IRichEditCommandFactoryService _service;
      private readonly IJournalTask _journalTask;
      private readonly IClipboardTask _clipboardTask;
      readonly RichEditControl _control;
      private readonly Func<JournalPageDTO> _journalPageDTORetriever;

      public CustomRichEditCommandFactoryService(RichEditControl control, IRichEditCommandFactoryService service, IJournalTask journalTask, IClipboardTask clipboardTask,
         Func<JournalPageDTO> journalPageDTORetriever)
      {
         _control = control;
         _service = service;
         _journalTask = journalTask;
         _clipboardTask = clipboardTask;
         _journalPageDTORetriever = journalPageDTORetriever;
      }

      public RichEditCommand CreateCommand(RichEditCommandId id)
      {
         if (id == RichEditCommandId.FileSave)
            return new CustomSaveDocumentCommand(_control, _journalTask, _journalPageDTORetriever);

         if (id == RichEditCommandId.PasteSelection)
            return new CustomPasteSelectionCommand(_control);

         if (id == RichEditCommandId.InsertTable)
            return new CustomInsertTableCommand(_control);

         if(id == RichEditCommandId.CopySelection)
            return new CustomCopySelectionCommand(_control, _clipboardTask);

         return _service.CreateCommand(id);
      }
   }
}