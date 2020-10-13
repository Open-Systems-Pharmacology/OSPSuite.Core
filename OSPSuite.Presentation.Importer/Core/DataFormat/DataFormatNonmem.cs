using OSPSuite.Presentation.Importer.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class DataFormatNonmem : AbstractColumnsDataFormat
   {
      private const string _nameNonMem = "Nonmem";
      private const string _descriptionNonMem = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";
      private const int _lloqColumnIndex = -1;
      public override string Name => _nameNonMem;
      public override string Description => _descriptionNonMem;
      private int _lloqIndex = _lloqColumnIndex;

      protected override string ExtractLloq(string description, IUnformattedData data, List<string> keys, ref double rank)
      {
         var lloqKey = data.GetHeaders().FindHeader(description + "_LLOQ");
         if (lloqKey == null)
         {
            return "";
         }
         keys.Remove(lloqKey);
         rank++;
         return lloqKey;
      }

      protected override UnitDescription ExtractUnits(string description, IUnformattedData data, List<string> keys, ref double rank)
      {
         var unitKey = data.GetHeaders().FindHeader(description + "_UNIT");
         if (unitKey == null)
         {
            return new UnitDescription(_ => "?");
         }
         keys.Remove(unitKey);
         var units = data.GetColumn(unitKey).ToList();
         var def = data.GetColumnDescription(unitKey).ExistingValues.FirstOrDefault();
         rank++; 
         return new UnitDescription(i => (i > 0) ? units[i] : def, unitKey);
      }

      protected override Dictionary<Column, IList<ValueAndLloq>> ParseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data, IReadOnlyList<OSPSuite.Core.Importer.ColumnInfo> columnInfos)
      {
         var dictionary = new Dictionary<Column, IList<ValueAndLloq>>();

         //Add time mapping
         var mappingParameters = Parameters.OfType<MappingDataFormatParameter>().ToList();

         var dataSet = rawDataSet.ToList();
         foreach (var columnInfo in columnInfos)
         {
            var currentParameter = mappingParameters.First(p => p.MappedColumn.Name == columnInfo.DisplayName);
            if (currentParameter == null) continue;
            dictionary.Add
            (
               currentParameter.MappedColumn,
               dataSet.Select
               (
                  row =>
                  {
                     double lloq;
                     if (_lloqIndex < 0 || !double.TryParse(row.ElementAt(_lloqIndex).Trim(), out lloq))
                        lloq = double.NaN;
                     var element = row.ElementAt(data.GetColumnDescription(currentParameter.ColumnName).Index).Trim();
                     if (double.TryParse(element, out double result))
                        return new ValueAndLloq()
                        {
                           Value = result,
                           Lloq = lloq
                        };
                     return new ValueAndLloq()
                     {
                        Value = double.NaN,
                        Lloq = lloq
                     };
                  }
               ).ToList()
            );
         }

         return dictionary;
      }
   }
}
