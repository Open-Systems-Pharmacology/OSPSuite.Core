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
            }
         }
         catch (Exception e)
         {
            logger.AddError(e.ToString());
            return null;
         }
         return null;
      }
   }
}
