using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    a collection of excel dataSheet sheets
   /// </summary>
   public class DataSheetCollection : IEnumerable<DataSheet>
   {
      private readonly Cache<string, DataSheet> _dataSheets = new Cache<string, DataSheet>(x => x.SheetName);

      public DataSheetCollection Clone()
      {
         var clone = new DataSheetCollection();
         _dataSheets.Each(sheet => clone.AddSheet(sheet));
         return clone;
      }

      public void AddSheet(DataSheet sheet)
      {
         _dataSheets.Add(sheet);
      }

      public DataSheet GetDataSheetByName(string sheetName)
      {
         return _dataSheets[sheetName];
      }

      public DataSheetCollection GetDataSheetsByName(IReadOnlyList<string> sheetNames)
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
            var dt = _dataSheets[key].ToDataTable();
            var dv = new DataView(dt);
            dv.RowFilter = filter;
            var ds = new DataSheet(_dataSheets[key]);
            foreach (DataRowView drv in dv)
            {
               ds.AddRow(drv.Row.ItemArray.Select(c => c.ToString()));
            }
            filteredDataSheets.AddSheet(ds);
         }

         return filteredDataSheets;
      }

      public void CopySheetsFrom (DataSheetCollection dataSheetsToCopy)
      {
         Clear();
         _dataSheets.AddRange(dataSheetsToCopy._dataSheets);
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

      /// <summary>
      ///    Adds the sheets of collection <paramref name="sheetsToBeAdded" /> to the current collection
      ///    Returns the names of the sheets added.
      /// </summary>
      public IReadOnlyList<string> AddNotExistingSheets(DataSheetCollection sheetsToBeAdded)
      {
         var notExistingSheets = sheetsToBeAdded.Where(x => !_dataSheets.Contains(x)).ToList();
         _dataSheets.AddRange(notExistingSheets);
         return notExistingSheets.Select(x => x.SheetName).ToList();
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