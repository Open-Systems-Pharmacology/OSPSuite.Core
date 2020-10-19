using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public class DataFormatHeadersWithUnits : AbstractColumnsDataFormat
   {
      private const string _tMetaDataName = "Headers with units";
      private const string _tMetaDataDescription = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639";
      public override string Name { get; } = _tMetaDataName;

      public override string Description { get; } = _tMetaDataDescription;

      protected override string ExtractLloq(string description, IUnformattedData data, List<string> keys, ref double rank)
      {
         return null;
      }

      protected override UnitDescription ExtractUnits(string description, IUnformattedData data, List<string> keys, ref double rank)
      {
         var units = Regex.Match(description, @"\[.+\]").Value;
         if (String.IsNullOrEmpty(units))
            return new UnitDescription("?");
         var unit = units
            .Substring(1, units.Length - 2) //remove the brackets
            .Trim() //remove whitespace
            .Split(',') //split comma separated list
            .FirstOrDefault()??"?"; //default = ?
         if (unit != "?")
         {
            rank++;
         }
         return new UnitDescription(unit);
      }

      protected override Dictionary<Column, IList<ValueAndLloq>> ParseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
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
            var currentParameter = mappingParameters.FirstOrDefault(p => p.MappedColumn.Name == columnInfo.DisplayName);
            if (currentParameter == null) continue;
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