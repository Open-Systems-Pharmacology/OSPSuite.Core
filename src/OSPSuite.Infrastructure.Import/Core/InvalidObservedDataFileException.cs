using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidObservedDataFileException : AbstractImporterException
   {
      public InvalidObservedDataFileException(string exceptionMessage = "") : base(Error.InvalidObservedDataFile(exceptionMessage))
      {
      }
   }
}
