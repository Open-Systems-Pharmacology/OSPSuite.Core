using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public class CsvDataSourceFile : DataSourceFile
   {
      public CsvDataSourceFile(string path, IImportLogger logger) : base(path, logger) { }

      override protected Dictionary<string, IDataSheet> LoadFromFile(string path)
      {
         try
         {
            using (var reader = new CsvReaderDisposer(path))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();
               var rows = new List<List<string>>(headers.Length);

               IDataSheet dataSheet = new DataSheet();
               dataSheet.RawData = new UnformattedData();

               for (var i = 0; i < headers.Length; i++)
                  dataSheet.RawData.AddColumn(headers[i], i);
               string[] currentRow = new string[csv.FieldCount];

               while (csv.ReadNextRecord())
               {
                  csv.CopyCurrentRecordTo(currentRow);
                  dataSheet.RawData.AddRow(currentRow.ToList());
               }

               var loadedData = new Dictionary<string, IDataSheet>();
               loadedData.Add("", dataSheet);
               return loadedData;
            }
         }
         catch (Exception e)
         {
            logger.AddError(e.ToString());
            return null;
         }
      }
   }
}
