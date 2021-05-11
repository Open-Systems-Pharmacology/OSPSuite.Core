using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class MismatchingArrayLengthsException : Exception
   {
      public MismatchingArrayLengthsException() : base(Error.MismatchingArrayLengths)
      {
      }
   }
}
