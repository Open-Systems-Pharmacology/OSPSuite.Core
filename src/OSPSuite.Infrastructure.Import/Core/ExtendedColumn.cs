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
      ///    Distinct non-empty values read for this data set from the error type column (empty when the error type does not come
      ///    from a column)
      /// </summary>
      public IReadOnlyList<string> ErrorStdDevValues { get; set; } = new List<string>();
   }
}