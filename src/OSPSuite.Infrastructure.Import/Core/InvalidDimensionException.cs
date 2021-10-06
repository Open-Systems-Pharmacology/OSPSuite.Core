using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidDimensionException : AbstractImporterException
   {
      public InvalidDimensionException(string invalidUnit, string mappingName) : base(Error.InvalidDimensionException(invalidUnit, mappingName))
      {
      }
   }
}
