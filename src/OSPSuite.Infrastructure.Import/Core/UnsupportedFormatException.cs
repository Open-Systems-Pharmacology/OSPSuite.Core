using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnsupportedFormatException : AbstractImporterException
   {
      public UnsupportedFormatException(string fileName) : base(Error.UnsupportedFileFormat(fileName))
      {
      }
   }
}