using System;

namespace OSPSuite.Presentation.Importer.Core
{
   public class UnsupportedFormatException : Exception
   {
      private const string MESSAGE = "The file format is not supported";

      public UnsupportedFormatException() : base(MESSAGE)
      {
      }
   }
}