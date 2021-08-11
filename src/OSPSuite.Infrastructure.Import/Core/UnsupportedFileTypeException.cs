using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnsupportedFileTypeException : AbstractImporterException
   {
      public UnsupportedFileTypeException() : base(Error.UnsupportedFileType)
      {
      }
   }
}