using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Core.DataFormat;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IDataFormat
   {
      string Name { get; }
      string Description { get; }
      bool SetParameters(IUnformattedData rawData);
      IList<DataFormatParameter> Parameters { get; }
      IList<Dictionary<Column, IList<double>>> Parse(IUnformattedData data);

      bool CheckConditions(Dictionary<string, string> conditions);
   }
}