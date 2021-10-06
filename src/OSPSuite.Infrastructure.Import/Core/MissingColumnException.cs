using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class MissingColumnException : AbstractImporterException
   {
      public MissingColumnException(string missingColumn) : base(Error.MissingColumnException(missingColumn))
      {
      }
   }
}
