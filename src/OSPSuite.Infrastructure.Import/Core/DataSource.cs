using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ParseErrors
   {
      private Cache<IDataSet, List<ParseErrorDescription>> _errors = new Cache<IDataSet, List<ParseErrorDescription>>(onMissingKey: _ => new List<ParseErrorDescription>());

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
   /// Collection of DataSets
   /// </summary>
   public interface IDataSource
   {
      void SetDataFormat(IDataFormat dataFormat);
      void SetNamingConvention(string namingConvention);
      ParseErrors AddSheets(Cache<string, DataSheet> dataSheets, Cache<string, ColumnInfo> columnInfos, string filter);
      void SetMappings(string fileName, IEnumerable<MetaDataMappingConverter> mappings);
      ImporterConfiguration GetImporterConfiguration();
      IEnumerable<MetaDataMappingConverter> GetMappings();
      Cache<string, IDataSet> DataSets { get; }
      IEnumerable<string> NamesFromConvention();
      NanSettings NanSettings { get; set; }
      ImportedDataSet ImportedDataSetAt(int index);
      IDataSet DataSetAt(int index);
      ParseErrors ValidateDataSourceUnits(Cache<string, ColumnInfo> columnInfos);
   }

   public class DataSource : IDataSource
   {
      private readonly IImporter _importer;
      private readonly ImporterConfiguration _configuration;
      private IEnumerable<MetaDataMappingConverter> _mappings;
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

      private Cache<string, DataSheet> filterSheets(Cache<string, DataSheet> dataSheets, string filter)
      {
         Cache<string, DataSheet> filteredDataSheets = new Cache<string, DataSheet>();
         foreach (var key in dataSheets.Keys)
         {
            var dt = dataSheets[key].RawData.AsDataTable();
            var dv = new DataView(dt);
            dv.RowFilter = filter;
            var list = new List<DataRow>();
            var ds = new DataSheet() { RawData = new UnformattedData(dataSheets[key].RawData) };
            foreach (DataRowView drv in dv)
            {
               ds.RawData.AddRow(drv.Row.ItemArray.Select(c => c.ToString()));
            }
            filteredDataSheets.Add(key, ds);
         }

         return filteredDataSheets;
      }

      public ParseErrors AddSheets(Cache<string, DataSheet> dataSheets, Cache<string, ColumnInfo> columnInfos, string filter)
      {
         _importer.AddFromFile(_configuration.Format, filterSheets(dataSheets, filter), columnInfos, this);
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

               var emptyDataSetsNames = emptyDataSets.Select(d => string.Join(".", d.Description.Where(metaData => metaData.Value != null).Select(metaData => metaData.Value)));
               errors.Add(dataSet.Value, new EmptyDataSetsParseErrorDescription(emptyDataSetsNames));
            }
         }
         return errors;
      }

      public void SetMappings(string fileName, IEnumerable<MetaDataMappingConverter> mappings)
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

      //checks that the dimension of all the units coming from columns for error have the same dimension to the corresponding measurement
      private ParseErrors validateErrorAgainstMeasurement(Cache<string, ColumnInfo> columnInfos)
      {
         var errors = new ParseErrors();
         foreach (var column in columnInfos.Where(c => !c.IsAuxiliary()))
         {
            foreach (var relatedColumn in columnInfos.Where(c => c.IsAuxiliary() && c.RelatedColumnOf == column.Name))
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
                        for (var i = 0; i < measurementColumn.Value.Count(); i++)
                        {
                           if (double.IsNaN(errorColumn.Value.ElementAt(i).Measurement))
                              continue;

                           var measurementSupportedDimension = column.SupportedDimensions.FirstOrDefault(x => x.HasUnit(measurementColumn.Value.ElementAt(i).Unit));
                           var errorSupportedDimension = column.SupportedDimensions.FirstOrDefault(x => x.HasUnit(errorColumn.Value.ElementAt(i).Unit));
                           if (measurementSupportedDimension != errorSupportedDimension)
                           {
                              errors.Add(dataSet, new ErrorUnitParseErrorDescription());
                              continue;
                           }
                        }
                     }
                     else
                     {
                        //if the dimension of the error is dimensionless (fe for geometric standard deviation)
                        //it is OK for it not to be of the smae dimension as the dimension of the measurement
                        if (errorDimension == Constants.Dimension.NO_DIMENSION
                            || errorDimension.Name == Constants.Dimension.FRACTION)
                           continue;

                        if (measurementDimension != errorDimension)
                        {
                           errors.Add(dataSet, new ErrorUnitParseErrorDescription());
                           continue;
                        }
                     }
                  }
               }
            }
         }
         return errors;
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
                  var column = set.Data.FirstOrDefault(x => x.Key.ColumnInfo.Name == columnInfo.Name);

                  if (column.Key == null || column.Key.ErrorDeviation == Constants.STD_DEV_GEOMETRIC)
                        continue;

                  //if unit comes from a column
                  if (column.Key.Column.Dimension == null)
                  {
                     var firstNonEmptyUnit = column.Value.FirstOrDefault(x => !string.IsNullOrEmpty(x.Unit));
                     
                     var dimensionOfFirstUnit = columnInfo.SupportedDimensions.FirstOrDefault(x => x.FindUnit(firstNonEmptyUnit.Unit, ignoreCase: true) != null);

                     for (var i = 0; i < column.Value.Count(); i++)
                     {
                        var currentValue = column.Value.ElementAt(i);
                        if (double.IsNaN(currentValue.Measurement))
                           continue;

                        var dimension = columnInfo.SupportedDimensions.FirstOrDefault(x => x.FindUnit(currentValue.Unit, ignoreCase: true) != null);

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
                           continue;
                        }
                     }
                  }
               }
            }
         }
         return errors;
      }

      public ParseErrors ValidateDataSourceUnits(Cache<string, ColumnInfo> columnInfos)
      {
         var errors = validateUnitsSupportedAndSameDimension(columnInfos);
         errors.Add(validateErrorAgainstMeasurement(columnInfos));
         return errors;
      }
   }

   public class MetaDataInstance
   {
      public string Name { get; private set; }
      public string Value { get; private set; }

      public MetaDataInstance(string name, string value)
      {
         Name = name;
         Value = value;
      }
   }

   public class ImportedDataSet
   {
      public string FileName { get; private set; }
      public string SheetName { get; private set; }
      public ParsedDataSet ParsedDataSet { get; private set; }
      public string Name { get; private set; }
      public IReadOnlyList<MetaDataInstance> MetaDataDescription { get; private set; }

      public ImportedDataSet(string fileName, string sheetName, ParsedDataSet parsedDataSet, string name, IReadOnlyList<MetaDataInstance> metaDataDescription)
      {
         FileName = fileName;
         SheetName = sheetName;
         ParsedDataSet = parsedDataSet;
         Name = name;
         MetaDataDescription = metaDataDescription;
      }
   }
}
