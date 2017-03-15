using System.IO;
using OSPSuite.Utility;
using DevExpress.XtraRichEdit;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Views;
using OSPSuite.UI.Services;

namespace OSPSuite.Starter.Presenters
{
   public interface IJournalRichEditFormPresenter : IPresenter<IJournalRichEditFormView>, IDisposablePresenter
   {
      void ShowJournalPages(JournalPage journalPage);
      void ExportToWord(byte[] openXmlBytes);
   }

   public class JournalRichEditFormPresenter : AbstractDisposablePresenter<IJournalRichEditFormView, IJournalRichEditFormPresenter>, IJournalRichEditFormPresenter
   {
      private readonly IRichEditDocumentServerFactory _richEditDocumentServerFactory;
      private readonly IDialogCreator _dialogCreator;

      public JournalRichEditFormPresenter(IJournalRichEditFormView view, IRichEditDocumentServerFactory richEditDocumentServerFactory, IDialogCreator dialogCreator) : base(view)
      {
         _richEditDocumentServerFactory = richEditDocumentServerFactory;
         _dialogCreator = dialogCreator;
      }

      public void ShowJournalPages(JournalPage journalPage)
      {
         _view.SetPageContent(journalPage.Content.Data);
         _view.Display();
      }

      public void ExportToWord(byte[] openXmlBytes)
      {
         var documentServer = _richEditDocumentServerFactory.Create();
         insertFirstPage(openXmlBytes, documentServer);
         exportDocumentAsWordFile(documentServer);
      }

      private void insertFirstPage(byte[] openXmlBytes, IRichEditDocumentServer documentServer)
      {
         var document = documentServer.Document;
         document.OpenXmlBytes = openXmlBytes;
      }

      private string exportDocumentAsWordFile(IRichEditDocumentServer documentServer)
      {
         var filePath = getFileNameForExport();

         if (string.IsNullOrEmpty(filePath))
            return string.Empty;

         FileHelper.TrySaveFile(filePath, () => exportFileToPath(documentServer, filePath));
         return filePath;
      }

      private string getFileNameForExport()
      {
         var defaultFileName = Captions.Journal.ExportWorkingJournalFileName("Project");
         return _dialogCreator.AskForFileToSave(Captions.ExportJournalToWord, Constants.Filter.WORD_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, defaultFileName);
      }

      private static void exportFileToPath(IRichEditDocumentServer documentServer, string filePath)
      {
         using (var fs = new FileStream(filePath, FileMode.Create))
         {
            documentServer.SaveDocument(fs, DocumentFormat.OpenXml);
         }
      }
   }
}
