using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ParseErrors
   {
      private readonly Cache<IDataSet, List<ParseErrorDescription>> _errors =
         new Cache<IDataSet, List<ParseErrorDescription>>(onMissingKey: _ => new List<ParseErrorDescription>());

      public bool Any() => _errors.Any();

      public IEnumerable<ParseErrorDescription> ErrorsFor(IDataSet key) => _errors[key];

      public void Add(IDataSet key, ParseErrorDescription x)
      {
         Add(key, new List<ParseErrorDescription>() { x });
      }

      public void Add(ParseErrors other)
      {
         foreach (var x in other._errors.KeyValues)
         {
            Add(x.Key, x.Value);
         }
      }

      public void Add(IDataSet key, IEnumerable<ParseErrorDescription> list)
      {
         if (_errors.Contains(key))
            _errors[key].AddRange(list);
         else
            _errors.Add(key, new List<ParseErrorDescription>(list));
      }
   }

   /// <summary>
   ///    Collection of DataSets
   /// </summary>
   public interface IDataSource
   {
      void SetDataFormat(IDataFormat dataFormat);
      void SetNamingConvention(string namingConvention);
      ParseErrors AddSheets(DataSheetCollection dataSheets, ColumnInfoCache columnInfos, string filter);
      void SetMappings(string fileName, IReadOnlyList<MetaDataMappingConverter> mappings);
      IEnumerable<MetaDataMappingConverter> GetMappings();
      Cache<string, IDataSet> DataSets { get; }
      IEnumerable<string> NamesFromConvention();
      NanSettings NanSettings { get; set; }
      ImportedDataSet ImportedDataSetAt(int index);
      ParseErrors ValidateDataSourceUnits(ColumnInfoCache columnInfos);

      /// <summary>
      ///    Validates the meta data values of all data sets, e.g. that a subject count is a whole number of at least one
      /// </summary>
      ParseErrors ValidateMetaData();
   }

   public class DataSource : IDataSource
   {
      private readonly IImporter _importer;
      private readonly ImporterConfiguration _configuration;
      private IReadOnlyList<MetaDataMappingConverter> _mappings;
      public Cache<string, IDataSet> DataSets { get; } = new Cache<string, IDataSet>();

      public DataSource(IImporter importer)
      {
         _importer = importer;
         _configuration = new ImporterConfiguration();
      }

      public void SetDataFormat(IDataFormat dataFormat)
      {
         _configuration.Format = dataFormat;
      }

      public void SetNamingConvention(string namingConvention)
      {
         _configuration.NamingConventions = namingConvention;
      }

      public NanSettings NanSettings { get; set; }

      public ParseErrors AddSheets(DataSheetCollection dataSheets, ColumnInfoCache columnInfos, string filter)
      {
         _importer.AddFromFile(_configuration.Format, dataSheets.Filter(filter), columnInfos, this);

         if (NanSettings == null || !double.TryParse(NanSettings.Indicator, out var indicator))
            indicator = double.NaN;
         var errors = new ParseErrors();
         foreach (var dataSet in DataSets.KeyValues)
         {
            if (NanSettings != null && NanSettings.Action == NanSettings.ActionType.Throw)
            {
               if (dataSet.Value.NanValuesExist(indicator))
                  errors.Add(dataSet.Value, new NaNParseErrorDescription());
            }
            else
            {
               dataSet.Value.ClearNan(indicator);
               var emptyDataSets = dataSet.Value.Data.Where(parsedDataSet => parsedDataSet.Data.All(column => column.Value.Count == 0)).ToList();
               if (emptyDataSets.Count == 0)
                  continue;

               var emptyDataSetsNames = emptyDataSets.Select(d =>
                  string.Join(".", d.Description.Where(metaData => metaData.Value != null).Select(metaData => metaData.Value)));
               errors.Add(dataSet.Value, new EmptyDataSetsParseErrorDescription(emptyDataSetsNames));
            }
         }

         errors.Add(resolveErrorStdDevFromColumn());
         return errors;
      }

      //the error type read from a column is resolved once the rows to ignore are removed, so that they do not decide it
      private ParseErrors resolveErrorStdDevFromColumn()
      {
         var errors = new ParseErrors();
         foreach (var dataSet in DataSets.KeyValues)
         {
            foreach (var parsedDataSet in dataSet.Value.Data)
            {
               var errorColumns = parsedDataSet.Data.Keys.Where(x => x.ColumnInfo.IsAuxiliary && !x.Column.ErrorStdDevColumn.IsNullOrEmpty());
               errorColumns.Each(x => resolveErrorStdDev(x, parsedDataSet, dataSet.Key, dataSet.Value, errors));
               validateArithmeticErrorFromColumnUnits(parsedDataSet, dataSet.Value, errors);
            }
         }

         return errors;
      }

      //the error type must be known and unique within the data set. Otherwise, the error is reported and the default error type is kept
      private void resolveErrorStdDev(ExtendedColumn errorColumn, ParsedDataSet parsedDataSet, string sheetName, IDataSet dataSet, ParseErrors errors)
      {
         var errorStdDevValues = errorColumn.ErrorStdDevValues.Where(x => !x.IsNullOrEmpty()).Distinct().ToList();
         var dataSetName = string.Join(".", parsedDataSet.Description.Where(metaData => metaData.Value != null).Select(metaData => metaData.Value));
         var unknownValues = errorStdDevValues.Where(x => ErrorStdDevValues.ErrorStdDevFor(x) == null).ToList();
         unknownValues.Each(x => errors.Add(dataSet, new UnknownErrorTypeParseErrorDescription(sheetName, dataSetName, x)));
         if (unknownValues.Any())
            return;

         var errorStdDevs = errorStdDevValues.Select(ErrorStdDevValues.ErrorStdDevFor).Distinct().ToList();
         if (errorStdDevs.Count > 1)
         {
            errors.Add(dataSet, new ConflictingErrorTypesParseErrorDescription(sheetName, dataSetName, errorStdDevValues));
            return;
         }

         //without any value, the configured error type is used
         var column = errorColumn.Column;
         var errorStdDev = errorStdDevs.SingleOrDefault() ?? column.ErrorStdDev;
         errorColumn.ErrorDeviation = errorStdDev;
         column.ErrorStdDev = errorStdDev;

         if (errorStdDev == Constants.STD_DEV_GEOMETRIC)
         {
            column.Dimension = Constants.Dimension.NO_DIMENSION;
            column.Unit = new UnitDescription(column.Dimension.BaseUnit.Name);
         }
         //an arithmetic error needs the dimension of its units and not the one of a geometric default
         else if (column.Dimension == Constants.Dimension.NO_DIMENSION)
            column.Dimension = null;
      }

      //an arithmetic error whose type comes from a column may keep the units of a geometric default (no unit)
      //so its units must have the dimension of the measurement units
      private void validateArithmeticErrorFromColumnUnits(ParsedDataSet parsedDataSet, IDataSet dataSet, ParseErrors errors)
      {
         var arithmeticErrorsFromColumn = parsedDataSet.Data.Where(x => !x.Key.Column.ErrorStdDevColumn.IsNullOrEmpty()
                                                                         && x.Key.ErrorDeviation == Constants.STD_DEV_ARITHMETIC);

         foreach (var errorColumn in arithmeticErrorsFromColumn)
         {
            var measurementColumn = parsedDataSet.Data.FirstOrDefault(x => x.Key.ColumnInfo.Name == errorColumn.Key.ColumnInfo.RelatedColumnOf);
            if (measurementColumn.Key == null || measurementColumn.Value.Count != errorColumn.Value.Count)
               continue;

            validateErrorFromColumnDimension(measurementColumn.Value, errorColumn.Value, measurementColumn.Key.ColumnInfo, errors, dataSet);
         }
      }

      public void SetMappings(string fileName, IReadOnlyList<MetaDataMappingConverter> mappings)
      {
         _configuration.FileName = fileName;
         _mappings = mappings;
      }

      public IEnumerable<MetaDataMappingConverter> GetMappings()
      {
         return _mappings;
      }

      public IEnumerable<string> NamesFromConvention()
      {
         return _importer.NamesFromConvention(_configuration.NamingConventions, _configuration.FileName, DataSets, _mappings);
      }

      public ImportedDataSet ImportedDataSetAt(int index)
      {
         var sheetIndex = 0;
         var sheet = DataSets.GetEnumerator();
         var accumulatedIndexes = 0;
         while (sheet.MoveNext() && index >= 0)
         {
            if (sheet.Current.Data.Count() > index)
            {
               var dataSet = sheet.Current.Data.ElementAt(index);
               return new ImportedDataSet(
                  _configuration.FileName,
                  DataSets.Keys.ElementAt(sheetIndex),
                  dataSet,
                  NamesFromConvention().ElementAt(accumulatedIndexes + index),
                  dataSet.EnumerateMetaData(_mappings)
               );
            }

            index -= sheet.Current.Data.Count();
            sheetIndex++;
            accumulatedIndexes += sheet.Current.Data.Count();
         }

         return null;
      }

      //checks that the dimension of all the error units coming from columns to have the same dimension to the corresponding measurement unit
      private ParseErrors validateErrorAgainstMeasurement(ColumnInfoCache columnInfos)
      {
         var errors = new ParseErrors();
         foreach (var column in columnInfos.Where(c => !c.IsAuxiliary))
         {
            foreach (var relatedColumn in columnInfos.RelatedColumnsFrom(column.Name))
            {
               foreach (var dataSet in DataSets)
               {
                  foreach (var set in dataSet.Data)
                  {
                     var measurementColumn = set.Data.FirstOrDefault(x => x.Key.ColumnInfo.Name == column.Name);
                     var errorColumn = set.Data.FirstOrDefault(x => x.Key.ColumnInfo.Name == relatedColumn.Name);

                     if (errorColumn.Key == null || errorColumn.Key.ErrorDeviation == Constants.STD_DEV_GEOMETRIC)
                        continue;

                     if (errorColumn.Value != null && measurementColumn.Value.Count != errorColumn.Value.Count)
                     {
                        errors.Add(dataSet, new MismatchingArrayLengthsParseErrorDescription());
                        continue;
                     }

                     var errorDimension = errorColumn.Key.Column.Dimension;
                     var measurementDimension = measurementColumn.Key.Column.Dimension;

                     if (errorDimension == null)
                     {
                        validateErrorFromColumnDimension(measurementColumn.Value, errorColumn.Value, column, errors, dataSet);
                     }
                     else
                     {
                        validateManuallySetErrorDimension(errorDimension, measurementDimension, errors, dataSet);
                     }
                  }
               }
            }
         }

         return errors;
      }

      private void validateManuallySetErrorDimension(IDimension errorDimension, IDimension measurementDimension, ParseErrors errors,
         IDataSet dataSet)
      {
         //if the dimension of the error is dimensionless (fe for geometric standard deviation)
         //it is OK for it not to be of the same dimension as the dimension of the measurement
         if (errorDimension == Constants.Dimension.NO_DIMENSION
             || errorDimension.Name == Constants.Dimension.FRACTION)
            return;

         if (measurementDimension != errorDimension)
            errors.Add(dataSet, new ErrorUnitParseErrorDescription());
      }

      private void validateErrorFromColumnDimension(IList<SimulationPoint> measurementValues, IList<SimulationPoint> errorValues, ColumnInfo column,
         ParseErrors errors,
         IDataSet dataSet)
      {
         for (var i = 0; i < measurementValues.Count(); i++)
         {
            if (double.IsNaN(errorValues.ElementAt(i).Measurement))
               continue;

            var measurementSupportedDimension = column.DimensionForUnit(measurementValues.ElementAt(i).Unit);
            var errorSupportedDimension = column.DimensionForUnit(errorValues.ElementAt(i).Unit);

            if (measurementSupportedDimension != errorSupportedDimension)
               errors.Add(dataSet, new ErrorUnitParseErrorDescription());
         }
      }

      //checks that all units coming from a mapped column unit belong to a valid dimension for this mapping
      //and also that they are all of the same dimension within every data set. 
      private ParseErrors validateUnitsSupportedAndSameDimension(Cache<string, ColumnInfo> columnInfos)
      {
         var errors = new ParseErrors();
         foreach (var columnInfo in columnInfos)
         {
            foreach (var dataSet in DataSets)
            {
               foreach (var set in dataSet.Data)
               {
                  var columnValuesPair = set.Data.FirstOrDefault(x => x.Key.ColumnInfo.Name == columnInfo.Name);

                  if (columnValuesPair.Key == null || columnValuesPair.Key.ErrorDeviation == Constants.STD_DEV_GEOMETRIC)
                     continue;

                  //if unit comes from a column
                  if (columnValuesPair.Key.Column.Dimension == null)
                     validateUnitComingFromColumnDimension(columnValuesPair.Value, columnInfo, errors, dataSet);
               }
            }
         }

         return errors;
      }

      private void validateUnitComingFromColumnDimension(IList<SimulationPoint> simulationPointsList, ColumnInfo columnInfo, ParseErrors errors,
         IDataSet dataSet)
      {
         var firstValueWithNonEmptyUnit = simulationPointsList.FirstOrDefault(x => !string.IsNullOrEmpty(x.Unit));
         var firstNonEmptyUnit = firstValueWithNonEmptyUnit == null ? "" : firstValueWithNonEmptyUnit.Unit;

         var dimensionOfFirstUnit = columnInfo.DimensionForUnit(firstNonEmptyUnit);

         for (var i = 0; i < simulationPointsList.Count(); i++)
         {
            var currentValue = simulationPointsList.ElementAt(i);
            if (double.IsNaN(currentValue.Measurement))
               continue;

            var currentUnit = currentValue.Unit;
            var dimension = columnInfo.DimensionForUnit(currentUnit);

            //if the unit specified does not belong to one of the supported dimensions of the mapping
            if (dimension == null)
            {
               errors.Add(dataSet, new InvalidDimensionParseErrorDescription(currentValue.Unit, columnInfo.DisplayName));
               continue;
            }

            //if the unit specified is not of the same dimension as the other units of the same data set
            if (dimension != dimensionOfFirstUnit)
            {
               errors.Add(dataSet, new InconsistentDimensionBetweenUnitsParseErrorDescription(columnInfo.DisplayName));
            }
         }
      }

      public ParseErrors ValidateDataSourceUnits(ColumnInfoCache columnInfos)
      {
         var errors = validateUnitsSupportedAndSameDimension(columnInfos);
         errors.Add(validateErrorAgainstMeasurement(columnInfos));
         return errors;
      }

      public ParseErrors ValidateMetaData()
      {
         var errors = new ParseErrors();
         foreach (var dataSet in DataSets)
         {
            var invalidSubjectCounts = dataSet.Data
               .SelectMany(x => x.EnumerateMetaData(_mappings))
               .Where(x => x.Name == Constants.ObservedData.SUBJECT_COUNT)
               .Select(x => x.Value)
               .Where(x => !x.IsNullOrEmpty() && !DataRepositoryExtensions.IsValidSubjectCount(x))
               .Distinct();

            invalidSubjectCounts.Each(x => errors.Add(dataSet, new InvalidSubjectCountParseErrorDescription(x)));
         }

         return errors;
      }
   }

   public class MetaDataInstance
   {
      public string Name { get; }
      public string Value { get; }

      public MetaDataInstance(string name, string value)
      {
         Name = name;
         Value = value;
      }
   }

   public class ImportedDataSet
   {
      public string FileName { get; }
      public string SheetName { get; }
      public ParsedDataSet ParsedDataSet { get; }
      public string Name { get; }
      public IReadOnlyList<MetaDataInstance> MetaDataDescription { get; }

      public ImportedDataSet(string fileName, string sheetName, ParsedDataSet parsedDataSet, string name,
         IReadOnlyList<MetaDataInstance> metaDataDescription)
      {
         FileName = fileName;
         SheetName = sheetName;
         ParsedDataSet = parsedDataSet;
         Name = name;
         MetaDataDescription = metaDataDescription;
      }
   }
}