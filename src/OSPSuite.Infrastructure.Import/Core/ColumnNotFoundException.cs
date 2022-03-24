using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ColumnNotFoundException : AbstractImporterException
   {
      public ColumnNotFoundException(string columnName) : base(Error.ColumnNotFound(columnName))
      {
      }
   }
}
