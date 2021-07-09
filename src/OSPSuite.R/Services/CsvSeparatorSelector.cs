using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.R.Services
{
   /// <summary>
   /// This class is used by the CsvDataSourceFile ro correclty parse csv files
   /// </summary>
   public class CsvSeparatorSelector : ICsvSeparatorSelector
   {
      public char? CsvSeparator { get; set; }

      public char? GetCsvSeparator(string fileName)
      {
         return CsvSeparator;
      }
   }
}
