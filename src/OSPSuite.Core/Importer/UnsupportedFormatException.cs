using System;

namespace OSPSuite.Core.Importer
{
   public class UnsupportedFormatException : Exception
   {
      private const string MESSAGE = "The file format is not supported";

      public UnsupportedFormatException() : base(MESSAGE)
      {
      }
   }
}