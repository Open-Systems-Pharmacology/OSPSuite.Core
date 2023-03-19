using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.R.Services
{
   public interface ICsvDynamicSeparatorSelector : ICsvSeparatorSelector
   {
      CSVSeparators CsvSeparators { set; }
   }

   /// <summary>
   ///    This class is used by the CsvDataSourceFile ro correctly parse csv files
   /// </summary>
   public class CsvSeparatorSelector : ICsvDynamicSeparatorSelector
   {
      public CSVSeparators CsvSeparators { get; set; }

      public CSVSeparators GetCsvSeparator(string fileName)
      {
         return CsvSeparators;
      }
   }
}