using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.Utils.Extensions;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName);
      void AddFromFile(IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, DataSource alreadyExisiting);
      IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);

      IEnumerable<string> NamesFromConvention
      (
         string namingConvention,
         string fileName,
         IDictionary<string, IDataSet> dataSets,
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

      public void AddFromFile(IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, DataSource alreadyExisiting)
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
         foreach (var element in dataSets)
         {
            var current = alreadyExisiting.DataSets.GetOrAdd(element.Key, _ => new DataSet());
            current.Data = current.Data.Union(element.Value.Data).ToDictionary(s => s.Key, s => s.Value);
         }
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
         IDictionary<string, IDataSet> dataSets,
         IEnumerable<MetaDataMappingConverter> mappings
      )
      {
         fileName = Path.GetFileNameWithoutExtension(fileName);
         var counters = new Dictionary<string, int>();
         return dataSets.SelectMany(ds => ds.Value.Data.Select(s =>
         {
            var key = mappings.Aggregate
            (
               new MetaDataMappingConverter()
               {
                  Id = namingConvention.Replace("{File}", fileName).Replace("{Sheet}", ds.Key)
               },
               (acc, x) =>
                  new MetaDataMappingConverter()
                  {
                     Id = acc.Id.Replace($"{{{x.Id}}}", $"{s.Key.FirstOrDefault(md => md.Id == x.Index(ds.Key))?.Value}")
                  }
            ).Id;
            var counter = counters.GetOrAdd(key, (_) => 0);
            counters[key]++;
            return key + (counter > 0 ? $"_{counter}" : "");
         })).ToList();
      }
   }
}