using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public class DimensionMismatchException : OSPSuiteException
   {
      public DimensionMismatchException(IEnumerable<IDimension> dimensionNames ) : base(Error.DimensionMismatchError(dimensionNames.Select(x=>x.Name)))
      {
      }
   }
}