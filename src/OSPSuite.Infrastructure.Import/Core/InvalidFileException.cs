using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidFileException : Exception
   {
      private const string MESSAGE = "An error occurred while reading the file. Please check the content";

      public InvalidFileException() : base(MESSAGE)
      {
      }
   }
}
