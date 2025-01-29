using OSPSuite.Core.Import;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ExtendedColumn
   {
      public Column Column { get; set; } //TODO: flatten the structure in order to avoid too many "column" namings
      public ColumnInfo ColumnInfo { get; set; }
      public string ErrorDeviation { get; set; }
   }
}