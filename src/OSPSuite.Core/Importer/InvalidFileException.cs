using System;

namespace OSPSuite.Core.Importer
{
   public class InvalidFileException : Exception
   {
      private const string MESSAGE = "An error occurred while reading the file. Please check the content";

      public InvalidFileException() : base(MESSAGE)
      {
      }
   }
}
