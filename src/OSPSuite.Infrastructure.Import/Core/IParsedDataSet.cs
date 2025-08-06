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
         var result = convention;

         var descriptionList = Description.ToList(); // we are assuming the description list has the same number of elements as mappings
         for (int i = 0; i < mappings.Count && i < descriptionList.Count; i++)
         {
            result = result.Replace($"{{{mappings[i].Id}}}", descriptionList[i].Value);
         }

         result = result
            .Replace($"{{{Constants.FILE}}}", fileName)
            .Replace($"{{{Constants.SHEET}}}", sheetName);

         return result;
      }

      public IReadOnlyList<MetaDataInstance> EnumerateMetaData(IEnumerable<MetaDataMappingConverter> mappings)
      {
         var descriptionList = Description.ToList();
         var mappingList = mappings.ToList();

         return mappingList.Select((m, i) =>
         {
            var value = i < descriptionList.Count && descriptionList[i] != null
               ? descriptionList[i].Value
               : string.Empty;

            return new MetaDataInstance(m.Id, value);
         }).ToList();
      }

      public string ValueForColumn(int columnId)
      {
         return Description.FirstOrDefault(md => md.Id == columnId)?.Value;
      }
   }
}