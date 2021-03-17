using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;

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
         foreach (var dataSet in DataSets)
         {
            if (NanSettings != null && NanSettings.Action == NanSettings.ActionType.Throw)
               dataSet.ThrowsOnNan(indicator);
            else
               dataSet.ClearNan(indicator);
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
            else
            {
               index -= sheet.Current.Data.Count();
               sheetIndex++;
               accumulatedIndexes += sheet.Current.Data.Count();
            }
         }

         return null;
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
