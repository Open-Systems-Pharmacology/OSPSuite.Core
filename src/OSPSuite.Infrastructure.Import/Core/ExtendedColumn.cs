using System.Collections.Generic;
using OSPSuite.Core.Import;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ExtendedColumn
   {
      public Column Column { get; set; } //TODO: flatten the structure in order to avoid too many "column" namings
      public ColumnInfo ColumnInfo { get; set; }
      public string ErrorDeviation { get; set; }

      /// <summary>
      ///    Value of the error type column for each row of this data set, in the order of the rows (empty when the error type does
      ///    not come from a column)
      /// </summary>
      public IList<string> ErrorStdDevValues { get; set; } = new List<string>();
   }
}