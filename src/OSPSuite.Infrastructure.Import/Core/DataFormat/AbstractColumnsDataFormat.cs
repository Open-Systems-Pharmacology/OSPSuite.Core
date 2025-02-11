using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public abstract class AbstractColumnsDataFormat : IDataFormat
   {
      public abstract string Name { get; }
      public abstract string Description { get; }
      public IList<DataFormatParameter> Parameters { get; private set; }

      public IList<string> ExcelColumnNames { get; protected set; } = new List<string>();

      public double SetParameters(DataSheet rawDataSheet, ColumnInfoCache columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         if (NotCompatible(rawDataSheet, columnInfos))
            return 0;

         return 1 + setParameters(rawDataSheet, columnInfos, metaDataCategories);
      }

      public IEnumerable<T> GetParameters<T>() where T : DataFormatParameter
      {
         return Parameters.OfType<T>();
      }

      public T GetColumnByName<T>(string columnName) where T : DataFormatParameter
      {
         return Parameters.OfType<T>().FirstOrDefault(x => x.ColumnName == columnName);
      }

      protected bool NotCompatible(DataSheet dataSheet, ColumnInfoCache columnInfos)
      {
         return (dataSheet.GetHeaders()
            .Select(dataSheet.GetColumnDescription)
            .Count(header => header.Level == ColumnDescription.MeasurementLevel.Numeric)) < columnInfos.Count(ci => ci.IsMandatory);
      }

      private double setParameters(DataSheet dataSheet, ColumnInfoCache columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         var keys = dataSheet.GetHeaders().ToList();
         ExcelColumnNames = keys.ToList();
         Parameters = new List<DataFormatParameter>();

         var missingKeys = new List<string>();

         var totalRank = 0.0;
         ExtractQualifiedHeadings(keys, missingKeys, columnInfos, dataSheet, ref totalRank);
         ExtractGeneralParameters(keys, dataSheet, metaDataCategories, ref totalRank);
         ExtractNonQualifiedHeadings(keys, missingKeys, columnInfos, dataSheet, ref totalRank);
         setSecondaryColumnUnit(columnInfos);
         setDimensionsForMappings(columnInfos);
         return totalRank;
      }

      private void setDimensionsForMappings(ColumnInfoCache columnInfos)
      {
         foreach (var parameter in GetParameters<MappingDataFormatParameter>())
         {
            var mappedColumn = parameter.MappedColumn;

            if (mappedColumn?.Unit == null || mappedColumn.Dimension != null)
               continue;

            var concreteColumnInfo = columnInfos[mappedColumn.Name];
            //initial settings for fraction dimension
            if (concreteColumnInfo.DefaultDimension?.Name == Constants.Dimension.FRACTION &&
                mappedColumn.Unit.ColumnName.IsNullOrEmpty() &&
                mappedColumn.Unit.SelectedUnit == UnitDescription.InvalidUnit)
            {
               mappedColumn.Dimension = concreteColumnInfo.DefaultDimension;
               mappedColumn.Unit = new UnitDescription(mappedColumn.Dimension.DefaultUnitName);
               continue;
            }

            if (!mappedColumn.Unit.ColumnName.IsNullOrEmpty())
               mappedColumn.Dimension = null;
            else
            {
               var dimensionForUnit = concreteColumnInfo.DimensionForUnit(mappedColumn.Unit.SelectedUnit);

               if (dimensionForUnit == null)
                  mappedColumn.Unit = new UnitDescription(UnitDescription.InvalidUnit);
               else
                  mappedColumn.Dimension = dimensionForUnit;
            }
         }
      }

      private void setSecondaryColumnUnit(ColumnInfoCache columnInfos)
      {
         var mappings = GetParameters<MappingDataFormatParameter>().ToList();
         foreach (var column in columnInfos.Where(c => !c.IsAuxiliary))
         {
            foreach (var relatedColumn in columnInfos.RelatedColumnsFrom(column.Name))
            {
               var relatedParameter = mappings.FirstOrDefault(p => p.ColumnName == relatedColumn.Name);
               if (relatedParameter != null && (relatedParameter.MappedColumn.Unit == null ||
                                                relatedParameter.MappedColumn.Unit.SelectedUnit == UnitDescription.InvalidUnit))
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

      protected abstract string ExtractLLOQ(string description, DataSheet dataSheet, List<string> keys, ref double rank);

      protected abstract UnitDescription ExtractUnits(string description, DataSheet dataSheet, List<string> keys,
         ColumnInfo columnInfo, ref double rank);

      public UnitDescription ExtractUnitDescriptions(string description, ColumnInfo columnInfo)
      {
         var rank = 0.0;
         return ExtractUnits(description, dataSheet: null, keys: null, columnInfo, ref rank);
      }

      protected virtual void ExtractQualifiedHeadings(List<string> keys, List<string> missingKeys, Cache<string, ColumnInfo> columnInfos,
         DataSheet dataSheet, ref double rank)
      {
         foreach (var header in columnInfos)
         {
            var headerName = header.DisplayName;
            var headerKey = keys.FindHeader(headerName);
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               var units = ExtractUnits(headerKey, dataSheet, keys, header, ref rank);

               var col = new Column
               {
                  Name = headerName,
                  Unit = units,
                  LloqColumn = ExtractLLOQ(headerKey, dataSheet, keys, ref rank)
               };
               if (columnInfos[headerName].IsAuxiliary)
               {
                  if (units.SelectedUnit.IsNullOrEmpty())
                     col.ErrorStdDev = Constants.STD_DEV_GEOMETRIC;
                  else
                     col.ErrorStdDev = Constants.STD_DEV_ARITHMETIC;
               }

               Parameters.Add(new MappingDataFormatParameter
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

      protected string ValidateUnit(string unit, ColumnInfo columnInfo)
      {
         var dimensionForUnit = columnInfo.DimensionForUnit(unit);

         if (dimensionForUnit == null)
            return UnitDescription.InvalidUnit;

         //We know it exists here as it was found previously
         return dimensionForUnit.FindUnit(unit, ignoreCase: true).Name;
      }

      protected virtual void ExtractNonQualifiedHeadings(List<string> keys, List<string> missingKeys, Cache<string, ColumnInfo> columnInfos,
         DataSheet dataSheet, ref double rank)
      {
         foreach (var header in missingKeys)
         {
            var headerKey = keys.FirstOrDefault
            (h =>
               dataSheet.GetColumnDescription(h).Level == ColumnDescription.MeasurementLevel.Numeric &&
               GetParameters<MappingDataFormatParameter>()
                  .All(m => m.ColumnName != h)
            );
            if (headerKey == null) continue;
            keys.Remove(headerKey);
            var units = ExtractUnits(headerKey, dataSheet, keys, columnInfos[header], ref rank);

            var col = new Column()
            {
               Name = header,
               Unit = units,
               LloqColumn = ExtractLLOQ(headerKey, dataSheet, keys, ref rank)
            };
            if (columnInfos[header].IsAuxiliary)
            {
               col.ErrorStdDev = Constants.STD_DEV_ARITHMETIC;
            }

            Parameters.Add
            (
               new MappingDataFormatParameter
               (
                  headerKey,
                  col
               )
            );
         }
      }

      protected virtual void ExtractGeneralParameters(
         List<string> keys,
         DataSheet dataSheet,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         ref double rank)
      {
         var matchingHeaders = getMatchingHeaders(keys, metaDataCategories);

         foreach (var header in matchingHeaders)
         {
            var key = findHeaderInMetaDataCategories(header, metaDataCategories);
            addParameterIfNew(header, key, keys);
         }
      }

      private IEnumerable<string> getMatchingHeaders(IEnumerable<string> keys, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         var metaDataCategoryNames = metaDataCategories.Select(c => c.Name).ToList();
         return keys.Where(header => metaDataCategoryNames.FindHeader(header) != null);
      }


      private void addParameterIfNew(string header, string key, List<string> keys)
      {
         if (!isMetaDataParameterExisting(key))
         {
            keys.Remove(header);
            Parameters.Add(new MetaDataFormatParameter(header, key));
         }
      }

      private bool isMetaDataParameterExisting(string key)
      {
         return Parameters.OfType<MetaDataFormatParameter>().Any(x => x.MetaDataId.Equals(key));
      }

      private string findHeaderInMetaDataCategories(string header, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         return metaDataCategories.Select(c => c.Name).FindHeader(header);
      }

      public IEnumerable<ParsedDataSet> Parse(DataSheet dataSheet, ColumnInfoCache columnInfos)
      {
         var missingColumns = Parameters.Where(p => p.ComesFromColumn() && dataSheet.GetColumnDescription(p.ColumnName) == null)
            .Select(p => p.ColumnName).ToList();
         if (missingColumns.Any())
            throw new MissingColumnException(dataSheet.SheetName, missingColumns);

         //we first isolate the mapping parameters
         var mappingCriteria =
            Parameters
               .Where(p => p.IsGroupingCriterion() && !p.IsAnImplementationOf<GroupByDataFormatParameter>())
               .Select(p => p.ColumnName);

         //and then add the grouping criteria, in order to have exactly the same order in the ParsedDataSet
         //that will be created as in the mappings in the dataSource
         var groupingCriteria = mappingCriteria.Union(Parameters
            .Where(p => p.IsGroupingCriterion() && p.IsAnImplementationOf<GroupByDataFormatParameter>())
            .Select(p => p.ColumnName));

         return buildDataSets(dataSheet, groupingCriteria.ToList(), columnInfos);
      }

      private string rowId(IEnumerable<string> parameters, DataSheet dataSheet, UnformattedRow row)
      {
         return string
            .Join(
               ",",
               parameters.Select(parameter =>
               {
                  var elementColumn = dataSheet.GetColumnDescription(parameter);
                  return elementColumn != null ? row.Data.ElementAt(elementColumn.Index) : parameter;
               })
            );
      }

      private IEnumerable<ParsedDataSet> buildDataSets(DataSheet dataSheet, IReadOnlyList<string> groupingParameters,
         Cache<string, ColumnInfo> columnInfos)
      {
         var cachedUnformattedRows = new Cache<string, List<UnformattedRow>>();
         foreach (var row in dataSheet.GetRows(_ => true))
         {
            var id = rowId(groupingParameters, dataSheet, row);
            if (!cachedUnformattedRows.Contains(id))
               cachedUnformattedRows.Add(id, new List<UnformattedRow>());
            cachedUnformattedRows[id].Add(row);
         }

         return cachedUnformattedRows.Select(rows =>
            new ParsedDataSet(groupingParameters, dataSheet, rows, parseMappings(rows, dataSheet, columnInfos)));
      }

      private Dictionary<ExtendedColumn, IList<SimulationPoint>> parseMappings(IEnumerable<UnformattedRow> rawDataSet, DataSheet dataSheet,
         Cache<string, ColumnInfo> columnInfos)
      {
         var dictionary = new Dictionary<ExtendedColumn, IList<SimulationPoint>>();

         //Add time mapping
         var mappingParameters = GetParameters<MappingDataFormatParameter>().ToList();

         var dataSet = rawDataSet.ToList();
         foreach (var columnInfo in columnInfos)
         {
            var currentParameter = mappingParameters.FirstOrDefault(p => p.MappedColumn.Name == columnInfo.DisplayName);
            if (currentParameter == null) continue;
            Func<MappingDataFormatParameter, DataSheet, UnformattedRow, SimulationPoint> mappingsParser =
               currentParameter.MappedColumn.LloqColumn == null
                  ? (Func<MappingDataFormatParameter, DataSheet, UnformattedRow, SimulationPoint>)parseMappingOnSameColumn
                  : parseMappingOnSameGivenColumn;

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
                  row => mappingsParser(currentParameter, dataSheet, row)
               ).ToList()
            );
         }

         return dictionary;
      }

      private SimulationPoint parseMappingOnSameColumn(MappingDataFormatParameter currentParameter, DataSheet dataSheet, UnformattedRow row)
      {
         var columnDescription = dataSheet.GetColumnDescription(currentParameter.ColumnName);
         var element = row.Data.ElementAt(columnDescription.Index).Trim();

         //unit comes from a column
         if (!currentParameter.MappedColumn.Unit.ColumnName.IsNullOrEmpty())
         {
            checkThatMappedColumnExists(currentParameter.MappedColumn.Unit.ColumnName, dataSheet);
         }

         var unit = currentParameter.MappedColumn.Unit.ExtractUnit(columnName => dataSheet.GetColumnDescription(columnName).Index, row.Data);

         if (double.TryParse(element, out var result))
            return new SimulationPoint
            {
               Measurement = result,
               Unit = unit,
               Lloq = double.NaN
            };
         if (element.StartsWith("<"))
         {
            result = element.Substring(1).ConvertedTo<double>();
            return new SimulationPoint
            {
               Lloq = result,
               Unit = unit
            };
         }

         return new SimulationPoint
         {
            Measurement = double.NaN,
            Unit = unit
         };
      }

      private static void checkThatMappedColumnExists(string columnName, DataSheet dataSheet)
      {
         var columnDescription = dataSheet.GetColumnDescription(columnName);
         if (columnDescription == null)
            throw new MissingColumnException(dataSheet.SheetName, columnName);
      }

      private SimulationPoint parseMappingOnSameGivenColumn(MappingDataFormatParameter currentParameter, DataSheet dataSheet, UnformattedRow row)
      {
         //Lloq comes from a column
         if (!currentParameter.MappedColumn.LloqColumn.IsNullOrEmpty())
         {
            checkThatMappedColumnExists(currentParameter.MappedColumn.LloqColumn, dataSheet);
         }

         var lloqIndex = string.IsNullOrWhiteSpace(currentParameter.MappedColumn.LloqColumn)
            ? -1
            : dataSheet.GetColumnDescription(currentParameter.MappedColumn.LloqColumn).Index;
         var unit = currentParameter.MappedColumn.Unit.ExtractUnit(columnName => dataSheet.GetColumnDescription(columnName).Index, row.Data);

         if (lloqIndex < 0 || !double.TryParse(row.Data.ElementAt(lloqIndex).Trim(), out var lloq))
            lloq = double.NaN;

         var columnDescription = dataSheet.GetColumnDescription(currentParameter.ColumnName);

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
         Parameters = new List<DataFormatParameter>(configuration.Parameters);
      }
   }
}