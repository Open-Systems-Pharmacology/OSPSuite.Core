using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Core.DataFormat;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IDataFormat
   {
      string Name { get; }
      string Description { get; }
      bool CheckFile(IUnformattedData rawData);
      IList<IDataFormatParameter> Parameters { get; }
      IList<Dictionary<Column, IList<double>>> Parse(IUnformattedData data);
   }
}