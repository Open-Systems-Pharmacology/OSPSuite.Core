
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class ColumnsDataFormat : IDataFormat
   {
      public string Name => "ColumnsFormat";

      public string Description => "Simple format with data identified by column data...";

      public IList<DataFormatParameter> Parameters { get; private set; }

      public bool CheckFile(Dictionary<string, IList<string>> rawData)
      {
         if (rawData.Keys.Count < 2)
            return false;
         SetParameters(rawData);
         return true;
      }

      private void SetParameters(Dictionary<string, IList<string>> rawData)
      {
         var keys = rawData.Keys.ToList();
         var timeKey = rawData.Keys.FirstOrDefault(h => h.ToUpper().Contains("TIME"));
         keys.Remove(timeKey);
         Parameters = new List<DataFormatParameter>();
         var units = Regex.Match(timeKey, @"\[\w+\]").Value;
         units = units.Substring(1, units.Length - 2).Trim();
         Parameters.Add(new MappingDataFormatParameter(timeKey, new Column() { Name = "Time", Unit = units }));
      }

      public IList<Dictionary<IColumn, IList<double>>> Parse(Dictionary<string, IList<string>> rawData)
      {
         throw new System.NotImplementedException();
      }
   }
}
