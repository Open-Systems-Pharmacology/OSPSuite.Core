
using OSPSuite.Presentation.Importer.Core.DataFormat;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{

   public interface IDataFormat
   {
      string Name { get; }
      string Description { get; }
      bool CheckFile(Dictionary<string, IList<string>> rawData);
      IList<DataFormatParameter> Parameters { get; }
      IList<Dictionary<IColumn, IList<double>>> Parse(Dictionary<string, IList<string>> rawData);
   }
}
