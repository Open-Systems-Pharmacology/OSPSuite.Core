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
               dataSheet.RawData = new UnformattedData();
               var headers = reader.CurrentRow;
     
               for (var j = 0; j < headers.Count; j++)
                  dataSheet.RawData.AddColumn(headers[j], j);

               while (reader.MoveToNextRow())
               {
                  //the first two could even be done only once
                  var levels = reader.GetMeasurementLevels(headers.Count);
                  dataSheet.RawData.CalculateColumnDescription(levels);
                  dataSheet.RawData.AddRow(reader.CurrentRow);
               }

               dataSheet.RawData.RemoveEmptyColumns();
               dataSheet.RawData.RemoveEmptyRows();

               loadedData.Add(sheetName, dataSheet);
            }

            return loadedData;
         }
         catch (Exception ex)
         {
            _logger.AddError(ex.ToString());
            throw new InvalidFileException();
         }
      }
   }
}