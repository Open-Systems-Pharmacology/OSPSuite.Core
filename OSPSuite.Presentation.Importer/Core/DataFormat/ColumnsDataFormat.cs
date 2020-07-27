using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using DevExpress.Charts.Native;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class ColumnsDataFormat : IDataFormat
   {
      public string Name { get; } = "ColumnsFormat";

      public string Description { get; } = "Simple format with data identified by column data...";

      public IList<DataFormatParameter> Parameters { get; private set; }

      public bool CheckFile(IUnformattedData data)
      {
         if (data.Headers.Where(h => h.Value.Level == ColumnDescription.MeasurementLevel.Numeric).Count() < 2)
            return false;
         setParameters(data);
         return true;
      }

      private void setParameters(IUnformattedData data)
      {
         var keys = data.Headers.Keys.ToList();
         Parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         extractQualifiedHeadings(keys, missingKeys);
         extractNonQualifiedHeadings(keys, missingKeys, data);
         extractGeneralParameters(keys, data);
      }

      private string extractUnits(string description)
      {
         var units = Regex.Match(description, @"\[.+\]").Value;
         return units.Substring(1, units.Length - 2).Trim();
      }

      private void extractQualifiedHeadings(List<string> keys, List<string> missingKeys)
      {
         foreach (var header in Enum.GetNames(typeof(Column.ColumnNames)))
         {
            var headerKey = keys.FirstOrDefault(h => h.ToUpper().Contains(header.ToUpper()));
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               Parameters.Add(new MappingDataFormatParameter(headerKey, new Column() { Name = Utility.EnumHelper.ParseValue<Column.ColumnNames>(header), Unit = extractUnits(headerKey) }));
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
                  data.Headers[h].Level == ColumnDescription.MeasurementLevel.Numeric && 
                  Parameters
                     .Where(p => p.Type == DataFormatParameterType.Mapping)
                     .Select(p => p as MappingDataFormatParameter)
                     .All(m => m.ColumnName != h)
               );
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               Parameters.Add(new MappingDataFormatParameter(headerKey, new Column() { Name = Utility.EnumHelper.ParseValue<Column.ColumnNames>(header), Unit = extractUnits(headerKey) }));
            }
         }
      }

      private void extractGeneralParameters(List<string> keys, IUnformattedData data)
      {
         var discreteColumns = keys.Where(h => data.Headers[h].Level == ColumnDescription.MeasurementLevel.Discrete).ToList();
         foreach (var header in discreteColumns.Where(h => data.Headers[h].ExistingValues.Count == 1))
         {
            keys.Remove(header);
            Parameters.Add(new MetaDataFormatParameter(header));
         }
         foreach (var header in discreteColumns.Where(h => data.Headers[h].ExistingValues.Count > 1))
         {
            keys.Remove(header);
            Parameters.Add(new GroupByDataFormatParameter(header));
         }
      }

      public IList<Dictionary<Column, IList<double>>> Parse(IUnformattedData data)
      {
         var groupByParams = Parameters.Where(p => p.Type == DataFormatParameterType.GroupBy).Select(p => (p.ColumnName, data.Headers[p.ColumnName].ExistingValues));
         var dataSets = new List<Dictionary<Column, IList<double>>>();
         return buildDataSets(data, groupByParams);
      }

      private List<Dictionary<Column, IList<double>>> buildDataSets(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters)
      {
         var dataSets = new List<Dictionary<Column, IList<double>>>();
         buildDataSetsRecursively(data, parameters, new Stack<int>(), dataSets);
         return dataSets;
      }

      /// <summary>
      /// Populates the datasets from the data recursively.
      /// </summary>
      /// <param name="data">The unformatted source data</param>
      /// <param name="parameters">Parameters of the format for grouping by</param>
      /// <param name="indexes">List of indexes for the recursion</param>
      /// <param name="dataSets">List to store the datasets</param>
      private void buildDataSetsRecursively(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters, Stack<int> indexes, List<Dictionary<Column, IList<double>>> dataSets)
      {
         if (indexes.Count() < parameters.Count()) //Still traversing the parameters
         {
            for (var i = 0; i < parameters.ElementAt(indexes.Count()).ExistingValues.Count(); i++) //For every existing value on the current parameter
            {
               indexes.Push(i);
               buildDataSetsRecursively(data, parameters, indexes, dataSets);
               indexes.Pop();
            }
         }
         else //Fully traversed the parameters list
         {
            //Filter based on the parameters
            var indexesCopy = indexes.ToList();
            var rawDataSet = data.GetRows(
               row =>
               {
                  var index = 0;
                  return parameters.All(p => row.ElementAt(data.Headers[p.ColumnName].Index) == p.ExistingValues[indexesCopy.ElementAt(indexesCopy.Count - 1 - (index++))]);
               }
            );
            dataSets.Add(parseMappings(rawDataSet, data));
         }
      }

      private Dictionary<Column, IList<double>> parseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data)
      {
         var dictionary = new Dictionary<Column, IList<double>>();
         //Add time mapping
         var mappingParameters = Parameters.Where(p => p.Type == DataFormatParameterType.Mapping).Select(p => p as MappingDataFormatParameter).ToList();
         var timeParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Time);
         dictionary.Add(timeParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row.ElementAt(data.Headers[timeParameter.ColumnName].Index))).ToList());

         //Add measurement mapping
         var measurementParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Concentration);
         dictionary.Add(measurementParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row.ElementAt(data.Headers[measurementParameter.ColumnName].Index))).ToList());

         //Add error mapping
         var errorParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Error);
         if (errorParameter != null)
            dictionary.Add(errorParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row.ElementAt(data.Headers[errorParameter.ColumnName].Index))).ToList());

         return dictionary;
      }
   }
}
