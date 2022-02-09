using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class BaseGridColumnNotFoundException : AbstractImporterException
   {
      public BaseGridColumnNotFoundException(string columnName) : base(Error.BaseGridColumnNotFoundException(columnName))
      {
      }
   }
}
