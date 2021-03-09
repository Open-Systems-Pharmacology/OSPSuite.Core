using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders;

namespace OSPSuite.Infrastructure.Import.Core
{
   public interface IDataSourceFileParser
   {
      IDataSourceFile For(string path);
   }

   public class DataSourceFileParser : IDataSourceFileParser
   {
      private readonly string[] _csvExtensions = { Constants.Filter.CSV_EXTENSION };
      private readonly string[] _excelExtensions = { Constants.Filter.XLS_EXTENSION, Constants.Filter.XLSX_EXTENSION };

      private readonly ICsvDataSourceFile _csvDataSourceFile;
      private readonly IExcelDataSourceFile _excelDataSourceFile;

      public DataSourceFileParser(ICsvDataSourceFile csvDataSourceFile, IExcelDataSourceFile excelDataSourceFile)
      {
         _csvDataSourceFile = csvDataSourceFile;
         _excelDataSourceFile = excelDataSourceFile;
      }

      public IDataSourceFile For(string path)
      {
         if (_csvExtensions.Any(path.EndsWith))
         {
            _csvDataSourceFile.Path = path;
            return _csvDataSourceFile;
         }
         if (_excelExtensions.Any(path.EndsWith))
         {
            _excelDataSourceFile.Path = path;
            return _excelDataSourceFile;
         }

         throw new UnsupportedFormatException();
      }
   }
}