using System.Collections;
using System.Data;

namespace OSPSuite.Core.Importer
{
   public class MetaDataColumnCollection : IEnumerable
   {
      private readonly DataColumnCollection _columns;

      internal MetaDataColumnCollection(DataColumnCollection columns)
      {
         _columns = columns;
      }

      public void Clear()
      {
         _columns.Clear();
      }

      public void Add(MetaDataColumn column)
      {
         _columns.Add(column);
      }

      public int Count
      {
         get { return _columns.Count; }
      }

      public void Remove(MetaDataColumn column)
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

      public bool ContainsColumn(MetaDataColumn column)
      {
         return _columns.Contains(column.ColumnName);
      }

      public MetaDataColumn ItemByName(string columnName)
      {
         return (MetaDataColumn) _columns[_columns.IndexOf(columnName)];
      }

      public MetaDataColumn ItemByIndex(int index)
      {
         return (MetaDataColumn) _columns[index];
      }

      public IEnumerator GetEnumerator()
      {
         return _columns.GetEnumerator();
      }
   }
}