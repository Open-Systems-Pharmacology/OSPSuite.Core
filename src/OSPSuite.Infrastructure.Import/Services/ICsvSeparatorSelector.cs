namespace OSPSuite.Infrastructure.Import.Services
{
   public class CSVSeparators
   {
      public char ColumnSeparator { get; set; }
      public char DecimalSeparator { get; set; }
   }

   public interface ICsvSeparatorSelector
   {
      /// <summary>
      ///    Returns the separator to be used for the reading of the .csv file
      /// </summary>
      CSVSeparators GetCsvSeparator(string fileName);
   }
}