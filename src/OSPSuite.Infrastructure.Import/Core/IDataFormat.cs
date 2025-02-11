using System.Collections.Generic;
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
      double SetParameters(DataSheet rawDataSheet, ColumnInfoCache columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories);
      IList<DataFormatParameter> Parameters { get; }
      void CopyParametersFromConfiguration(OSPSuite.Core.Import.ImporterConfiguration configuration);
      IList<string> ExcelColumnNames { get; }
      IEnumerable<ParsedDataSet> Parse(DataSheet dataSheet, ColumnInfoCache columnInfos);
      UnitDescription ExtractUnitDescriptions(string description, ColumnInfo columnInfo);
      T GetColumnByName<T>(string columnName) where T : DataFormatParameter;
      IEnumerable<T> GetParameters<T>() where T : DataFormatParameter;
   }
}