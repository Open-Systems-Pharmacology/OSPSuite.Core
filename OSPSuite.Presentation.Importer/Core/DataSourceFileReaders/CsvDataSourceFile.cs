using System;
using System.Collections.Generic;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public class CsvDataSourceFile : DataSourceFile
   {
      private readonly IImportLogger logger;
      public CsvDataSourceFile(string path, IImportLogger logger) : base(path, logger) { }

      override protected Dictionary<string, IDataSheet> LoadFromFile(string path)
      {
         throw new NotImplementedException(); 
         /*
         try
         {
            using (var reader = new CsvReaderDisposer(path))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();
               IDataSheet dataSheet = new DataSheet();
               dataSheet.RawData = new Dictionary<string, IList<string>>();
               var rows = new List<List<string>>(headers.Length);
               for (var i = 0; i < headers.Length; i++)
                  rows.Add(new List<string>());
               while (csv.ReadNextRecord())
               {
                  for (var i = 0; i < headers.Length; i++)
                     rows[i].Add(csv[i]);
               }
               for (var i = 0; i < headers.Length; i++)
                  dataSheet.RawData.Add(headers[i], rows[i]);
               var loadedData = new Dictionary<string, IDataSheet>();
               loadedData.Add("", dataSheet);
               return loadedData;
            }
         }
         catch (Exception e)
         {
            logger.AddError(e.ToString());
            return null;
         }*/
      }
   }
}
