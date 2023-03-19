using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ImporterEmptyFileException : AbstractImporterException
   {
      public ImporterEmptyFileException() : base(Error.ImporterEmptyFile)
      {
      }
   }
}