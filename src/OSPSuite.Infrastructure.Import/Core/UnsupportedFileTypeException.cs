using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnsupportedFileTypeException : Exception
   {
      public UnsupportedFileTypeException() : base(Error.UnsupportedFileType)
      {
      }
   }
}