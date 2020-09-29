using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class DataFormat_TMetaData_C : AbstractColumnsDataFormat
   {
      public override string Name { get; } = "015_TMetaData_C(E)";

      public override string Description { get; } = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639";

      protected override Func<int, string> ExtractUnits(string description, IUnformattedData data, List<string> keys)
      {
         var units = Regex.Match(description, @"\[.+\]").Value;
         if (String.IsNullOrEmpty(units))
            return _ => "?";
         var unit = units.Substring(1, units.Length - 2).Trim().Split(',').FirstOrDefault()??"?";
         return _ => unit;
      }

      protected override Dictionary<Column, IList<ValueAndLloq>> parseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var dictionary = new Dictionary<Column, IList<ValueAndLloq>>();
         //Add time mapping
         var mappingParameters =
            Parameters
               .OfType<MappingDataFormatParameter>()
               .ToList();

         var dataSet = rawDataSet.ToList();
         foreach (var columnInfo in columnInfos)
         {
            var currentParameter = mappingParameters.First(p => p.MappedColumn.Name.ToString() == columnInfo.DisplayName);
            if (currentParameter != null)
               dictionary.Add
               (
                  currentParameter.MappedColumn,
                  dataSet.Select
                  (
                     row =>
                     {
                        var element = row.ElementAt(data.GetColumnDescription(currentParameter.ColumnName).Index).Trim();
                        if (double.TryParse(element, out double result))
                           return new ValueAndLloq()
                           {
                              Value = result
                           };
                        if (element.StartsWith("<"))
                        {
                           double.TryParse(element.Substring(1), out result);
                           return new ValueAndLloq()
                           {
                              Lloq = result
                           };
                        }
                        return new ValueAndLloq()
                        {
                           Value = double.NaN
                        };
                     }
                  ).ToList()
               );
         }


         return dictionary;
      }
   }
}