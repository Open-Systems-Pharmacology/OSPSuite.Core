using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidFileException : Exception
   {
      public InvalidFileException() : base(Error.InvalidFileException)
      {
      }
   }
}
