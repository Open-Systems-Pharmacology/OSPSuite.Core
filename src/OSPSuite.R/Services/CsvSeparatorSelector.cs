using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.R.Services
{
   public interface ICsvDynamicSeparatorSelector : ICsvSeparatorSelector
   {
      (char? ColumnSeparator, char DecimalSeparator) CsvSeparators { set; }
   }

   /// <summary>
   ///    This class is used by the CsvDataSourceFile ro correctly parse csv files
   /// </summary>
   public class CsvSeparatorSelector : ICsvDynamicSeparatorSelector
   {
      public (char? ColumnSeparator, char DecimalSeparator) CsvSeparators { get; set; }

      public (char? ColumnSeparator, char DecimalSeparator) GetCsvSeparator(string fileName)
      {
         return CsvSeparators;
      }
   }
}