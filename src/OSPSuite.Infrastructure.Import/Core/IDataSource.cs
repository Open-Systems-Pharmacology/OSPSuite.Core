using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   /// Collection of DataSets
   /// </summary>
   public interface IDataSource
   {
      void SetDataFormat( IDataFormat dataFormat);
      void SetNamingConvention(string namingConvention);
      void AddSheets( Cache<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos);
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
         _configuration =  new ImporterConfiguration();
      }

      public void SetDataFormat(IDataFormat dataFormat)
      {
         _configuration.Format = dataFormat;
      }

      public void SetNamingConvention(string namingConvention)
      {
         _configuration.NamingConventions = namingConvention;
      }

      public void AddSheets(Cache<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos)
      {
         _importer.AddFromFile(_configuration.Format, dataSheets, columnInfos, this);
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
