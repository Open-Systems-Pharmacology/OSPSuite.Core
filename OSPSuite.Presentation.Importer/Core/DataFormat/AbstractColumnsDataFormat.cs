using OSPSuite.Core.Importer;
using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Importer.Core.Extensions;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public abstract class AbstractColumnsDataFormat : IDataFormat
   {
      const int _columnNotFound = -1;
      public abstract string Name { get; }
      public abstract string Description { get; }
      public IList<DataFormatParameter> Parameters { get; protected set; }

      public bool SetParameters(IUnformattedData rawData, IReadOnlyList<ColumnInfo> columnInfos)
      {
         if (NotCompatible(rawData, columnInfos))
            return false;

         setParameters(rawData, columnInfos);
         return true;
      }

      protected bool NotCompatible(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         return (data.GetHeaders()
            .Select(data.GetColumnDescription)
            .Count(header => header.Level == ColumnDescription.MeasurementLevel.Numeric)) < columnInfos.Count(ci => ci.IsMandatory);
      }

      private void setParameters(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var keys = data.GetHeaders().ToList();
         Parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         extractQualifiedHeadings(keys, missingKeys, columnInfos, data);
         extractNonQualifiedHeadings(keys, missingKeys, data);
         ExtractGeneralParameters(keys, data);
      }

      protected abstract Func<int, string> ExtractUnits(string description, IUnformattedData data, List<string> keys);

      private void extractQualifiedHeadings(List<string> keys, List<string> missingKeys, IReadOnlyList<ColumnInfo> columnInfos, IUnformattedData data)
      {
         foreach (var header in columnInfos.Select(ci => ci.DisplayName))
         {
            var headerKey = keys.FindHeader(header);
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               var units = ExtractUnits(headerKey, data, keys);
               Parameters.Add(new MappingDataFormatParameter
               (
                  headerKey,
                  new Column()
                  {
                     Name = header,
                     Units = units,
                     SelectedUnit = units(_columnNotFound)
                  })
               );
            }
            else
            {
               missingKeys.Add(header);
            }
         }
      }

      private void extractNonQualifiedHeadings(List<string> keys, List<string> missingKeys, IUnformattedData data)
      {
         foreach (var header in missingKeys)
         {
            var headerKey = keys.FirstOrDefault
               (h =>
                  //TODO: Only add this if we make a robust decision on what columns are numeric
                  //data.GetColumnDescription(h).Level == ColumnDescription.MeasurementLevel.Numeric &&
                  Parameters
                     .OfType<MappingDataFormatParameter>()
                     .All(m => m.ColumnName != h)
               );
            if (headerKey == null) continue;
            keys.Remove(headerKey);
            var units = ExtractUnits(headerKey, data, keys);
            Parameters.Add
            (
               new MappingDataFormatParameter
               (
                  headerKey,
                  new Column()
                  {
                     Name = header,
                     Units = units,
                     SelectedUnit = units(_columnNotFound)
                  }
               )
            );
         }
      }

      protected virtual void ExtractGeneralParameters(List<string> keys, IUnformattedData data)
      {
         var discreteColumns = keys.Where(h => data.GetColumnDescription(h).Level == ColumnDescription.MeasurementLevel.Discrete).ToList();
         foreach (var header in discreteColumns.Where(h => data.GetColumnDescription(h).ExistingValues.Count == 1))
         {
            keys.Remove(header);
            Parameters.Add(new MetaDataFormatParameter(header, header));
         }
         foreach (var header in discreteColumns.Where(h => data.GetColumnDescription(h).ExistingValues.Count > 1))
         {
            keys.Remove(header);
            Parameters.Add(new GroupByDataFormatParameter(header));
         }
      }

      public IEnumerable<IParsedDataSet> Parse(IUnformattedData data,
         IReadOnlyList<ColumnInfo> columnInfos)
      {
         var groupByParams =
            Parameters
               .Where(p => p is GroupByDataFormatParameter || p is MetaDataFormatParameter)
               .Select(p => (p.ColumnName, data.GetColumnDescription(p.ColumnName).ExistingValues));
         return buildDataSets(data, groupByParams, columnInfos);
      }

      private IEnumerable<IParsedDataSet> buildDataSets(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var dataSets = new List<IParsedDataSet>();
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
      private void buildDataSetsRecursively(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters, Stack<int> indexes, List<IParsedDataSet> dataSets, IReadOnlyList<ColumnInfo> columnInfos)
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
         if (rawDataSet.Any())
         {
            dataSets.Add(
               new ParsedDataSet()
               {
                  Description = valueTuples.Select(p =>
                     new InstantiatedMetaData()
                     {
                        Id = data.GetColumnDescription(p.ColumnName).Index,
                        Value = rawDataSet.First().ElementAt(data.GetColumnDescription(p.ColumnName).Index)
                     }
                  ),
                  Data = ParseMappings(rawDataSet, data, columnInfos)
               }
            );
         }
      }

      protected abstract Dictionary<Column, IList<ValueAndLloq>> ParseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);
   }
}
