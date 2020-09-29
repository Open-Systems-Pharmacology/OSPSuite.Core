using System;
using System.Collections.Generic;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Importer.Infrastructure;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public interface IExcelDataSourceFile : IDataSourceFile
   {
   }

   public class ExcelDataSourceFile : DataSourceFile, IExcelDataSourceFile
   {
      public ExcelDataSourceFile(IImportLogger logger) : base(logger)
      {
      }

      protected override Dictionary<string, IDataSheet> LoadFromFile(string path)
      {
         try
         {
            var loadedData = new Dictionary<string, IDataSheet>();


            var reader = new ExcelReader(path);

            while (reader.MoveToNextSheet())
            {
               if (!reader.MoveToNextRow()) continue;

               var sheetName = reader.CurrentSheet.SheetName;
               IDataSheet dataSheet = new DataSheet();
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

               loadedData.Add(sheetName, dataSheet);
            }

            return loadedData;
         }
         catch (Exception ex)
         {
            _logger.AddError(ex.ToString());
            return null;
         }
      }
   }
}