using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class SimulationPoint
   {
      public double Measurement { get; set; }
      public double Lloq { get; set; }
      public string Unit { get; set; }
   }
   public interface IDataFormat
   {
      string Name { get; }
      string Description { get; }
      double SetParameters(IUnformattedData rawData, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories);
      IList<DataFormatParameter> Parameters { get; }
      void CopyParametersFromConfiguration(OSPSuite.Core.Import.ImporterConfiguration configuration);
      IList<string> ExcelColumnNames { get; }
      IEnumerable<ParsedDataSet> Parse(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);
      UnitDescription ExtractUnits(string description, IReadOnlyList<IDimension> supportedDimensions);
   }
}