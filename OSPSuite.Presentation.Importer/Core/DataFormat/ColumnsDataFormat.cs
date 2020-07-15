
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NPOI.SS.Util;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class ColumnsDataFormat : IDataFormat
   {
      public string Name => "ColumnsFormat";

      public string Description => "Simple format with data identified by column data...";

      public IList<DataFormatParameter> Parameters { get; private set; }

      public bool CheckFile(IUnformattedData data)
      {
         if (data.Headers.Where(h => h.Value.Level == ColumnDescription.MeasurmentLevel.NUMERIC).Count() < 2)
            return false;
         SetParameters(data);
         return true;
      }

      private void SetParameters(IUnformattedData data)
      {
         var keys = data.Headers.Keys.ToList();
         Parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         foreach (var header in new[] { "Time", "Measurement", "Error" })
         {
            var headerKey = keys.FirstOrDefault(h => h.ToUpper().Contains(header.ToUpper()));
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               Parameters.Add(new MappingDataFormatParameter(headerKey, new Column() { Name = header, Unit = ExtractUnits(headerKey) }));
            }
            else
            {
               missingKeys.Add(header);
            }
         }

         foreach (var header in missingKeys)
         {
            var headerKey = keys.FirstOrDefault(h => data.Headers[h].Level == ColumnDescription.MeasurmentLevel.NUMERIC);
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               Parameters.Add(new MappingDataFormatParameter(headerKey, new Column() { Name = header, Unit = ExtractUnits(headerKey) }));
            }
         }
      }

      private string ExtractUnits(string description)
      {
         var units = Regex.Match(description, @"\[.+\]").Value;
         return units.Substring(1, units.Length - 2).Trim();
      }

      public IList<Dictionary<IColumn, IList<double>>> Parse(Dictionary<string, IList<string>> rawData)
      {
         throw new System.NotImplementedException();
      }
   }
}
