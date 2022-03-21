namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    e.g. a sheet in an excel file
   /// </summary>
   public class DataSheet //ToDo: if it is being used everywhere, just make the sheetName part of the Unformatted  Data
   {
      public DataSheet(string sheetName, UnformattedSheetData rawSheetData)
      {
         SheetName = sheetName;
         RawSheetData = rawSheetData;
      }

      public string SheetName { get; set; }
      public UnformattedSheetData RawSheetData { get; set; }
   }
}