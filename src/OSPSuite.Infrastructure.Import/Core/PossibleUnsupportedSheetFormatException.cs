using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class PossibleUnsupportedSheetFormatException : Exception
   {
      public PossibleUnsupportedSheetFormatException( string sheetName) : base(Error.PossibleUnsupportedSheetFormatException(sheetName))
      {
      }
   }
}
