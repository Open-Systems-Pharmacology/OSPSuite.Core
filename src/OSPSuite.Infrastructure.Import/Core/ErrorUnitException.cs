using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ErrorUnitException : Exception
   {
      private const string MESSAGE = "The dimension of the error units must be the same as the dimension of the measurement units.";

      public ErrorUnitException() : base(MESSAGE)
      {
      }
   }
}
