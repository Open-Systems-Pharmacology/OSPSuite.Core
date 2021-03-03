using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ErrorUnitException : Exception
   {
      public ErrorUnitException() : base(Error.InavlidErrorDimension)
      {
      }
   }
}
