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

            IDataSheet dataSheet = new DataSheet();
            while (reader.MoveToNextSheet())
            {
               var sheetName = reader.CurrentSheet.SheetName;
               var headers = new List<string>();
               dataSheet.RawData = new UnformattedData();

               if (reader.MoveToNextRow())
                  headers = reader.CurrentRow;
               //else probably throw exception

               for (var j = 0; j < headers.Count; j++)
                  dataSheet.RawData.AddColumn(headers[j], j);

               while (reader.MoveToNextRow())
               {
                  //the first two could even be done only once
                  var levels = reader.GetMeasurementLevels();
                  dataSheet.RawData.CalculateColumnDescription(levels);
                  dataSheet.RawData.AddRow(reader.CurrentRow);
               }

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