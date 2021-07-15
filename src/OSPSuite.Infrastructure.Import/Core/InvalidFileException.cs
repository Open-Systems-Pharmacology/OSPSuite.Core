using OSPSuite.Assets;
using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidFileException : Exception
   {
      public InvalidFileException(string exceptionMessage = "") : base(Error.InvalidFileException(exceptionMessage))
      {
      }
   }
}
