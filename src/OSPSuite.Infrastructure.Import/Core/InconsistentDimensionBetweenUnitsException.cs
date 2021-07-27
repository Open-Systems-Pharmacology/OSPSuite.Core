using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InconsistentDimensionBetweenUnitsException : OSPSuiteException
   {
      public InconsistentDimensionBetweenUnitsException(string mappingName) : base(Error.InconsistentDimensionBetweenUnitsException(mappingName))
      {
      }
   }
}
