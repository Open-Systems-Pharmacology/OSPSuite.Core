using System.Data;

namespace OSPSuite.Core.Importer
{
   public class ImportDataRowCollection : System.Collections.IEnumerable
   {

      private readonly DataRowCollection _rows;

      internal ImportDataRowCollection(DataRowCollection rows)
      {
         _rows = rows;
      }

      public void Clear()
      {
         _rows.Clear();
      }

      public void Add(ImportDataRow row)
      {
         _rows.Add(row);
      }

      public int Count
      {
         get { return _rows.Count; }
      }

      public void Remove(ImportDataRow row)
      {
         _rows.Remove(row);
      }

      public void RemoveAt(int index)
      {
         _rows.RemoveAt(index);
      }

      public bool ContainsRow(ImportDataRow row)
      {
         return _rows.Contains(row);
      }

      public ImportDataRow ItemByIndex(int index)
      {
         return (ImportDataRow)_rows[index];
      }

      public System.Collections.IEnumerator GetEnumerator()
      {
         return _rows.GetEnumerator();
      }


   }
}