using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   /// Collection of DataSets
   /// </summary>
   public interface IDataSource
   {
      void SetDataFormat(IDataFormat dataFormat);
      void SetNamingConvention(string namingConvention);
      void AddSheets(Cache<string, DataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, string filter);
      void SetMappings(string fileName, IEnumerable<MetaDataMappingConverter> mappings);
      ImporterConfiguration GetImporterConfiguration();
      IEnumerable<MetaDataMappingConverter> GetMappings();
      Cache<string, IDataSet> DataSets { get; }
      IEnumerable<string> NamesFromConvention();
      NanSettings NanSettings { get; set; }
      ImportedDataSet DataSetAt(int index);
      void ValidateDataSourceUnits(IReadOnlyList<ColumnInfo> columnInfos);
   }

   public class DataSource : IDataSource
   {
      private IImporter _importer;
      private ImporterConfiguration _configuration;
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

      public void AddSheets(Cache<string, DataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, string filter)
      {         
         _importer.AddFromFile(_configuration.Format, filterSheets(dataSheets, filter), columnInfos, this);
         if (NanSettings == null || !double.TryParse(NanSettings.Indicator, out var indicator))
            indicator = double.NaN;
         foreach (var dataSet in DataSets.KeyValues)
         {
            if (NanSettings != null && NanSettings.Action == NanSettings.ActionType.Throw)
            {
               dataSet.Value.ThrowsOnNan(indicator);
            }
            else
            {
               dataSet.Value.ClearNan(indicator);
               var emptyDataSets = dataSet.Value.Data.Where(parsedDataSet => parsedDataSet.Data.All(column => column.Value.Count == 0)).ToList();
               if (emptyDataSets.Count == 0)
                  continue;

               var emptyDataSetsNames = emptyDataSets.Select(d => string.Join(".", d.Description.Where(metaData => metaData.Value != null).Select(metaData => metaData.Value)));
               throw new EmptyDataSetsException(emptyDataSetsNames);
            }
         }
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

      public ImportedDataSet DataSetAt(int index)
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
      private void validateErrorAgainstMeasurement(IReadOnlyList<ColumnInfo> columnInfos)
      {
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

                     if (errorColumn.Key == null)
                        continue;

                     if (errorColumn.Value != null && measurementColumn.Value.Count != errorColumn.Value.Count)
                        throw new OSPSuiteException(Error.MismatchingArrayLengths);

                     var errorDimension = errorColumn.Key.Column.Dimension;
                     var measurementDimension = measurementColumn.Key.Column.Dimension;

                     if (errorDimension == null)
                     {
                        for (var i = 0; i < measurementColumn.Value.Count(); i++)
                        {
                           if (double.IsNaN(errorColumn.Value.ElementAt(i).Measurement))
                              continue;

                           if (column.SupportedDimensions.FirstOrDefault(x => x.HasUnit(measurementColumn.Value.ElementAt(i).Unit)) !=
                               column.SupportedDimensions.FirstOrDefault(x => x.HasUnit(errorColumn.Value.ElementAt(i).Unit)))
                              throw new ErrorUnitException();
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
                           throw new ErrorUnitException();
                     }
                  }
               }
            }
         }
      }
      //checks that all units coming from a mapped column unit belong to a valid dimension for this mapping
      //and also that they are all of the same dimension within every data set. 
      private void validateUnitsSupportedAndSameDimension(IReadOnlyList<ColumnInfo> columnInfos)
      {
         foreach (var columnInfo in columnInfos)
         {
            foreach (var dataSet in DataSets)
            {
               foreach (var set in dataSet.Data)
               {
                  var column = set.Data.FirstOrDefault(x => x.Key.ColumnInfo.Name == columnInfo.Name);

                  if (column.Key == null)
                        continue;

                  //if unit comes from a column
                  if (column.Key.Column.Dimension == null)
                  {
                     var dimensionOfFirstUnit = columnInfo.SupportedDimensions.FirstOrDefault(x => x.HasUnit(column.Value.ElementAt(0).Unit));

                     for (var i = 0; i < column.Value.Count(); i++)
                     {
                        if (double.IsNaN(column.Value.ElementAt(i).Measurement))
                           continue;

                        //if the unit specified does not belong to one of the supported dimensions of the mapping
                        if (!columnInfo.SupportedDimensions.Any(x => x.HasUnit(column.Value.ElementAt(i).Unit)))
                           throw new InvalidDimensionException(column.Value.ElementAt(i).Unit, columnInfo.DisplayName);

                        //if the unit specified is not of the same dimension as the other units of the same data set
                        if (columnInfo.SupportedDimensions.First(x => x.HasUnit(column.Value.ElementAt(i).Unit)) != dimensionOfFirstUnit)
                           throw new InconsistentDimensionBetweenUnitsException(columnInfo.DisplayName);
                     }
                  }
               }
            }
         }
      }

      void IDataSource.ValidateDataSourceUnits(IReadOnlyList<ColumnInfo> columnInfos)
      {
         validateUnitsSupportedAndSameDimension(columnInfos);
         validateErrorAgainstMeasurement(columnInfos);
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
