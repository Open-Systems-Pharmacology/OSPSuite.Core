using System;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;

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

      protected override Cache<string, DataSheet> LoadFromFile(string path)
      {
         try
         {
            var loadedData = new Cache<string, DataSheet>();

            var reader = new ExcelReader(path);

            while (reader.MoveToNextSheet())
            {
               if (!reader.MoveToNextRow()) continue;

               var sheetName = reader.CurrentSheet.SheetName;
               DataSheet dataSheet = new DataSheet();
               dataSheet.RawSheetData = new UnformattedSheetData();
               var headers = reader.CurrentRow;
     
               for (var j = 0; j < headers.Count; j++)
                  dataSheet.RawSheetData.AddColumn(headers[j], j);

               while (reader.MoveToNextRow())
               {
                  //the first two could even be done only once
                  var levels = reader.GetMeasurementLevels(headers.Count);
                  dataSheet.RawSheetData.CalculateColumnDescription(levels);
                  dataSheet.RawSheetData.AddRow(reader.CurrentRow);
               }

               dataSheet.RawSheetData.RemoveEmptyColumns();
               dataSheet.RawSheetData.RemoveEmptyRows();

               loadedData.Add(sheetName, dataSheet);
            }

            return loadedData;
         }
         catch (Exception ex)
         {
            _logger.AddError(ex.Message);
            throw new InvalidObservedDataFileException(ex.Message);
         }
      }
   }
}