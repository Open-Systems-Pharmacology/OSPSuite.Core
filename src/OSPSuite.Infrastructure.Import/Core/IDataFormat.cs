using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Utility.Collections;

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
      double SetParameters(IUnformattedData rawData, Cache<string, ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories);
      IList<DataFormatParameter> Parameters { get; }
      void CopyParametersFromConfiguration(OSPSuite.Core.Import.ImporterConfiguration configuration);
      IList<string> ExcelColumnNames { get; }
      IEnumerable<ParsedDataSet> Parse(IUnformattedData data, Cache<string, ColumnInfo> columnInfos);
      UnitDescription ExtractUnitDescriptions(string description, IReadOnlyList<IDimension> supportedDimensions);
   }
}