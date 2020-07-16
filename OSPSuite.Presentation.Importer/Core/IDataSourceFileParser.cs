using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Importer.Core.DataSourceFileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IDataSourceFileParser
   {
      IDataSourceFile For(string path);
   }

   public class DataSourceFileParser
   {
      private readonly IImportLogger importLogger;

      public DataSourceFileParser(IImportLogger importLogger)
      {
         this.importLogger = importLogger;
      }

      public IDataSourceFile For(string path)
      {
         if (path.EndsWith(".csv"))
         {
            return new CsvDataSourceFile(path, importLogger);
         }
         else if (path.EndsWith(".xls") || path.EndsWith(".xlsx"))
         {
            return new ExcelDataSourceFile(path, importLogger);
         }
         throw new UnsuportedFormatException();
      }
   }
}
