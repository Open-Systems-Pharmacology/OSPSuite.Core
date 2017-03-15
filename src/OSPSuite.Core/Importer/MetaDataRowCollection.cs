using System.Data;

namespace OSPSuite.Core.Importer
{
   public class MetaDataRowCollection : System.Collections.IEnumerable
   {

      private readonly DataRowCollection _rows;

      internal MetaDataRowCollection(DataRowCollection rows)
      {
         _rows = rows;
      }

      public void Clear()
      {
         _rows.Clear();
      }

      public void Add(MetaDataRow row)
      {
         _rows.Add(row);
      }

      public int Count
      {
         get {return _rows.Count;}
      }

      public void Remove(MetaDataRow row)
      {
         _rows.Remove(row);
      }

      public void RemoveAt(int index)
      {
         _rows.RemoveAt(index);
      }

      public bool ContainsRow(MetaDataRow row)
      {
         return _rows.Contains(row);
      }

      public MetaDataRow ItemByIndex(int index)
      {
         return (MetaDataRow)_rows[index];
      }

      public System.Collections.IEnumerator GetEnumerator()
      {
         return _rows.GetEnumerator();
      }


   }
}