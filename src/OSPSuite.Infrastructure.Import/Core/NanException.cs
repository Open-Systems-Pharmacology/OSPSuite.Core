using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class NanException : Exception
   {
      private const string MESSAGE = "Data contains NaN values at imported columns. Select a different action for NaN values or clean your data.";

      public NanException() : base(MESSAGE)
      {
      }
   }
}
