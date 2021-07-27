using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InconsistentDimensionBetweenUnitsException : AbstractImporterExceptions
   {
      public InconsistentDimensionBetweenUnitsException(string mappingName) : base(Error.InconsistentDimensionBetweenUnitsException(mappingName))
      {
      }
   }
}
