using System.Collections;
using System.Data;

namespace OSPSuite.Core.Importer
{
   public class ImportDataColumnCollection : IEnumerable
   {
      private readonly DataColumnCollection _columns;

      internal ImportDataColumnCollection(DataColumnCollection columns)
      {
         _columns = columns;
      }

      public void Clear()
      {
         _columns.Clear();
      }

      public void Add(ImportDataColumn column)
      {
         _columns.Add(column);
      }

      public int Count
      {
         get { return _columns.Count; }
      }

      public void Remove(ImportDataColumn column)
      {
         _columns.Remove(column);
      }
      public void RemoveAt(int index)
      {
         _columns.RemoveAt(index);
      }

      public bool ContainsName(string columnName)
      {
         return _columns.Contains(columnName);
      }

      public bool ContainsColumn(ImportDataColumn column)
      {
         return _columns.Contains(column.ColumnName);
      }
      public ImportDataColumn ItemByName(string columnName)
      {
         if (!_columns.Contains(columnName)) return null;
         return (ImportDataColumn) _columns[_columns.IndexOf(columnName)];
      }

      public ImportDataColumn ItemByIndex(int index)
      {
         return (ImportDataColumn) _columns[index];
      }

      public IEnumerator GetEnumerator()
      {
         return _columns.GetEnumerator();
      }
   }
}