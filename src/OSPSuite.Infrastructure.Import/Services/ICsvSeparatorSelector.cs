namespace OSPSuite.Infrastructure.Import.Services
{
   public interface ICsvSeparatorSelector
   {
      /// <summary>
      /// Returns the separator to be used for the reading of the .csv file
      /// </summary>
      char? GetCsvSeparator(string fileName);
   }
}
