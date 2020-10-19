using System.Collections.Generic;
using OSPSuite.Core.Importer.DataFormat;
using OSPSuite.Core.Importer;

namespace OSPSuite.Core.Importer
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
      double SetParameters(IUnformattedData rawData, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories);
      IList<DataFormatParameter> Parameters { get; }
      IEnumerable<ParsedDataSet> Parse(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);
   }
}