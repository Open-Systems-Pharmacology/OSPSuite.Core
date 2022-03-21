using System;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders
{
   public interface IExcelDataSourceFile : IDataSourceFile
   {
   }

   public class ExcelDataSourceFile : DataSourceFile, IExcelDataSourceFile
   {
      public ExcelDataSourceFile(IImportLogger logger) : base(logger)
      {
      }

      protected override void LoadFromFile(string path)
      {
         DataSheets.Initialize(); //first we clear the sheet collection, in case there were some sheets left from previously loading

         try
         {
            var reader = new ExcelReader(path);

            while (reader.MoveToNextSheet())
            {
               if (!reader.MoveToNextRow()) continue;

               var sheetName = reader.CurrentSheet.SheetName; 
               var rawSheetData = new UnformattedSheetData();
               var headers = reader.CurrentRow;
     
               for (var j = 0; j < headers.Count; j++)
                  rawSheetData.AddColumn(headers[j], j);

               while (reader.MoveToNextRow())
               {
                  //the first two could even be done only once
                  var levels = reader.GetMeasurementLevels(headers.Count);
                  rawSheetData.CalculateColumnDescription(levels);
                  rawSheetData.AddRow(reader.CurrentRow);
               }

               rawSheetData.RemoveEmptyColumns();
               rawSheetData.RemoveEmptyRows();

               DataSheets.AddSheet(sheetName, rawSheetData);
            }
         }
         catch (Exception ex)
         {
            DataSheets.ResetPreviousDataSheets();
            _logger.AddError(ex.Message);
            throw new InvalidObservedDataFileException(ex.Message);
         }
      }
   }
}