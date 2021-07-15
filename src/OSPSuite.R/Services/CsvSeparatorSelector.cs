using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.R.Services
{
   public interface ICsvDynamicSeparatorSelector : ICsvSeparatorSelector
   {
      char? CsvSeparator { set; }
   }

   /// <summary>
   /// This class is used by the CsvDataSourceFile ro correclty parse csv files
   /// </summary>
   public class CsvSeparatorSelector : ICsvDynamicSeparatorSelector
   {
      public char? CsvSeparator { get; set; }

      public char? GetCsvSeparator(string fileName)
      {
         return CsvSeparator;
      }
   }
}
