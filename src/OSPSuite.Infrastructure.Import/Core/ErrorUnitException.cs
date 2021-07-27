using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ErrorUnitException : AbstractImporterException
   { 
      public ErrorUnitException() : base(Error.InvalidErrorDimension)
      {
      }
   }
}
