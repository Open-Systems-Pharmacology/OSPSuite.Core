using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class MismatchingArrayLengthsException : OSPSuiteException
   {
      public MismatchingArrayLengthsException() : base(Error.MismatchingArrayLengths)
      {
      }
   }
}
