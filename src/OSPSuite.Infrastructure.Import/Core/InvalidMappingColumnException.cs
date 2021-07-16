using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidMappingColumnException : OSPSuiteException
   {
      InvalidMappingColumnException() : base(Error.InvalidMappingColumn)
      {

      }
   }
}
