using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidDimensionException : AbstractImporterExceptions
   {
      public InvalidDimensionException(string invalidUnit, string mappingName) : base(Error.InvalidDimensionException(invalidUnit, mappingName))
      {
      }
   }
}
