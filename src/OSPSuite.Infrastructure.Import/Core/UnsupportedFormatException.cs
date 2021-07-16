using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnsupportedFormatException : OSPSuiteException
   {
      public UnsupportedFormatException(string fileName) : base(Error.UnsupportedFileFormat(fileName))
      {
      }
   }
}