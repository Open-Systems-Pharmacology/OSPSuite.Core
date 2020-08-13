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

      public bool SetParameters(IUnformattedData data)
      {
         if((data.GetHeaders()
            .Select(data.GetColumnDescription )
            .Count(header => header.Level == ColumnDescription.MeasurementLevel.Numeric)) <2 )
            return false;
         
         setParameters(data);
         return true;
      }

      /// <summary>
      ///    This method checks whether all conditions are fullfilled.
      /// </summary>
      /// <param name="conditions">Dictionary of column name, value pairs.</param>
      /// <returns>True, if all conditions are fullfilled.</returns>
      public bool CheckConditions(Dictionary<string, string> conditions)
      {
         var valid = false;
         foreach (var condition in conditions)
         {
            var parameter = Parameters.FirstOrDefault(p => p.ColumnName == condition.Key);
            if (parameter == null)
               throw new Exception($"Unknown column {condition.Key}.");

            valid = (parameter.Configuration.Data.ToString() == condition.Value);
         }
         return valid;
      }

      //make this public and call every time SetParameters returns true OR rename SetParameters to CheckAndLoadFile
      private void setParameters(IUnformattedData data)
      {
         var keys = data.GetHeaders().ToList();
         Parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         extractQualifiedHeadings(keys, missingKeys);
         extractNonQualifiedHeadings(keys, missingKeys, data);
         extractGeneralParameters(keys, data);
      }

      private IEnumerable<string> extractUnits(string description)
      {
         var units = Regex.Match(description, @"\[.+\]").Value;
         return units.Substring(1, units.Length - 2).Trim().Split(',');
      }

      private void extractQualifiedHeadings(List<string> keys, List<string> missingKeys)
      {
         foreach (var header in Enum.GetNames(typeof(Column.ColumnNames)))
         {
            var headerKey = keys.FirstOrDefault(h => h.ToUpper().Contains(header.ToUpper()));
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               var units = extractUnits(headerKey);
               Parameters.Add(new MappingDataFormatParameter
               (
                  headerKey,
                  new Column()
                  {
                     Name = Utility.EnumHelper.ParseValue<Column.ColumnNames>(header),
                     Unit = units.Count() > 0 ? units.ElementAt(0) : "",
                     AvailableUnits = units
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
                  data.GetColumnDescription(h).Level == ColumnDescription.MeasurementLevel.Numeric && 
                  Parameters
                     .Where(p => p is MappingDataFormatParameter)
                     .Select(p => p as MappingDataFormatParameter)
                     .All(m => m.ColumnName != h)
               );
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               var units = extractUnits(headerKey);
               Parameters.Add
               (
                  new MappingDataFormatParameter
                  (
                     headerKey, 
                     new Column() 
                     { 
                        Name = Utility.EnumHelper.ParseValue<Column.ColumnNames>(header), 
                        Unit = units.Count() > 0 ? units.ElementAt(0) : "",
                        AvailableUnits = units
                     }
                  )
               );
            }
         }
      }

      private void extractGeneralParameters(List<string> keys, IUnformattedData data)
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

      public IList<Dictionary<Column, IList<double>>> Parse(IUnformattedData data)
      {
         var groupByParams = 
            Parameters
               .Where(p => p is GroupByDataFormatParameter)
               .Select(p => (p.ColumnName, data.GetColumnDescription(p.ColumnName).ExistingValues));
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
      /// <param name="indexes">List of indexes for the recursion, each index encodes an existingValue, 
      /// e.g. [1,2,1] would mean that the first parameter is constraint to have its value equal to its ExistingValue on index 1,
      /// the second parameter is constraint to have its value equal to its ExistingValue on index 2, and
      /// the third parameter is constraint to have its value equal to its ExistingValue on index 1</param>
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
            indexesCopy.Reverse();
            var rawDataSet = data.GetRows(
               row =>
               {
                  var index = 0;
                  return parameters.All(p => row.ElementAt(data.GetColumnDescription(p.ColumnName).Index) == p.ExistingValues[indexesCopy.ElementAt(index++)]);
               }
            );
            dataSets.Add(parseMappings(rawDataSet, data));
         }
      }

      private Dictionary<Column, IList<double>> parseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data)
      {
         var dictionary = new Dictionary<Column, IList<double>>();
         //Add time mapping
         var mappingParameters = 
            Parameters
               .Where(p => p is MappingDataFormatParameter)
               .Select(p => p as MappingDataFormatParameter)
               .ToList();
         var timeParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Time);
         dictionary.Add(timeParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row.ElementAt(data.GetColumnDescription(timeParameter.ColumnName).Index))).ToList());

         //Add measurement mapping
         var measurementParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Concentration);
         dictionary.Add(measurementParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row.ElementAt(data.GetColumnDescription(measurementParameter.ColumnName).Index))).ToList());

         //Add error mapping
         var errorParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Error);
         if (errorParameter != null)
            dictionary.Add(errorParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row.ElementAt(data.GetColumnDescription(errorParameter.ColumnName).Index))).ToList());

         return dictionary;
      }
   }
}
