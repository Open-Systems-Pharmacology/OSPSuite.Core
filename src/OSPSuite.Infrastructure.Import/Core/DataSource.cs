using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ParseErrors
   {
      private readonly Cache<IDataSet, List<ParseErrorDescription>> _errors =
         new Cache<IDataSet, List<ParseErrorDescription>>(onMissingKey: _ => new List<ParseErrorDescription>());

      public bool Any() => _errors.Any();

      public bool Contains(IDataSet key) => _errors.Contains(key);

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
      ImporterConfiguration GetImporterConfiguration();
      IEnumerable<MetaDataMappingConverter> GetMappings();
      Cache<string, IDataSet> DataSets { get; }
      IEnumerable<string> NamesFromConvention();
      NanSettings NanSettings { get; set; }
      ImportedDataSet ImportedDataSetAt(int index);
      IDataSet DataSetAt(int index);
      ParseErrors ValidateDataSourceUnits(ColumnInfoCache columnInfos);
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

         return errors;
      }

      public void SetMappings(string fileName, IReadOnlyList<MetaDataMappingConverter> mappings)
      {
         _configuration.FileName = fileName;
         _mappings = mappings;
      }

      public ImporterConfiguration GetImporterConfiguration()
      {
         return _configuration;
      }

      public IEnumerable<MetaDataMappingConverter> GetMappings()
      {
         return _mappings;
      }

      public IEnumerable<string> NamesFromConvention()
      {
         return _importer.NamesFromConvention(_configuration.NamingConventions, _configuration.FileName, DataSets, _mappings);
      }

      public IDataSet DataSetAt(int index)
      {
         var sheetIndex = 0;
         var sheet = DataSets.GetEnumerator();
         var accumulatedIndexes = 0;
         while (sheet.MoveNext() && index >= 0)
         {
            var countOnSheet = sheet.Current.Data.Count();
            if (countOnSheet > index)
            {
               return sheet.Current;
            }

            index -= countOnSheet;
            sheetIndex++;
            accumulatedIndexes += countOnSheet;
         }

         return null;
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