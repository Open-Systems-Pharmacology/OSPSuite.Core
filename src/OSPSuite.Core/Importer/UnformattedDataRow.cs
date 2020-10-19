using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Importer
{
   public class UnformattedDataRow :IEnumerable
   {
      private readonly IEnumerable<string> _row;
      private readonly Cache<string, ColumnDescription> _headers;
      public UnformattedDataRow(IEnumerable<string> row, Cache<string, ColumnDescription> headers)
      {
         _row = row;
         _headers = headers;
      }

      public IEnumerator GetEnumerator()
      {
         return _row.GetEnumerator();
      }

      public string GetCellValue( string columnName )
      {
         return _row.ElementAt(_headers[columnName].Index);
      }

      public int Count()
      {
         return _row.Count();
      }
   }
}