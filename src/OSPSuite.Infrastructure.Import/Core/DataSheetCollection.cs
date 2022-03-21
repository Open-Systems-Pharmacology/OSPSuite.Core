using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    e.g. a sheet in an excel file
   /// </summary>
   public class DataSheetCollection
   {
      private Cache<string, DataSheet> _dataSheets;
   }
}