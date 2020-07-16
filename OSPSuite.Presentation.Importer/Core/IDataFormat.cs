
using OSPSuite.Presentation.Importer.Core.DataFormat;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{

   public interface IDataFormat
   {
      string Name { get; }
      string Description { get; }
      bool CheckFile(IUnformattedData rawData);
      IList<DataFormatParameter> Parameters { get; }
      IList<Dictionary<Column, IList<double>>> Parse(IUnformattedData data);
   }
}
