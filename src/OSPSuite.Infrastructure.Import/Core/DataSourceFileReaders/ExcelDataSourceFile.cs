using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OSPSuite.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders
{
   public interface IExcelDataSourceFile : IDataSourceFile
   {
   }

   public class ExcelDataSourceFile : DataSourceFile, IExcelDataSourceFile
   {
      public ExcelDataSourceFile(IImportLogger logger, IHeavyWorkManager heavyWorkManager) : base(logger, heavyWorkManager)
      {
      }

      protected override void LoadFromFile(string path, CancellationToken cancellationToken = default)
      {
         //we keep a copy of the already loaded sheets, in case the reading fails
         var alreadyLoadedDataSheets = DataSheets.Clone();
         DataSheets.Clear();

         try
         {
            var reader = new ExcelReader(path);

            while (reader.MoveToNextSheet())
            {
               if (!reader.MoveToNextRow()) continue;

               var rawSheetData = new DataSheet
               {
                  SheetName = reader.CurrentSheet.SheetName
               };
               var headers = reader.CurrentRow;

               checkSheetForDuplicateHeaders(headers, reader);

               for (var j = 0; j < headers.Count; j++)
                  rawSheetData.AddColumn(headers[j], j);

               while (reader.MoveToNextRow())
               {
                  cancellationToken.ThrowIfCancellationRequested();

                  var levels = reader.GetMeasurementLevels(headers.Count);
                  rawSheetData.CalculateColumnDescription(levels);
                  rawSheetData.AddRow(reader.CurrentRow);
               }

               rawSheetData.RemoveEmptyColumns();
               rawSheetData.RemoveEmptyRows();

               DataSheets.AddSheet(rawSheetData);
            }

            //if the file was empty
            if (DataSheets.GetDataSheetNames().Count == 0)
               throw new ImporterEmptyFileException();
         }
         catch (Exception ex)
         {
            DataSheets.CopySheetsFrom(alreadyLoadedDataSheets);
            _logger.AddError(ex.Message);
            if (ex is OperationCanceledException)
               throw;
            else
               throw new InvalidObservedDataFileException(ex.Message);
         }
      }

      private static void checkSheetForDuplicateHeaders(List<string> headers, ExcelReader reader)
      {
         var headerDuplicates = headers.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key).ToList();

         //since an empty header could have multiple occurrences we remove from the duplicate list
         headerDuplicates.Remove("");

         if (headerDuplicates.Count() != 0)
         {
            throw new DataFileWithDuplicateHeaderException(Error.SheetWithDuplicateHeader(reader.CurrentSheet.SheetName, headerDuplicates));
         }
      }
   }
}