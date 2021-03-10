using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ParsedDataSet
   {
      protected IEnumerable<InstantiatedMetaData> Description { get; set; }

      //new class with column AND everything I need from the colInfo
      public IReadOnlyDictionary<ExtendedColumn, IList<SimulationPoint>> Data { get; protected set; }

      public ParsedDataSet(
         IEnumerable<(string ColumnName, IList<string> ExistingValues)> mappings,
         IUnformattedData columnHandler,
         IEnumerable<UnformattedRow> rawData,
         Dictionary<ExtendedColumn, IList<SimulationPoint>> parsedData
      )
      {
         Description = mappings.Select(p =>
         {
            var columnDescription = columnHandler.GetColumnDescription(p.ColumnName);
            return new InstantiatedMetaData()
            {
               Id = columnDescription != null ? columnDescription.Index : -1,
               Value = columnDescription != null ? rawData.First().Data.ElementAt(columnHandler.GetColumnDescription(p.ColumnName).Index) : p.ColumnName
            };
         }
         );
         Data = parsedData;
      }

      public string NameFromConvention(IEnumerable<MetaDataMappingConverter> mappings, string convention, string fileName, string sheetName)
      {
         var result = convention.Replace($"{{{Constants.FILE}}}", fileName).Replace($"{{{Constants.SHEET}}}", sheetName);
         for (var i = 0; i < mappings.Count(); i++)
         {
            result = result.Replace($"{{{mappings.ElementAt(i).Id}}}", $"{Description.ElementAt(i).Value}");
         }
         return result;
      }

      public IReadOnlyDictionary<string, string> EnumerateMetaData(IEnumerable<MetaDataMappingConverter> mappings, string sheetName)
      {
         var result = new Dictionary<string, string>();
         for (var i = 0; i < mappings.Count(); i++)
         {
            result.Add(mappings.ElementAt(i).Id, Description.ElementAt(i).Value);
         }
         return result;
      }

      public string ValueForColumn(int columnId)
      {
         return Description.FirstOrDefault(md => md.Id == columnId)?.Value;
      }
   }
}
