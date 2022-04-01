using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    a collection of excel dataSheet sheets
   /// </summary>
   public class DataSheetCollection : IEnumerable<DataSheet>
   {
      private readonly Cache<string, DataSheet> _dataSheets;

      public DataSheetCollection()
      {
         _dataSheets = new Cache<string, DataSheet>(x => x.SheetName);
      }

      public void AddSheet(DataSheet sheet)
      {
         _dataSheets.Add(sheet);
      }

      public DataSheet GetDataSheet(string sheetName)
      {
         return _dataSheets[sheetName];
      }

      public DataSheetCollection GetDataSheets(IReadOnlyList<string> sheetNames)
      {
         var sheets = new DataSheetCollection();

         foreach (var sheetName in sheetNames)
         {
            if (_dataSheets.Contains(sheetName))
               sheets.AddSheet(_dataSheets[sheetName]);
         }

         return sheets;
      }

      
      public IEnumerator<DataSheet> GetEnumerator()
      {
         return _dataSheets.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return _dataSheets.GetEnumerator();
      }

      public DataSheetCollection Filter(string filter)
      {
         var filteredDataSheets = new DataSheetCollection();
         foreach (var key in _dataSheets.Keys)
         {
            var dt = _dataSheets[key].AsDataTable();
            var dv = new DataView(dt);
            dv.RowFilter = filter;
            var list = new List<DataRow>();
            var ds = new DataSheet(_dataSheets[key]);
            foreach (DataRowView drv in dv)
            {
               ds.AddRow(drv.Row.ItemArray.Select(c => c.ToString()));
            }
            filteredDataSheets.AddSheet(ds);
         }

         return filteredDataSheets;
      }

      public Cache<string, IDataSet> GetDataSets(IDataFormat format, ColumnInfoCache columnInfos)
      {
         var dataSets = new Cache<string, IDataSet>();

         foreach (var sheetKeyValue in _dataSheets.KeyValues)
         {
            var data = new DataSet();
            data.AddData(format.Parse(sheetKeyValue.Value, columnInfos));
            dataSets.Add(sheetKeyValue.Key, data);
         }

         return dataSets;
      }

      public IReadOnlyList<string> AddNotExistingSheets(DataSheetCollection existingDataSheets)
      {
         var sheetNames = new List<string>();
         foreach (var sheet in _dataSheets)
         {
            if (existingDataSheets.Contains(sheet.SheetName))
               continue;

            _dataSheets.Add(sheet);
            sheetNames.Add(sheet.SheetName);
         }

         return sheetNames;
      }

      public bool Contains(string sheetName)
      {
         return _dataSheets.Contains(sheetName);
      }

      public void Remove(string sheetName)
      {
         _dataSheets.Remove(sheetName);
      }

      public void Clear()
      {
         _dataSheets.Clear();
      }

      public IReadOnlyList<string> GetDataSheetNames()
      {
         return _dataSheets.Keys.ToList();
      }
   }
}