using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    e.g. a sheet in an excel file
   /// </summary>
   public class DataSheetCollection
   {
      private Cache<string, DataSheet> _dataSheets;
      private Cache<string, DataSheet> _previouslyLoadedDataSheets;

      public DataSheetCollection()
      {
         _dataSheets = new Cache<string, DataSheet>();
         _previouslyLoadedDataSheets = new Cache<string, DataSheet>();
      }
      public void Initialize()
      {
         _previouslyLoadedDataSheets = _dataSheets;
         _dataSheets = new Cache<string, DataSheet>();
      }

      public void AddSheet(string sheetName, DataSheet dataSheet)
      {
         _dataSheets.Add(sheetName, dataSheet);
      }

      public void ResetPreviousDataSheets()
      {
         _dataSheets = _previouslyLoadedDataSheets;
      }

      public void GetDataSheet(string sheetName)
      {
         var test = _dataSheets[sheetName]; //OK, what if we do not find this?
      }
   }
}