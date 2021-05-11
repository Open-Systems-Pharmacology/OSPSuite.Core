using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnsupportedFormatException : Exception
   {
      public UnsupportedFormatException(string fileName) : base(Error.UnsupportedFileFormat(fileName))
      {
      }
   }
}