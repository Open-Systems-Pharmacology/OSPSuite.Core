using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class DataFormat_Nonmem : AbstractColumnsDataFormat
   {
      public override string Name => "Nonmem Oriented";

      public override string Description => "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";

      private int _lloq_index = -1;

      protected override void extractGeneralParameters(List<string> keys, IUnformattedData data)
      {
         base.extractGeneralParameters(keys, data);
         var lloq = data.GetHeaders().FirstOrDefault(h => h.ToUpper() == "LLOQ");
         if (lloq != null)
            _lloq_index = data.GetColumnDescription(lloq).Index;
      }

      protected override Func<int, string> extractUnits(string description, IUnformattedData data, List<string> keys)
      {
         var unitKey = data.GetHeaders().FirstOrDefault(h => h.ToUpper() == (description.ToUpper() + "_UNIT"));
         if (unitKey == null)
         {
            return _ => "?";
         }
         keys.Remove(unitKey);
         var units = data.GetColumn(unitKey).ToList();
         var def = data.GetColumnDescription(unitKey).ExistingValues.FirstOrDefault();
         return i => (i > 0) ? units[i] : def;
      }

      protected override Dictionary<Column, IList<ValueAndLloq>> parseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data, IReadOnlyList<OSPSuite.Core.Importer.ColumnInfo> columnInfos)
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
                        double lloq;
                        if (_lloq_index < 0 || !double.TryParse(row.ElementAt(_lloq_index).Trim(), out lloq))
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
