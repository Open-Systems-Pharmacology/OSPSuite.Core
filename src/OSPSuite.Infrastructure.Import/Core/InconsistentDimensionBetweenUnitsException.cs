using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InconsistentDimensionBetweenUnitsException : AbstractImporterException
   {
      public InconsistentDimensionBetweenUnitsException(string mappingName) : base(Error.InconsistentDimensionBetweenUnitsException(mappingName))
      {
      }
   }
}
