using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnsupportedFileTypeException : AbstractImporterException
   {
      public UnsupportedFileTypeException() : base(Error.UnsupportedFileType)
      {
      }
   }
}