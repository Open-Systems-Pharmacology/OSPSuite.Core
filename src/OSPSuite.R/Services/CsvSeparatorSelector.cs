using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.R.Services
{
   public class CsvSeparatorSelector : ICsvSeparatorSelector
   {
      public char? GetCsvSeparator(string fileName)
      {
         return ',';
      }
   }
}
