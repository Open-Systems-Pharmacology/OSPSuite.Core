using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InconsistentDimensionBetweenUnitsException : AbstractImporterException
   {
      public InconsistentDimensionBetweenUnitsException(string mappingName) : base(Error.InconsistentDimensionBetweenUnitsException(mappingName))
      {
      }
   }
}
