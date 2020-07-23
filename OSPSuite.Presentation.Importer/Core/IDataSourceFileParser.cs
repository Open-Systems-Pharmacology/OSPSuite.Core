using OSPSuite.Presentation.Importer.Core.DataSourceFileReaders;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IDataSourceFileParser
   {
      IDataSourceFile For(string path);
   }

   public class DataSourceFileParser : IDataSourceFileParser
   {
      private readonly ICsvDataSourceFile _csvDataSourceFile;
      private readonly IExcelDataSourceFile _excelDataSourceFile;

      public DataSourceFileParser(ICsvDataSourceFile csvDataSourceFile, IExcelDataSourceFile excelDataSourceFile)
      {
         _csvDataSourceFile = csvDataSourceFile;
         _excelDataSourceFile = excelDataSourceFile;
      }

      public IDataSourceFile For(string path)
      {
         if (path.EndsWith(".csv"))
         {
            _csvDataSourceFile.Path = path;
            return _csvDataSourceFile;
         }
         else if (path.EndsWith(".xls") || path.EndsWith(".xlsx"))
         {
            _excelDataSourceFile.Path = path;
            return _excelDataSourceFile;
         }

         throw new UnsupportedFormatException();
      }
   }
}