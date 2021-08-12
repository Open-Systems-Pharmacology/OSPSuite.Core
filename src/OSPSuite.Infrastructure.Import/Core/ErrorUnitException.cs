using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ErrorUnitException : AbstractImporterException
   { 
      public ErrorUnitException() : base(Error.InvalidErrorDimension)
      {
      }
   }
}
