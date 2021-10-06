using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnformattedDataRow : IEnumerable<string>
   {
      private readonly IEnumerable<string> _row;
      private readonly Cache<string, ColumnDescription> _headers;
      public UnformattedDataRow(IEnumerable<string> row, Cache<string, ColumnDescription> headers)
      {
         _row = row;
         _headers = headers;
      }

      IEnumerator<string> IEnumerable<string>.GetEnumerator()
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

      public IEnumerator GetEnumerator()
      {
         return _row.GetEnumerator();
      }
   }
}