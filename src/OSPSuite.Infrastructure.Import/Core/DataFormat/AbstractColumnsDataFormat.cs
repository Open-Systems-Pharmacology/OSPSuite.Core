using OSPSuite.Infrastructure.Import.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Core.Import;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public abstract class AbstractColumnsDataFormat : IDataFormat
   {
      public abstract string Name { get; }
      public abstract string Description { get; }
      private IList<DataFormatParameter> _parameters;
      public IList<DataFormatParameter> Parameters { get => _parameters; } //ToDo: Move entities to core and avoid copying this list
      public IList<string> ExcelColumnNames { get; protected set; } = new List<string>();

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
         ExcelColumnNames = keys.ToList();
         _parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         var totalRank = 0.0;
         ExtractQualifiedHeadings(keys, missingKeys, columnInfos, data, ref totalRank);
         ExtractGeneralParameters(keys, data, metaDataCategories, ref totalRank);
         ExtractNonQualifiedHeadings(keys, missingKeys, columnInfos, data, ref totalRank);
         setSecondaryColumnUnit(columnInfos);
         return totalRank;
      }

      private void setSecondaryColumnUnit(IReadOnlyList<ColumnInfo> columnInfos)
      {
         var mappings = Parameters.OfType<MappingDataFormatParameter>();
         foreach (var column in columnInfos.Where(c => !c.IsAuxiliary()))
         {
            foreach (var relatedColumn in columnInfos.Where(c => c.IsAuxiliary() && c.RelatedColumnOf == column.Name))
            {
               var relatedParameter = mappings.FirstOrDefault(p => p.ColumnName == relatedColumn.Name);
               if (relatedParameter != null && (relatedParameter.MappedColumn.Unit == null || relatedParameter.MappedColumn.Unit.SelectedUnit == UnitDescription.InvalidUnit))
               {
                  var mainParameter = mappings.FirstOrDefault(p => p.MappedColumn.Name == column.Name);
                  if (mainParameter != null)
                  {
                     relatedParameter.MappedColumn.Unit = mainParameter.MappedColumn.Unit;
                  }
               }
            }
         }
      }

      protected abstract string ExtractLloq(string description, IUnformattedData data, List<string> keys, ref double rank);

      protected abstract UnitDescription ExtractUnits(string description, IUnformattedData data, List<string> keys, IReadOnlyList<IDimension> supportedDimensions,  ref double rank);

      protected virtual void ExtractQualifiedHeadings(List<string> keys, List<string> missingKeys, IReadOnlyList<ColumnInfo> columnInfos, IUnformattedData data, ref double rank)
      {
         foreach (var header in columnInfos)
         {
            var headerName = header.DisplayName;
            var headerKey = keys.FindHeader(headerName);
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               var units = ExtractUnits(headerKey, data, keys, header.SupportedDimensions, ref rank);

               var col = new Column()
               {
                  Name = headerName,
                  Unit = units,
                  LloqColumn = ExtractLloq(headerKey, data, keys, ref rank)
               };
               if (columnInfos.First(ci => ci.DisplayName == headerName).IsAuxiliary())
               {
                  if(units.ColumnName.IsNullOrEmpty() && units.SelectedUnit == UnitDescription.InvalidUnit)
                     col.ErrorStdDev = Constants.STD_DEV_GEOMETRIC;
                  else
                     col.ErrorStdDev = Constants.STD_DEV_ARITHMETIC;
               }
               _parameters.Add(new MappingDataFormatParameter
               (
                  headerKey,
                  col
               ));
            }
            else
            {
               missingKeys.Add(headerName);
            }
         }
      }

      protected string GetLastBracketsOfString(string header)
      {
         return Regex.Match(header, @"\[([^\]\[]*)\]$").Value;
      }
      protected string GetAndValidateUnitFromBrackets(string units, IReadOnlyList<IDimension> supportedDimensions)
      {
         return units
            .Trim().Substring(1, units.Length - 2).Trim() //remove the brackets and whitespaces from end and beginning
            .Split(',') //we split in case there are more than one units separated with ,
            //only accepts valid and supported units
            .FirstOrDefault(unitName => supportedDimensions.Any(x => x.HasUnit((string)unitName))) ?? UnitDescription.InvalidUnit;     //default = ?
      }

      protected virtual void ExtractNonQualifiedHeadings(List<string> keys, List<string> missingKeys, IReadOnlyList<ColumnInfo> columnInfos, IUnformattedData data, ref double rank)
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
            var units = ExtractUnits(headerKey, data, keys, columnInfos.First(ci => ci.DisplayName == header).SupportedDimensions,  ref rank);

            var col = new Column()
            {
               Name = header,
               Unit = units,
               LloqColumn = ExtractLloq(headerKey, data, keys, ref rank)
            };
            if (columnInfos.First(ci => ci.DisplayName == header).IsAuxiliary())
            {
               col.ErrorStdDev = Constants.STD_DEV_ARITHMETIC;
            }
            _parameters.Add
            (
               new MappingDataFormatParameter
               (
                  headerKey,
                  col
               )
            );
         }
      }

      protected virtual void ExtractGeneralParameters(List<string> keys, IUnformattedData data, IReadOnlyList<MetaDataCategory> metaDataCategories, ref double rank)
      {
         var columnsCopy = keys.ToList();
         foreach (var header in columnsCopy.Where(h => metaDataCategories.Any(c => c.Name == h)))
         {
            keys.Remove(header);
            _parameters.Add(new MetaDataFormatParameter(header, header));
         }
      }

      public IEnumerable<ParsedDataSet> Parse(IUnformattedData data,
         IReadOnlyList<ColumnInfo> columnInfos)
      {
         var groupByParams =
            Parameters
               .Where(p => p is GroupByDataFormatParameter || p is MetaDataFormatParameter)
               .Select(p => (p.ColumnName, p is GroupByDataFormatParameter || (p as MetaDataFormatParameter).IsColumn ? (data.GetColumnDescription(p.ColumnName).ExistingValues) as IList<string> : new List<string>() { p.ColumnName }));
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
               return valueTuples.All(p =>
               {
                  var elementColumn = data.GetColumnDescription(p.ColumnName);
                  var element = elementColumn != null ? row.ElementAt(elementColumn.Index) : p.ColumnName;
                  return element == p.ExistingValues[indexesCopy.ElementAt(index++)];
               });
            }
         ).ToList();
         if (!rawDataSet.Any())
            return;

         dataSets.Add(new ParsedDataSet(valueTuples, data, rawDataSet, parseMappings(rawDataSet, data, columnInfos)));
      }

      private Dictionary<ExtendedColumn, IList<SimulationPoint>> parseMappings(IEnumerable<UnformattedRow> rawDataSet, IUnformattedData data,
         IReadOnlyList<ColumnInfo> columnInfos)
      {
         var dictionary = new Dictionary<ExtendedColumn, IList<SimulationPoint>>();

         //Add time mapping
         var mappingParameters = Parameters.OfType<MappingDataFormatParameter>().ToList();

         var dataSet = rawDataSet.ToList();
         foreach (var columnInfo in columnInfos)
         {
            var currentParameter = mappingParameters.FirstOrDefault(p => p.MappedColumn.Name == columnInfo.DisplayName);
            if (currentParameter == null) continue;
            Func<MappingDataFormatParameter, IUnformattedData, UnformattedRow, SimulationPoint> mappingsParser = 
               currentParameter.MappedColumn.LloqColumn == null ? 
                  (Func<MappingDataFormatParameter, IUnformattedData, UnformattedRow, SimulationPoint>)parseMappingOnSameColumn : 
                  parseMappingOnSameGivenColumn;
            
            dictionary.Add
            (
               new ExtendedColumn
               {
                  Column = currentParameter.MappedColumn,
                  ColumnInfo = columnInfo,
                  ErrorDeviation = currentParameter.MappedColumn.ErrorStdDev
               },
               dataSet.Select
               (
                  row => mappingsParser(currentParameter, data, row)
               ).ToList()
            );
         }

         return dictionary;
      }

      private SimulationPoint parseMappingOnSameColumn(MappingDataFormatParameter currentParameter, IUnformattedData data, UnformattedRow row)
      {
         var columnDescription = data.GetColumnDescription(currentParameter.ColumnName);
         if (columnDescription == null)
            throw new MissingColumnException(currentParameter.ColumnName);

         var element = row.Data.ElementAt(columnDescription.Index).Trim();

         if (!currentParameter.MappedColumn.Unit.ColumnName.IsNullOrEmpty())
         {
            ColumnDescription unitColumnDescription = null;
            if (!string.IsNullOrEmpty(currentParameter.MappedColumn.Unit.ColumnName))
               unitColumnDescription = data.GetColumnDescription(currentParameter.MappedColumn.Unit.ColumnName);
            if (unitColumnDescription == null)
               throw new MissingColumnException(currentParameter.MappedColumn.Unit.ColumnName);
         }

         var unit = currentParameter.MappedColumn.Unit.ExtractUnit(columnName => data.GetColumnDescription(columnName).Index, row.Data);
         
         if (double.TryParse(element, out var result))
            return new SimulationPoint()
            {
               Measurement = result,
               Unit = unit,
               Lloq = double.NaN
            };
         if (element.StartsWith("<"))
         {
            result = element.Substring(1).ConvertedTo<double>();
            return new SimulationPoint()
            {
               Lloq = result,
               Unit = unit
            };
         }

         return new SimulationPoint()
         {
            Measurement = double.NaN,
            Unit = unit
         };
      }

      private SimulationPoint parseMappingOnSameGivenColumn(MappingDataFormatParameter currentParameter, IUnformattedData data, UnformattedRow row)
      {
         var lloqIndex = string.IsNullOrWhiteSpace(currentParameter.MappedColumn.LloqColumn)
            ? -1
            : data.GetColumnDescription(currentParameter.MappedColumn.LloqColumn).Index;
         var unit = currentParameter.MappedColumn.Unit.ExtractUnit(columnName => data.GetColumnDescription(columnName).Index, row.Data);

         if (lloqIndex < 0 || !double.TryParse(row.Data.ElementAt(lloqIndex).Trim(), out var lloq))
            lloq = double.NaN;

         var columnDescription = data.GetColumnDescription(currentParameter.ColumnName);
         
         if (columnDescription == null)
            throw new MissingColumnException(currentParameter.ColumnName);
         
         var element = row.Data.ElementAt(columnDescription.Index).Trim();
         if (double.TryParse(element, out var result))
            return new SimulationPoint()
            {
               Measurement = result,
               Lloq = lloq,
               Unit = unit
            };
         return new SimulationPoint()
         {
            Measurement = double.NaN,
            Lloq = lloq,
            Unit = unit
         };
      }

      public void CopyParametersFromConfiguration(OSPSuite.Core.Import.ImporterConfiguration configuration)
      {
         _parameters = new List<DataFormatParameter>(configuration.Parameters);
      }
   }
}
