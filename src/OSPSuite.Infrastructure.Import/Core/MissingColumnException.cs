using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class MissingColumnException : Exception
   {
      public MissingColumnException(string missingColumn) : base(Error.MissingColumnException(missingColumn))
      {
      }
   }
}
