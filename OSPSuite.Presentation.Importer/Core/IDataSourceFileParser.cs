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
      private readonly ICsvDataSourceFile csvDataSourceFile;
      private readonly IExcelDataSourceFile excelDataSourceFile;

      public DataSourceFileParser(ICsvDataSourceFile csvDataSourceFile, IExcelDataSourceFile excelDataSourceFile)
      {
         
      }

      public IDataSourceFile For(string path)
      {
         if (path.EndsWith(".csv"))
         {
            csvDataSourceFile.Path = path;
            return csvDataSourceFile;
         }
         else if (path.EndsWith(".xls") || path.EndsWith(".xlsx"))
         {
            excelDataSourceFile.Path = path;
            return excelDataSourceFile;
         }
         throw new UnsuportedFormatException();
      }
   }
}
