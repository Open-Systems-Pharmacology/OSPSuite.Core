using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnsupportedFileTypeException : OSPSuiteException
   {
      public UnsupportedFileTypeException() : base(Error.UnsupportedFileType)
      {
      }
   }
}