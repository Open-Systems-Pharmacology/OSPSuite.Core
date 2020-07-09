
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IFormat
   {
      bool CheckFile(Dictionary<string, IList<string>> rawData);
      Dictionary<IColumn, IList<double>> Parse(Dictionary<string, IList<string>> rawData);
   }
}
