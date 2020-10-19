using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ParsedDataSet
   {
      protected IEnumerable<InstantiatedMetaData> Description { get; set; }

      public IReadOnlyDictionary<Column, IList<ValueAndLloq>> Data { get; protected set; }

      public ParsedDataSet(
         IEnumerable<(string ColumnName, IList<string> ExistingValues)> mappings,
         IUnformattedData columnHandler,
         IEnumerable<IEnumerable<string>> rawData,
         Dictionary<Column, IList<ValueAndLloq>> parsedData
      )
      {
         Description = mappings.Select(p =>
            new InstantiatedMetaData()
            {
               Id = columnHandler.GetColumnDescription(p.ColumnName).Index,
               Value = rawData.First().ElementAt(columnHandler.GetColumnDescription(p.ColumnName).Index)
            }
         );
         Data = parsedData;
      }

      public string NameFromConvention(IEnumerable<MetaDataMappingConverter> mappings, string convention, string fileName, string sheetName)
      {
         return mappings.Aggregate
         (
            // Start with the namingConvention replacing {file} and {sheet} by their names
            new MetaDataMappingConverter()
            {
               Id = convention.Replace($"{{{Constants.FILE}}}", fileName).Replace($"{{{Constants.SHEET}}}", sheetName)
            },
            // Aggregates then by iterating on mappings (which contains all valid keys, e.g. metadata) and replacing any text
            // {id} with id being the id of the current key by the value stored for such a key when the data was parsed
            (acc, x) =>
               new MetaDataMappingConverter()
               {
                  Id = acc.Id.Replace($"{{{x.Id}}}", $"{ValueForColumn(x.Index(sheetName))}")
               }
         ).Id;
      }

      public string ValueForColumn(int columnId)
      {
         return Description.FirstOrDefault(md => md.Id == columnId)?.Value;
      }
   }
}
