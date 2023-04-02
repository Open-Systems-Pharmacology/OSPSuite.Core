using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidObservedDataFileException : AbstractImporterException
   {
      public InvalidObservedDataFileException(string exceptionMessage = "") : base(Error.InvalidObservedDataFile(exceptionMessage))
      {
      }
   }

   public class DataFileWithDuplicateHeaderException : AbstractImporterException
   {
      public DataFileWithDuplicateHeaderException(string exceptionMessage = "") : base(exceptionMessage)
      {
      }
   }
}