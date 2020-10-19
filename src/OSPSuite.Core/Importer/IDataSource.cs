using System.Collections.Generic;
using OSPSuite.Core.Importer.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Importer;

namespace OSPSuite.Core.Importer
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
      Cache<string, IDataSet> DataSets { get; }
      IEnumerable<string> NamesFromConvention();
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
      }

      public void SetMappings(string fileName, IEnumerable<MetaDataMappingConverter> mappings)
      {
         _configuration.FileName = fileName;
         _mappings = mappings;
      }

      private IEnumerable<MetaDataMappingConverter> _mappings;
      public Cache<string, IDataSet> DataSets { get; } = new Cache<string, IDataSet>();
      public IEnumerable<string> NamesFromConvention()
      {
         return _importer.NamesFromConvention(_configuration.NamingConventions, _configuration.FileName, DataSets, _mappings);
      }
   }
}
