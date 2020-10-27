using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ExtendedColumn
   { 
      public Column Column { get; set; } //WE END UP HAVING COLUMN WAY TOO MANY TIMES LIKE THIS
      public string BaseGridName { get; set; } //should we actually be getting the whole ColumnInfo?
      public string Name { get; set; }

      public ExtendedColumn(Column column, string baseGridName, string name)
      {
         Column = column;
         BaseGridName = baseGridName;
         Name = name;
      }
   }
}
