using System.Collections.Generic;
using OSPSuite.Core.Journal;

namespace OSPSuite.Presentation.Services
{
   public interface IJournalExportTask
   {
      /// <summary>
      /// Exports all the pages of a <paramref name="journal"/> to a file
      /// </summary>
      void ExportJournalToWordFile(Journal journal);

      /// <summary>
      /// Exports a list of pages to  a word file in the order they occur in the <paramref name="orderedPages"/>
      /// </summary>
      void ExportSelectedPagesToWordFile(IReadOnlyList<JournalPage> orderedPages);
   }
}
