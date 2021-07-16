using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ErrorUnitException : OSPSuiteException
   {
      public ErrorUnitException() : base(Error.InvalidErrorDimension)
      {
      }
   }
}
