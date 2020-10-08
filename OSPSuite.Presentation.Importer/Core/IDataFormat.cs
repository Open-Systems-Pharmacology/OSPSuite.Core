using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Core
{
   public class ValueAndLloq
   {
      public double Value { get; set; }
      public double? Lloq { get; set; }
   }
   public interface IDataFormat
   {
      string Name { get; }
      string Description { get; }
      double SetParameters(IUnformattedData rawData, IReadOnlyList<ColumnInfo> columnInfos);
      IList<DataFormatParameter> Parameters { get; }
      IEnumerable<ParsedDataSet> Parse(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);
   }
}