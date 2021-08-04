using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class MissingColumnException : AbstractImporterException
   {
      public MissingColumnException(string missingColumn) : base(Error.MissingColumnException(missingColumn))
      {
      }
   }
}
