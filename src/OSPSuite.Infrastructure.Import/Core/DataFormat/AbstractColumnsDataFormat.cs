using OSPSuite.Infrastructure.Import.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public abstract class AbstractColumnsDataFormat : IDataFormat
   {
      public abstract string Name { get; }
      public abstract string Description { get; }
      public IList<DataFormatParameter> Parameters { get; protected set; }

      public double SetParameters(IUnformattedData rawData, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         if (NotCompatible(rawData, columnInfos))
            return 0;

         return 1 + setParameters(rawData, columnInfos, metaDataCategories);
      }

      protected bool NotCompatible(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         return (data.GetHeaders()
            .Select(data.GetColumnDescription)
            .Count(header => header.Level == ColumnDescription.MeasurementLevel.Numeric)) < columnInfos.Count(ci => ci.IsMandatory);
      }

      private double setParameters(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         var keys = data.GetHeaders().ToList();
         Parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         var totalRank = 0.0;
         ExtractQualifiedHeadings(keys, missingKeys, columnInfos, data, ref totalRank);
         ExtractNonQualifiedHeadings(keys, missingKeys, data, ref totalRank);
         ExtractGeneralParameters(keys, data, metaDataCategories, ref totalRank);
         return totalRank;
      }

      protected abstract string ExtractLloq(string description, IUnformattedData data, List<string> keys, ref double rank);

      protected abstract UnitDescription ExtractUnits(string description, IUnformattedData data, List<string> keys, ref double rank);

      protected virtual void ExtractQualifiedHeadings(List<string> keys, List<string> missingKeys, IReadOnlyList<ColumnInfo> columnInfos, IUnformattedData data, ref double rank)
      {
         foreach (var header in columnInfos.Select(ci => ci.DisplayName))
         {
            var headerKey = keys.FindHeader(header);
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               var units = ExtractUnits(headerKey, data, keys, ref rank);

               Parameters.Add(new MappingDataFormatParameter
               (
                  headerKey,
                  new Column()
                  {
                     Name = header,
                     Unit = units,
                     LloqColumn = ExtractLloq(headerKey, data, keys, ref rank)
                  })
               );
            }
            else
            {
               missingKeys.Add(header);
            }
         }
      }

      protected virtual void ExtractNonQualifiedHeadings(List<string> keys, List<string> missingKeys, IUnformattedData data, ref double rank)
      {
         foreach (var header in missingKeys)
         {
            var headerKey = keys.FirstOrDefault
               (h =>
                  //TODO: Only add this if we make a robust decision on what columns are numeric
                  data.GetColumnDescription(h).Level == ColumnDescription.MeasurementLevel.Numeric &&
                  Parameters
                     .OfType<MappingDataFormatParameter>()
                     .All(m => m.ColumnName != h)
               );
            if (headerKey == null) continue;
            keys.Remove(headerKey);
            var units = ExtractUnits(headerKey, data, keys, ref rank);
            Parameters.Add
            (
               new MappingDataFormatParameter
               (
                  headerKey,
                  new Column()
                  {
                     Name = header,
                     Unit = units,
                     LloqColumn = ExtractLloq(headerKey, data, keys, ref rank)
                  }
               )
            );
         }
      }

      protected virtual void ExtractGeneralParameters(List<string> keys, IUnformattedData data, IReadOnlyList<MetaDataCategory> metaDataCategories, ref double rank)
      {
         var discreteColumns = keys.Where(h => data.GetColumnDescription(h).Level == ColumnDescription.MeasurementLevel.Discrete).ToList();
         foreach (var header in discreteColumns.Where(h => metaDataCategories.Any(c => c.Name == h)))
         {
            keys.Remove(header);
            Parameters.Add(new MetaDataFormatParameter(header, header));
         }
         foreach (var header in discreteColumns.Where(h => metaDataCategories.All(c => c.Name != h)))
         {
            keys.Remove(header);
            Parameters.Add(new GroupByDataFormatParameter(header));
         }
      }

      public IEnumerable<ParsedDataSet> Parse(IUnformattedData data,
         IReadOnlyList<ColumnInfo> columnInfos)
      {
         var groupByParams =
            Parameters
               .Where(p => p is GroupByDataFormatParameter || p is MetaDataFormatParameter)
               .Select(p => (p.ColumnName, data.GetColumnDescription(p.ColumnName).ExistingValues));
         return buildDataSets(data, groupByParams, columnInfos);
      }

      private IEnumerable<ParsedDataSet> buildDataSets(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var dataSets = new List<ParsedDataSet>();
         buildDataSetsRecursively(data, parameters, new Stack<int>(), dataSets, columnInfos);
         return dataSets;
      }

      /// <summary>
      /// Populates the dataSets from the data recursively.
      /// </summary>
      /// <param name="data">The unformatted source data</param>
      /// <param name="parameters">Parameters of the format for grouping by</param>
      /// <param name="indexes">List of indexes for the recursion, each index encodes an existingValue, 
      /// e.g. [1,2,1] would mean that the first parameter is constraint to have its value equal to its ExistingValue on index 1,
      /// the second parameter is constraint to have its value equal to its ExistingValue on index 2, and
      /// the third parameter is constraint to have its value equal to its ExistingValue on index 1</param>
      /// <param name="dataSets">List to store the dataSets</param>
      private void buildDataSetsRecursively(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters, Stack<int> indexes, List<ParsedDataSet> dataSets, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var valueTuples = parameters.ToList();
         if (indexes.Count() < valueTuples.Count()) //Still traversing the parameters
         {
            for (var i = 0; i < valueTuples.ElementAt(indexes.Count()).ExistingValues.Count(); i++) //For every existing value on the current parameter
            {
               indexes.Push(i);
               buildDataSetsRecursively(data, valueTuples, indexes, dataSets, columnInfos);
               indexes.Pop();
            }

            return;
         }

         //Filter based on the parameters
         var indexesCopy = indexes.ToList();
         indexesCopy.Reverse();
         var rawDataSet = data.GetRows(
            row =>
            {
               var index = 0;
               return valueTuples.All(p => row.ElementAt(data.GetColumnDescription(p.ColumnName).Index) == p.ExistingValues[indexesCopy.ElementAt(index++)]);
            }
         ).ToList();
         if (!rawDataSet.Any())
            return;

         dataSets.Add(new ParsedDataSet(valueTuples, data, rawDataSet, parseMappings(rawDataSet, data, columnInfos)));
      }

      private Dictionary<Column, IList<ValueAndLloq>> parseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data,
         IReadOnlyList<ColumnInfo> columnInfos)
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
               currentParameter.MappedColumn.LloqColumn == null ?
                  parseMappingOnSameColumn(currentParameter, data, dataSet) :
                  parseMappingOnSameGivenColumn(currentParameter, data, dataSet)
            );
         }

         return dictionary;
      }

      private IList<ValueAndLloq> parseMappingOnSameColumn(MappingDataFormatParameter currentParameter, IUnformattedData data, IReadOnlyList<IEnumerable<string>> dataSet)
      {
         return dataSet.Select
         (
            row =>
            {
               var element = row.ElementAt(data.GetColumnDescription(currentParameter.ColumnName).Index).Trim();
               if (double.TryParse(element, out var result))
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
         ).ToList();
      }

      private IList<ValueAndLloq> parseMappingOnSameGivenColumn(MappingDataFormatParameter currentParameter, IUnformattedData data, IReadOnlyList<IEnumerable<string>> dataSet)
      {
         return dataSet.Select
         (
            row =>
            {
               var lloqIndex = string.IsNullOrWhiteSpace(currentParameter.MappedColumn.LloqColumn)
                  ? -1
                  : data.GetColumnDescription(currentParameter.MappedColumn.LloqColumn).Index;

               if (lloqIndex < 0 || !double.TryParse(row.ElementAt(lloqIndex).Trim(), out var lloq))
                  lloq = double.NaN;

               var element = row.ElementAt(data.GetColumnDescription(currentParameter.ColumnName).Index).Trim();
               if (double.TryParse(element, out var result))
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
         ).ToList();
      }
   }
}
