using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class NanException : OSPSuiteException
   {
      public NanException() : base(Error.NaNOnData)
      {
      }
   }
}
