using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
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

      public double SetParameters(IDataSheet rawDataSheet, ColumnInfoCache columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         if (NotCompatible(rawDataSheet, columnInfos))
            return 0;

         return 1 + setParameters(rawDataSheet, columnInfos, metaDataCategories);
      }

      protected bool NotCompatible(IDataSheet dataSheet, ColumnInfoCache columnInfos)
      {
         return (dataSheet.GetHeaders()
            .Select(dataSheet.GetColumnDescription)
            .Count(header => header.Level == ColumnDescription.MeasurementLevel.Numeric)) < columnInfos.Count(ci => ci.IsMandatory);
      }

      private double setParameters(IDataSheet dataSheet, ColumnInfoCache columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories)
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
         foreach (var parameter in Parameters.OfType<MappingDataFormatParameter>())
         {
            var mappedColumn = parameter.MappedColumn;

            if (mappedColumn?.Unit == null || mappedColumn?.Dimension != null)
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
               var supportedDimensions = concreteColumnInfo.SupportedDimensions;
               var dimensionForUnit = supportedDimensions.FirstOrDefault(x => x.HasUnit(mappedColumn.Unit.SelectedUnit));

               if (dimensionForUnit == null)
                  mappedColumn.Unit = new UnitDescription(UnitDescription.InvalidUnit);
               else
                  mappedColumn.Dimension = dimensionForUnit;
            }
         }
      }

      private void setSecondaryColumnUnit(ColumnInfoCache columnInfos)
      {
         var mappings = Parameters.OfType<MappingDataFormatParameter>();
         foreach (var column in columnInfos.Where(c => !c.IsAuxiliary))
         {
            foreach (var relatedColumn in columnInfos.RelatedColumnsFrom(column.Name))
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

      protected abstract string ExtractLloq(string description, IDataSheet dataSheet, List<string> keys, ref double rank);

      protected abstract UnitDescription ExtractUnits(string description, IDataSheet dataSheet, List<string> keys, IReadOnlyList<IDimension> supportedDimensions, ref double rank);

      public UnitDescription ExtractUnitDescriptions(string description, IReadOnlyList<IDimension> supportedDimensions)
      {
         var rank = 0.0;
         return ExtractUnits(description, dataSheet: null, keys: null, supportedDimensions, ref rank);
      }

      protected virtual void ExtractQualifiedHeadings(List<string> keys, List<string> missingKeys, Cache<string, ColumnInfo> columnInfos, IDataSheet dataSheet, ref double rank)
      {
         foreach (var header in columnInfos)
         {
            var headerName = header.DisplayName;
            var headerKey = keys.FindHeader(headerName);
            if (headerKey != null)
            {
               keys.Remove(headerKey);
               var units = ExtractUnits(headerKey, dataSheet, keys, header.SupportedDimensions, ref rank);

               var col = new Column()
               {
                  Name = headerName,
                  Unit = units,
                  LloqColumn = ExtractLloq(headerKey, dataSheet, keys, ref rank)
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

     protected string ValidateUnit(string unit, IReadOnlyList<IDimension> supportedDimensions)
      {
         return supportedDimensions.Any(x => x.HasUnit(unit)) ? unit : UnitDescription.InvalidUnit;
      }

      protected virtual void ExtractNonQualifiedHeadings(List<string> keys, List<string> missingKeys, Cache<string, ColumnInfo> columnInfos, IDataSheet dataSheet, ref double rank)
      {
         foreach (var header in missingKeys)
         {
            var headerKey = keys.FirstOrDefault
            (h =>
               dataSheet.GetColumnDescription(h).Level == ColumnDescription.MeasurementLevel.Numeric &&
               Parameters
                  .OfType<MappingDataFormatParameter>()
                  .All(m => m.ColumnName != h)
            );
            if (headerKey == null) continue;
            keys.Remove(headerKey);
            var units = ExtractUnits(headerKey, dataSheet, keys, columnInfos[header].SupportedDimensions, ref rank);

            var col = new Column()
            {
               Name = header,
               Unit = units,
               LloqColumn = ExtractLloq(headerKey, dataSheet, keys, ref rank)
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

      protected virtual void ExtractGeneralParameters(List<string> keys, IDataSheet dataSheet, IReadOnlyList<MetaDataCategory> metaDataCategories, ref double rank)
      {
         var columnsCopy = keys.ToList();
         foreach (var header in columnsCopy.Where(h => metaDataCategories.Select(c => c.Name).FindHeader(h) != null))
         {
            keys.Remove(header);
            Parameters.Add(new MetaDataFormatParameter(header, metaDataCategories.Select(c => c.Name).FindHeader(header)));
         }
      }

      public IEnumerable<ParsedDataSet> Parse(IDataSheet dataSheet, ColumnInfoCache columnInfos)
      {
         var missingColumns = Parameters.Where(p => p.ComesFromColumn() && dataSheet.GetColumnDescription(p.ColumnName) == null).Select(p => p.ColumnName).ToList();
         if (missingColumns.Any())
            throw new MissingColumnException(missingColumns);

         var groupingCriteria =
            Parameters
               .Where(p => p.IsGroupingCriterion())
               .Select(p => p.ColumnName);

         return buildDataSets(dataSheet, groupingCriteria, columnInfos);
      }

      private string rowId(IEnumerable<string> parameters, IDataSheet dataSheet, UnformattedRow row)
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

      private IEnumerable<ParsedDataSet> buildDataSets(IDataSheet data, IEnumerable<string> groupingParameters, Cache<string, ColumnInfo> columnInfos)
      {
         var dataSets = new List<ParsedDataSet>();
         var cachedUnformattedRows = new Cache<string, List<UnformattedRow>>();
         foreach(var row in dataSheet.GetRows(_ => true))
         {
            var id = rowId(groupingParameters, dataSheet, row);
            if (!cachedUnformattedRows.Contains(id))
               cachedUnformattedRows.Add(id, new List<UnformattedRow>());
            cachedUnformattedRows[id].Add(row);
         }
         return cachedUnformattedRows.Select(rows => new ParsedDataSet(groupingParameters, dataSheet, rows, parseMappings(rows, dataSheet, columnInfos)));
      }

      private Dictionary<ExtendedColumn, IList<SimulationPoint>> parseMappings(IEnumerable<UnformattedRow> rawDataSet, IDataSheet dataSheet,
         Cache<string, ColumnInfo> columnInfos)
      {
         var dictionary = new Dictionary<ExtendedColumn, IList<SimulationPoint>>();

         //Add time mapping
         var mappingParameters = Parameters.OfType<MappingDataFormatParameter>().ToList();

         var dataSet = rawDataSet.ToList();
         foreach (var columnInfo in columnInfos)
         {
            var currentParameter = mappingParameters.FirstOrDefault(p => p.MappedColumn.Name == columnInfo.DisplayName);
            if (currentParameter == null) continue;
            Func<MappingDataFormatParameter, IDataSheet, UnformattedRow, SimulationPoint> mappingsParser =
               currentParameter.MappedColumn.LloqColumn == null ? (Func<MappingDataFormatParameter, IDataSheet, UnformattedRow, SimulationPoint>) parseMappingOnSameColumn : parseMappingOnSameGivenColumn;

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

      private SimulationPoint parseMappingOnSameColumn(MappingDataFormatParameter currentParameter, IDataSheet dataSheet, UnformattedRow row)
      {
         var columnDescription = dataSheet.GetColumnDescription(currentParameter.ColumnName);
         var element = row.Data.ElementAt(columnDescription.Index).Trim();

         if (!currentParameter.MappedColumn.Unit.ColumnName.IsNullOrEmpty())
         {
            ColumnDescription unitColumnDescription = null;
            if (!string.IsNullOrEmpty(currentParameter.MappedColumn.Unit.ColumnName))
               unitColumnDescription = dataSheet.GetColumnDescription(currentParameter.MappedColumn.Unit.ColumnName);
            if (unitColumnDescription == null)
               throw new MissingColumnException(currentParameter.MappedColumn.Unit.ColumnName);
         }

         var unit = currentParameter.MappedColumn.Unit.ExtractUnit(columnName => dataSheet.GetColumnDescription(columnName).Index, row.Data);

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

      private SimulationPoint parseMappingOnSameGivenColumn(MappingDataFormatParameter currentParameter, IDataSheet dataSheet, UnformattedRow row)
      {
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