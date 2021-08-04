using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public abstract class AbstractImporterException : OSPSuiteException
   {
      protected AbstractImporterException(string message) : base(message)
      {
      }
   }
}