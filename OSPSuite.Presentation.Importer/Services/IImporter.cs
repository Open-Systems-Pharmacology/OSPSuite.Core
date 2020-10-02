using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.Utils.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Utility.Collections;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName);
      void AddFromFile(IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IDataSource alreadyExisting);
      IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);

      IEnumerable<string> NamesFromConvention
      (
         string namingConvention,
         string fileName,
         Cache<string, IDataSet> dataSets,
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

      public void AddFromFile(IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IDataSource alreadyExisting)
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
            //TODO implement GetOrAdd() as a method for Cache, in order not to be doing this all the time
            IDataSet current;
            if (alreadyExisting.DataSets.Contains(element.Key))
               current = alreadyExisting.DataSets[element.Key];
            else
            {
               current = new DataSet();
               alreadyExisting.DataSets.Add(current);
            }
            current.Data = current.Data.Union(element.Value.Data).ToDictionary(s => s.Key, s => s.Value);
         }
      }

      public IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName)
      {
         //var filename = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA, fileName);
         //in the presenter : if string == "" (Cancel clicked), then do not try to parse


         var dataSource = _parser.For(fileName);
         dataSource.AvailableFormats = AvailableFormats(dataSource.DataSheets.ElementAt(0).RawData, columnInfos).ToList();
         dataSource.Format = dataSource.AvailableFormats.FirstOrDefault();
         //TODO: check that all sheets are supporting the formats...
         
         return dataSource;
      }

      public IEnumerable<string> NamesFromConvention
      (
         string namingConvention,
         string fileName,
         Cache<string, IDataSet> dataSets,
         IEnumerable<MetaDataMappingConverter> mappings
      )
      {
         fileName = Path.GetFileNameWithoutExtension(fileName);
         var counters = new Dictionary<string, int>();
         // Iterate over the list of datasets to generate a unique name for each one based on the naming convention
         return dataSets.KeyValues.SelectMany(ds => ds.Value.Data.Select(s =>
         {
            // Obtain the key first before checking if it already is taken by any other dataset
            var key = mappings.Aggregate
            (
               // Start with the namingConvention replacing {file} and {sheet} by their names
               new MetaDataMappingConverter()
               {
                  Id = namingConvention.Replace($"{{{Constants.FILE}}}", fileName).Replace($"{{{Constants.SHEET}}}", ds.Key)
               },
               // Aggregates then by iterating on mappings (which contains all valid keys, e.g. metadata) and replacing any text
               // {id} with id being the id of the current key by the value stored for such a key when the data was parsed
               (acc, x) =>
                  new MetaDataMappingConverter()
                  {
                     Id = acc.Id.Replace($"{{{x.Id}}}", $"{s.Key.FirstOrDefault(md => md.Id == x.Index(ds.Key))?.Value}")
                  }
            ).Id;
            var counter = counters.GetOrAdd(key, (_) => 0);
            counters[key]++;
            // Only add a number (for making it unique) to the name if the key already existed in the counters
            return key + (counter > 0 ? $"_{counter}" : "");
         })).ToList();
      }
   }
}