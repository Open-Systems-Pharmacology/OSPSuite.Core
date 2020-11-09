using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ExtendedColumn
   { 
      public Column Column { get; set; } //WE END UP HAVING COLUMN WAY TOO MANY TIMES LIKE THIS
      public ColumnInfo ColumnInfo { get; set; }
      public string ErrorDeviation { get; set; }

      /*
      public string BaseGridName { get; set; } //should we actually be getting the whole ColumnInfo?
      public string Name { get; set; }
      public string ColumnType { get; set; }
      public string ErrorDeviation { get; set; }
*/
   }
}
