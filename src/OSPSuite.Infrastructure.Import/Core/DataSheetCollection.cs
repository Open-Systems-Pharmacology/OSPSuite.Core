using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    e.g. a sheet in an excel file
   /// </summary>
   public class DataSheetCollection //: IEnumerable<UnformattedSheetData>
   {
      private Cache<string, UnformattedSheetData> _dataSheets;
      private Cache<string, UnformattedSheetData> _previouslyLoadedDataSheets;

      public DataSheetCollection()
      {
         _dataSheets = new Cache<string, UnformattedSheetData>();
         _previouslyLoadedDataSheets = new Cache<string, UnformattedSheetData>();
      }
      public void Initialize()
      {
         _previouslyLoadedDataSheets = _dataSheets;
         _dataSheets = new Cache<string, UnformattedSheetData>();
      }

      public void AddSheet(string sheetName, UnformattedSheetData dataSheet)
      {
         _dataSheets.Add(sheetName, dataSheet);
      }

      public void ResetPreviousDataSheets()
      {
         _dataSheets = _previouslyLoadedDataSheets;
      }

      public DataSheet GetDataSheet(string sheetName)
      {
         var rawData = _dataSheets[sheetName]; //OK, what if we do not find this?
         return new DataSheet(sheetName, rawData);
      }

      /*
      public IEnumerator<UnformattedSheetData> GetEnumerator()
      {
         return _dataSheets.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return _dataSheets.GetEnumerator();
      }
*/
      public DataSheetCollection Filter(string filter)
      {
         var filteredDataSheets = new DataSheetCollection();
         foreach (var key in _dataSheets.Keys)
         {
            var dt = _dataSheets[key].AsDataTable();
            var dv = new DataView(dt);
            dv.RowFilter = filter;
            var list = new List<DataRow>();
            var ds = new UnformattedSheetData(_dataSheets[key]);
            foreach (DataRowView drv in dv)
            {
               ds.AddRow(drv.Row.ItemArray.Select(c => c.ToString()));
            }
            filteredDataSheets.AddSheet(key, ds);
         }

         return filteredDataSheets;
      }
   }
}