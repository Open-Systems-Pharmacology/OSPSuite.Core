using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using System.Collections.Generic;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class EmptyDataSetsException : OSPSuiteException
   {
      public EmptyDataSetsException(IEnumerable<string> dataSetNames) : base(Error.EmptyDataSet($"{string.Join(", ", dataSetNames)}"))
      {
      }
   }
}
