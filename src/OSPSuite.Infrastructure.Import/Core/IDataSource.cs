using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
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
      void AddSheets(Cache<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, string filter);
      void SetMappings(string fileName, IEnumerable<MetaDataMappingConverter> mappings);
      ImporterConfiguration GetImporterConfiguration();
      IEnumerable<MetaDataMappingConverter> GetMappings();
      Cache<string, IDataSet> DataSets { get; }
      IEnumerable<string> NamesFromConvention();
      NanSettings NanSettings { get; set; }
   }

   public class DataSource : IDataSource
   {
      private IImporter _importer;
      private ImporterConfiguration _configuration;

      public DataSource(IImporter importer)
      {
         _importer = importer;
         _configuration = new ImporterConfiguration();

         var type = typeof(DynamicQueryable).Assembly.GetType("System.Linq.Dynamic.ExpressionParser");

         FieldInfo field = type.GetField("predefinedTypes", BindingFlags.Static | BindingFlags.NonPublic);

         Type[] predefinedTypes = (Type[])field.GetValue(null);

         Array.Resize(ref predefinedTypes, predefinedTypes.Length + 1);
         predefinedTypes[predefinedTypes.Length - 1] = typeof(IEnumerable<string>); // Your type

         field.SetValue(null, predefinedTypes);
      }

      public void SetDataFormat(IDataFormat dataFormat)
      {
         _configuration.Format = dataFormat;
      }

      public void SetNamingConvention(string namingConvention)
      {
         _configuration.NamingConventions = namingConvention;
      }

      private Cache<string, IDataSheet> filterSheets(Cache<string, IDataSheet> dataSheets, string filter)
      {
         Cache<string, IDataSheet> filteredDataSheets = new Cache<string, IDataSheet>();
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

      public void AddSheets(Cache<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, string filter)
      {         
         _importer.AddFromFile(_configuration.Format, filterSheets(dataSheets, filter), columnInfos, this);
         if (!double.TryParse(NanSettings.Indicator, out var indicator))
            indicator = double.NaN;
         foreach (var dataSet in DataSets)
         {
            if (NanSettings.Action == NanSettings.ActionType.Throw)
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

      private IEnumerable<MetaDataMappingConverter> _mappings;
      public Cache<string, IDataSet> DataSets { get; } = new Cache<string, IDataSet>();
      public IEnumerable<string> NamesFromConvention()
      {
         return _importer.NamesFromConvention(_configuration.NamingConventions, _configuration.FileName, DataSets, _mappings);
      }
      public NanSettings NanSettings { get; set; }
   }
}
