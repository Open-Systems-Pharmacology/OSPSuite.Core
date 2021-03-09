using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class NanException : Exception
   {
      public NanException() : base(Error.NaNOnData)
      {
      }
   }
}
