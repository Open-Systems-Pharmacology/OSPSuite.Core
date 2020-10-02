using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IParsedDataSet
   {
      IEnumerable<InstantiatedMetaData> Description { get; set; }

      Dictionary<Column, IList<ValueAndLloq>> Data { get; set; }
   }

   public class ParsedDataSet : IParsedDataSet
   {
      public IEnumerable<InstantiatedMetaData> Description { get; set; }

      public Dictionary<Column, IList<ValueAndLloq>> Data { get; set; }
   }
}
