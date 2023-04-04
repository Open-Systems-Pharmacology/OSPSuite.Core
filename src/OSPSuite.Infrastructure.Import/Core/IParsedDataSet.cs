using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ParsedDataSet
   {
      public IEnumerable<InstantiatedMetaData> Description { get; protected set; }

      //new class with column AND everything I need from the colInfo
      public IReadOnlyDictionary<ExtendedColumn, IList<SimulationPoint>> Data { get; protected set; }

      public ParsedDataSet(
         IEnumerable<string> groupingParameters,
         DataSheet columnHandler,
         IEnumerable<UnformattedRow> rawData,
         Dictionary<ExtendedColumn, IList<SimulationPoint>> parsedData
      )
      {
         Description = groupingParameters.Select(x =>
            {
               //All rows should share the same value for the groupingParameters
               var columnDescription = columnHandler.GetColumnDescription(x);
               var columnValue = columnDescription != null ? rawData.First().Data.ElementAt(columnDescription.Index) : x;
               return new InstantiatedMetaData()
               {
                  Id = columnDescription?.Index ?? -1, //-1 stands for no real position
                  Value = columnValue
               };
            }
         );
         Data = parsedData;
      }

      public string NameFromConvention(IReadOnlyList<MetaDataMappingConverter> mappings, string convention, string fileName, string sheetName)
      {
         var result = convention.Replace($"{{{Constants.FILE}}}", fileName).Replace($"{{{Constants.SHEET}}}", sheetName);
         for (var i = 0; i < mappings.Count(); i++)
         {
            result = result.Replace($"{{{mappings.ElementAt(i).Id}}}", $"{Description.ElementAt(i).Value}");
         }

         return result;
      }

      public IReadOnlyList<MetaDataInstance> EnumerateMetaData(IEnumerable<MetaDataMappingConverter> mappings)
      {
         return mappings.Select((m, i) => new MetaDataInstance(m.Id, Description.ElementAt(i).Value)).ToList();
      }

      public string ValueForColumn(int columnId)
      {
         return Description.FirstOrDefault(md => md.Id == columnId)?.Value;
      }
   }
}