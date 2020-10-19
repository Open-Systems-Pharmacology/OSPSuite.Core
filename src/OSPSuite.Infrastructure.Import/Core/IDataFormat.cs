using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core.DataFormat;

namespace OSPSuite.Infrastructure.Import.Core
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