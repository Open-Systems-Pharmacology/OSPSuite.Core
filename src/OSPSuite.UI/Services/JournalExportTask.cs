using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Services;
using IDialogCreator = OSPSuite.Core.Services.IDialogCreator;

namespace OSPSuite.UI.Services
{
   public class JournalExportTask : IJournalExportTask
   {
      private readonly IContentLoader _contentLoader;
      private readonly IDialogCreator _dialogCreator;
      private readonly IProjectRetriever _projectRetriever;
      private readonly IRichEditDocumentServerFactory _richEditDocumentServerFactory;
      private float _pageWidth;

      public JournalExportTask(IContentLoader contentLoader, IDialogCreator dialogCreator, IProjectRetriever projectRetriever, 
         IRichEditDocumentServerFactory richEditDocumentServerFactory)
      {
         _contentLoader = contentLoader;
         _dialogCreator = dialogCreator;
         _projectRetriever = projectRetriever;
         _richEditDocumentServerFactory = richEditDocumentServerFactory;
      }

      public void ExportJournalToWordFile(Journal journal)
      {
         var orderedPages = journal.JournalPages.OrderBy(page => page.UniqueIndex).ToList();

         ExportSelectedPagesToWordFile(orderedPages);
      }

      public void ExportSelectedPagesToWordFile(IReadOnlyList<JournalPage> orderedPages)
      {
         var documentServer = _richEditDocumentServerFactory.Create();

         _pageWidth = documentServer.Document.Sections.Any() ? documentServer.Document.Sections.First().Page.Width : Units.InchesToDocumentsF(8.5f);

         if (!orderedPages.Any())
         {
            _dialogCreator.MessageBoxError(Error.NoPagesToExport);
            return;
         }

         addPagesToDocument(orderedPages, documentServer);
         resetRightIndent(documentServer);
         resetLandscape(documentServer);
         exportAndOpenDocument(documentServer);
      }

      private void exportAndOpenDocument(IRichEditDocumentServer documentServer)
      {
         var savedDocument = exportDocumentAsWordFile(documentServer);
         if (string.IsNullOrEmpty(savedDocument))
            return;

         FileHelper.TryOpenFile(savedDocument);
      }

      private void addPagesToDocument(IReadOnlyList<JournalPage> orderedPages, IRichEditDocumentServer documentServer)
      {
         var firstPage = orderedPages.FirstOrDefault();
         insertFirstPage(firstPage, documentServer);

         orderedPages.Except(new[] {firstPage}).Each(page => { addPageToDocument(page, documentServer); });
      }

      private void addPageToDocument(JournalPage page, IRichEditDocumentServer documentServer)
      {
         var document = getDocumentFor(page);
         addTitle(page, document);
         insertPageInNewSection(documentServer, document, page);
      }

      private string exportDocumentAsWordFile(IRichEditDocumentServer documentServer)
      {
         var filePath = getFileNameForExport();

         if (string.IsNullOrEmpty(filePath))
            return string.Empty;

         FileHelper.TrySaveFile(filePath, () => exportFileToPath(documentServer, filePath));
         return filePath;
      }

      private static void exportFileToPath(IRichEditDocumentServer documentServer, string filePath)
      {
         using (var fs = new FileStream(filePath, FileMode.Create))
         {
            documentServer.SaveDocument(fs, DocumentFormat.OpenXml);
         }
      }

      private string getFileNameForExport()
      {
         var defaultFileName = Captions.Journal.ExportWorkingJournalFileName(_projectRetriever.ProjectName);
         var filePath = _dialogCreator.AskForFileToSave(Captions.ExportJournalToWord, Constants.Filter.WORD_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, defaultFileName);
         return filePath;
      }

      private void insertPageInNewSection(IRichEditDocumentServer documentServer, Document document, JournalPage page)
      {
         var section = documentServer.Document.AppendSection();
         createHeader(section, page);
         createFooter(section, page);
         documentServer.Document.AppendDocumentContent(document.Range);
      }

      private void resetLandscape(IRichEditDocumentServer documentServer)
      {
         documentServer.Document.Sections.Each(section =>
         {
            section.Page.Landscape = false;
            section.Page.Width = _pageWidth;
         });
      }

      private static void resetRightIndent(IRichEditDocumentServer documentServer)
      {
         documentServer.Document.Paragraphs.Where(x => Math.Abs(x.RightIndent) > double.Epsilon).Each(x => x.RightIndent = 0);   
      }

      private static void addTitle(JournalPage page, Document document)
      {
         var style = createOutlineStyle(document);
         var range = document.InsertText(document.CreatePosition(0), $"{createPageTitle(page)}{Environment.NewLine}");
         var paragraphProperties = document.BeginUpdateParagraphs(range);
         paragraphProperties.Style = style;
      }

      private static string createPageTitle(JournalPage page)
      {
         return $"{page.UniqueIndex} - {page.Title}";
      }

      private static ParagraphStyle createOutlineStyle(Document document)
      {
         var style = document.ParagraphStyles.CreateNew();
         document.ParagraphStyles.Add(style);
         style.OutlineLevel = 1;
         style.FontSize = 14;
         style.LineSpacingMultiplier = 1.5f;
         return style;
      }

      private void insertFirstPage(JournalPage firstPage, IRichEditDocumentServer documentServer)
      {
         var document = documentServer.Document;
         
         loadContentFor(firstPage);
         loadDocumentServer(firstPage, documentServer);

         addTitle(firstPage, document);

         var section = document.GetSection(document.CaretPosition);
         createFooter(section, firstPage);
         createHeader(section, firstPage);
      }

      private void fixTablePreferredWidths(Document document)
      {
         foreach (var table in document.Tables)
         {
            if (shouldNotChangePreferredWidths(table))
               continue;

            table.TableLayout = TableLayoutType.Autofit;
            table.ForEachCell((cell, rowIndex, columnIndex) => cell.PreferredWidthType = WidthType.Auto);
         }
      }

      private static bool shouldNotChangePreferredWidths(Table table)
      {
         return table.NestingLevel != 0 || table.TableLayout == TableLayoutType.Autofit;
      }

      private void createHeader(Section section, JournalPage page)
      {
         section.UnlinkHeaderFromPrevious();
         var header = section.BeginUpdateHeader();
         clearSubDocument(header);
         header.InsertText(header.CreatePosition(0), $"Page Title: {createPageTitle(page)}");
         section.EndUpdateHeader(header);
      }

      private void clearSubDocument(SubDocument document)
      {
         document.Delete(document.Range);
      }

      private void createFooter(Section section, JournalPage page)
      {
         section.UnlinkFooterFromPrevious();

         var footer = section.BeginUpdateFooter();
         clearSubDocument(footer);
         if (page.Tags.Any())
         {
            footer.InsertText(footer.CreatePosition(0), $"Page Tags: {page.Tags.ToString(", ")}");
         }
         section.EndUpdateFooter(footer);
      }

      private Document getDocumentFor(JournalPage page)
      {
         var documentServer = new RichEditDocumentServer();
         loadContentFor(page);
         loadDocumentServer(page, documentServer);
         return documentServer.Document;
      }

      private void loadDocumentServer(JournalPage page, IRichEditDocumentServer documentServer)
      {
         documentServer.Document.OpenXmlBytes = page.Content.Data;
         fixTablePreferredWidths(documentServer.Document);
      }

      private void loadContentFor(JournalPage page)
      {
         _contentLoader.Load(page);
      }
   }
}
