using System;
using System.Collections.Generic;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public class CsvDataSourceFile : DataSourceFile
   {
      private readonly IImportLogger logger;
      public CsvDataSourceFile(string path, IImportLogger logger) : base(path) 
      {
         this.logger = logger;
      }

      override protected Dictionary<string, IDataTable> LoadFromFile(string path)
      {
         try
         {
            using (var reader = new CsvReaderDisposer(path))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();
               IDataTable dataTable = new DataTable();
               dataTable.RawData = new Dictionary<string, IList<string>>();
               var rows = new List<List<string>>(headers.Length);
               for (var i = 0; i < headers.Length; i++)
                  rows.Add(new List<string>());
               while (csv.ReadNextRecord())
               {
                  for (var i = 0; i < headers.Length; i++)
                     rows[i].Add(csv[i]);
               }
               for (var i = 0; i < headers.Length; i++)
                  dataTable.RawData.Add(headers[i], rows[i]);
               var loadedData = new Dictionary<string, IDataTable>();
               loadedData.Add("", dataTable);
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
