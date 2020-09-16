using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName);
      IDataSource ImportFromFile(IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos);
      IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);

      IEnumerable<string> NamesFromConvention
      (
         string namingConvention,
         string fileName,
         IReadOnlyDictionary<string, IDataSet> dataSets,
         IEnumerable<MetaDataMappingConverter> mappings
      );
   }

   public class Importer : IImporter
   {
      private readonly IoC _container;
      private readonly IDataSourceFileParser _parser;

      public Importer( IoC container, IDataSourceFileParser parser)
      {
         _container = container;
         _parser = parser;
      }

      public IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         return _container.ResolveAll<IDataFormat>()
            .Where(x => x.SetParameters(data, columnInfos));
      }

      public IDataSource ImportFromFile(IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos)
      {
         var dataSets =
            dataSheets
               .ToDictionary
               (
                  s => s.Key,
                  s =>
                     new DataSet()
                     {
                        Data = format.Parse(s.Value.RawData, columnInfos)
                     } as IDataSet
               );
         return new DataSource() 
         {
            DataSets = dataSets
         };
      }

      public IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName)
      {
         //var filename = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA, fileName);
         //in the presenter : if string == "" (Cancel clicked), then do not try to parse


         var dataSource = _parser.For(fileName);
         dataSource.AvailableFormats = AvailableFormats(dataSource.DataSheets.ElementAt(0).Value.RawData, columnInfos).ToList();
         dataSource.Format = dataSource.AvailableFormats.FirstOrDefault();
         //TODO: check that all sheets are supporting the formats...
         
         return dataSource;
      }

      public IEnumerable<string> NamesFromConvention
      (
         string namingConvention,
         string fileName,
         IReadOnlyDictionary<string, IDataSet> dataSets,
         IEnumerable<MetaDataMappingConverter> mappings
      )
      {
         return dataSets.SelectMany(ds => ds.Value.Data.Select(s =>
            mappings.Aggregate
            (
               new MetaDataMappingConverter()
               {
                  Id = namingConvention.Replace("{File}", fileName).Replace("{Sheet}", ds.Key)
               },
               (acc, x) => new MetaDataMappingConverter()
               {
                  Id = acc.Id.Replace($"{{{x.Id}}}", $"{s.Key.FirstOrDefault(md => md.Id == x.Index(ds.Key))?.Value}")
               }
            ).Id
         ));
      }
   }
}