
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class ColumnsDataFormat : IDataFormat
   {
      public string Name => "ColumnsFormat";

      public string Description => "Simple format with data identified by column data...";

      public bool CheckFile(Dictionary<string, IList<string>> rawData)
      {
         throw new System.NotImplementedException();
      }

      public Dictionary<IColumn, IList<double>> Parse(Dictionary<string, IList<string>> rawData)
      {
         throw new System.NotImplementedException();
      }
   }
}
