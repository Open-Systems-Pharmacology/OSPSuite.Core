using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class DataFormat_TMetaData_C : IDataFormat
   {
      public string Name { get; } = "015_TMetaData_C(E)";

      public string Description { get; } = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639";

      public IList<DataFormatParameter> Parameters { get; private set; }

      public bool SetParameters(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         if((data.GetHeaders()
            .Select(data.GetColumnDescription )
            .Count(header => header.Level == ColumnDescription.MeasurementLevel.Numeric)) < columnInfos.Count(ci => ci.IsMandatory))
            return false;
         
         setParameters(data, columnInfos);
         return true;
      }

      //make this public and call every time setParameters returns true OR rename SetParameters to CheckAndLoadFile
      private void setParameters(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var keys = data.GetHeaders().ToList();
         Parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         extractQualifiedHeadings(keys, missingKeys, columnInfos);
         extractNonQualifiedHeadings(keys, missingKeys, data);
         extractGeneralParameters(keys, data);
      }

      private IEnumerable<string> extractUnits(string description)
      {
         var units = Regex.Match(description, @"\[.+\]").Value;
         if (String.IsNullOrEmpty(units))
            return new List<string>() { "?" };
         return units.Substring(1, units.Length - 2).Trim().Split(',');
      }

      private void extractQualifiedHeadings(List<string> keys, List<string> missingKeys, IReadOnlyList<ColumnInfo> columnInfos)
      {
         if (keys == null) throw new ArgumentNullException(nameof(keys));
         foreach (var header in columnInfos.Select(ci => ci.DisplayName))
         {
            var headerKey = keys.FirstOrDefault(h => h.ToUpper().Contains(header.ToUpper()));
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               var units = extractUnits(headerKey);
               var availableUnits = units.ToList();
               Parameters.Add(new MappingDataFormatParameter
               (
                  headerKey,
                  new Column()
                  {
                     Name = header,
                     Unit = availableUnits.Any() ? availableUnits.ElementAt(0) : "",
                     AvailableUnits = availableUnits
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
                     .OfType<MappingDataFormatParameter>()
                     .All(m => m.ColumnName != h)
               );
            if (headerKey == null) continue;
            keys.Remove(headerKey);
            var units = extractUnits(headerKey);
            var availableUnits = units.ToList();
            Parameters.Add
            (
               new MappingDataFormatParameter
               (
                  headerKey, 
                  new Column() 
                  { 
                     Name = header, 
                     Unit = availableUnits.Any() ? availableUnits.ElementAt(0) : "",
                     AvailableUnits = availableUnits
                  }
               )
            );
         }
      }

      private void extractGeneralParameters(List<string> keys, IUnformattedData data)
      {
         if (keys == null) throw new ArgumentNullException(nameof(keys));
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

      public IReadOnlyDictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>> Parse(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var groupByParams = 
            Parameters
               .Where(p => p is GroupByDataFormatParameter || p is MetaDataFormatParameter)
               .Select(p => (p.ColumnName, data.GetColumnDescription(p.ColumnName).ExistingValues));
         var dataSets = new List<Dictionary<Column, IList<double>>>();
         return buildDataSets(data, groupByParams, columnInfos);
      }

      private Dictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>> buildDataSets(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var dataSets = new Dictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>>();
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
      private void buildDataSetsRecursively(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters, Stack<int> indexes, Dictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>> dataSets, IReadOnlyList<ColumnInfo> columnInfos)
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
                  return valueTuples.All(p => row.ElementAt(data.GetColumnDescription(p.ColumnName).Index) == p.ExistingValues[indexesCopy.ElementAt(index++)]);
               }
            );
            if (rawDataSet.Count() > 0)
            {
               dataSets.Add(
                  valueTuples.Select(p =>
                     new InstanstiatedMetaData()
                     {
                        Id = data.GetColumnDescription(p.ColumnName).Index,
                        Value = rawDataSet.First().ElementAt(data.GetColumnDescription(p.ColumnName).Index)
                     }
                  ),
                  parseMappings(rawDataSet, data, columnInfos)
               );
            }
         }
      }

      private Dictionary<Column, IList<ValueAndLloq>> parseMappings(IEnumerable<IEnumerable<string>> rawDataSet, IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var dictionary = new Dictionary<Column, IList<ValueAndLloq>>();
         //Add time mapping
         var mappingParameters = 
            Parameters
               .OfType<MappingDataFormatParameter>()
               .ToList();

         var dataSet = rawDataSet.ToList();
         foreach (var columnInfo in columnInfos)
         {
            var currentParameter = mappingParameters.First(p => p.MappedColumn.Name.ToString() == columnInfo.DisplayName);
            if (currentParameter != null)
               dictionary.Add
               (
                  currentParameter.MappedColumn,
                  dataSet.Select
                  (
                     row =>
                     {
                        var element = row.ElementAt(data.GetColumnDescription(currentParameter.ColumnName).Index).Trim();
                        if (double.TryParse(element, out double result))
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
                  ).ToList()
               );
         }


         return dictionary;
      }
   }
}
