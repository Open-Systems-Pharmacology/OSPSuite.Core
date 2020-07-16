
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NPOI.SS.Util;
using System;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class ColumnsDataFormat : IDataFormat
   {
      public string Name => "ColumnsFormat";

      public string Description => "Simple format with data identified by column data...";

      public IList<DataFormatParameter> Parameters { get; private set; }

      public bool CheckFile(IUnformattedData data)
      {
         if (data.Headers.Where(h => h.Value.Level == ColumnDescription.MeasurmentLevel.NUMERIC).Count() < 2)
            return false;
         SetParameters(data);
         return true;
      }

      private void SetParameters(IUnformattedData data)
      {
         var keys = data.Headers.Keys.ToList();
         Parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         extractQualifiedHeadings(data.Headers.Keys.ToList(), missingKeys);
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
               Column.ColumnNames columnName;
               Enum.TryParse(header, out columnName);
               Parameters.Add(new MappingDataFormatParameter(headerKey, new Column() { Name = columnName, Unit = extractUnits(headerKey) }));
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
                  data.Headers[h].Level == ColumnDescription.MeasurmentLevel.NUMERIC && 
                  Parameters.Where(p => p.Type == DataFormatParameterType.MAPPING).Select(p => p as MappingDataFormatParameter).Where(m => m.ColumnName == h).Count() == 0
               );
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               Column.ColumnNames columnName;
               Enum.TryParse(header, out columnName);
               Parameters.Add(new MappingDataFormatParameter(headerKey, new Column() { Name = columnName, Unit = extractUnits(headerKey) }));
            }
         }
      }

      private void extractGeneralParameters(List<string> keys, IUnformattedData data)
      {
         var discreteColumns = keys.Where(h => data.Headers[h].Level == ColumnDescription.MeasurmentLevel.DISCRETE).ToList();
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
         var groupByParams = Parameters.Where(p => p.Type == DataFormatParameterType.GROUP_BY).Select(p => (p.ColumnName, data.Headers[p.ColumnName].ExistingValues));
         var dataSets = new List<Dictionary<Column, IList<double>>>();
         buildDataSet(data, groupByParams, new Stack<int>(), dataSets);
         return dataSets;
      }

      private void buildDataSet(IUnformattedData data, IEnumerable<(string ColumnName, IList<string> ExistingValues)> parameters, Stack<int> indexes, List<Dictionary<Column, IList<double>>> dataSets)
      {
         if (indexes.Count() == parameters.Count())
         {
            var rawDataSet = data.GetRows(row =>
            {
               var check = true;
               var i = 0;
               while (check 
                  && i < indexes.Count)
               {
                  check &= row[data.Headers[parameters.ElementAt(i).ColumnName].Index] == parameters.ElementAt(i).ExistingValues[indexes.ElementAt(indexes.Count - 1 - i)];
                  i++;
               }
               return check;
            });

            var dictionary = new Dictionary<Column, IList<double>>();
            parseMappings(rawDataSet, data, dictionary);

            dataSets.Add(dictionary);
         }
         else
         {
            for (var i = 0; i < parameters.ElementAt(indexes.Count()).ExistingValues.Count(); i++)
            {
               indexes.Push(i);
               buildDataSet(data, parameters, indexes, dataSets);
               indexes.Pop();
            }
         }
      }

      private void parseMappings(IList<IList<string>> rawDataSet, IUnformattedData data, Dictionary<Column, IList<double>> dictionary)
      {
         var mappingParameters = Parameters.Where(p => p.Type == DataFormatParameterType.MAPPING).Select(p => p as MappingDataFormatParameter);
         var timeParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Time); // add Time Measurement and Error as enum in the MappedColumn
         dictionary.Add(timeParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row[data.Headers[timeParameter.ColumnName].Index])).ToList());

         var measurementParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Measurement);
         dictionary.Add(measurementParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row[data.Headers[measurementParameter.ColumnName].Index])).ToList());

         var errorParameter = mappingParameters.First(p => p.MappedColumn.Name == Column.ColumnNames.Error);
         if (errorParameter != null)
            dictionary.Add(errorParameter.MappedColumn, rawDataSet.Select(row => double.Parse(row[data.Headers[errorParameter.ColumnName].Index])).ToList());
      }
   }
}
