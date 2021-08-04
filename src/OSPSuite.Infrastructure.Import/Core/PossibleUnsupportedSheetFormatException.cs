using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class PossibleUnsupportedSheetFormatException : AbstractImporterException
   {
      public PossibleUnsupportedSheetFormatException( string sheetName) : base(Error.PossibleUnsupportedSheetFormatException(sheetName))
      {
      }
   }
}
