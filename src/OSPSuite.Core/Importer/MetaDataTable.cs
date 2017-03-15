using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Assets;

namespace OSPSuite.Core.Importer
{
   public class MetaDataTable : DataTable, System.Collections.IEnumerable
   {
      private readonly MetaDataColumnCollection _columns;
      private readonly MetaDataRowCollection _rows;

      public MetaDataTable()
      {
         _rows = new MetaDataRowCollection(base.Rows);
         _columns = new MetaDataColumnCollection(base.Columns);
      }

      public new MetaDataColumnCollection Columns
      {
         get { return (_columns); }
      }

      public new MetaDataRowCollection Rows
      {
         get { return _rows; }
      }

      public new MetaDataTable Copy()
      {
         var retValue = Clone();
         retValue.BeginLoadData();
         foreach (MetaDataRow row in Rows)
            retValue.ImportRow(row);
         retValue.EndLoadData();
         return retValue;
      }

      public new MetaDataTable Clone()
      {
         var retValue = base.Clone() as MetaDataTable;
         if (retValue == null) return null;

         foreach (MetaDataColumn col in Columns)
         {
            var cloneColumn = retValue.Columns.ItemByName(col.ColumnName);
            if (cloneColumn == null) continue;
            cloneColumn.Description = col.Description;
            if (col.ListOfValues != null)
               cloneColumn.ListOfValues = new Dictionary<string, string>(col.ListOfValues);
            if (col.ListOfImages != null)
               cloneColumn.ListOfImages = new Dictionary<string, ApplicationIcon>(col.ListOfImages);
            cloneColumn.IsListOfValuesFixed = col.IsListOfValuesFixed;
            cloneColumn.MinValue = col.MinValue;
            cloneColumn.MinValueAllowed = col.MinValueAllowed;
            cloneColumn.MaxValue = col.MaxValue;
            cloneColumn.MaxValueAllowed = col.MaxValueAllowed;
         }
         return retValue;
      }

      public void RemapListOfValueSelection()
      {
         if (Rows.Count == 0) return;
         foreach (MetaDataColumn col in Columns)
         {
            if (col.ListOfValues == null) continue;
            if (col.IsListOfValuesFixed) continue;
            var value = Rows.ItemByIndex(0)[col];
            if (value == DBNull.Value) continue;
            if (!col.ListOfValues.ContainsValue(value.ToString())) continue;
            Rows.ItemByIndex(0)[col] = findKeyByValueInDictionary(col.ListOfValues, value.ToString());
         }
      }

      static private string findKeyByValueInDictionary(Dictionary<string, string> dictionary, string value)
      {
         return dictionary.Keys.FirstOrDefault(key => dictionary[key] == value);
      }

      protected override DataTable CreateInstance()
      {
         return new MetaDataTable();
      }

      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
         return new MetaDataRow(builder);
      }

      public System.Collections.IEnumerator GetEnumerator()
      {
         return Rows.GetEnumerator();
      }

      private bool _disposed;

      protected override void Dispose(bool disposing)
      {
         if (!_disposed)
         {
            try
            {
               if (disposing)
               {
                  foreach (MetaDataColumn col in _columns)
                     col.Dispose();
               }
               _disposed = true;
            }
            finally
            {
               base.Dispose(disposing);
            }
         }
      }

   }
}
