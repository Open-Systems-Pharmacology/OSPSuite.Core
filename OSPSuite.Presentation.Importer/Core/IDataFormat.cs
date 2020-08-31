using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IDataFormat
   {
      string Name { get; }
      string Description { get; }
      bool SetParameters(IUnformattedData rawData);
      IList<DataFormatParameter> Parameters { get; }
      IList<Dictionary<Column, IList<double>>> Parse(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);
   }
}