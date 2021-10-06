using OSPSuite.Assets;
using System.Collections.Generic;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class EmptyDataSetsException : AbstractImporterException
   {
      public EmptyDataSetsException(IEnumerable<string> dataSetNames) : base(Error.EmptyDataSet($"{string.Join(", ", dataSetNames)}"))
      {
      }
   }
}
