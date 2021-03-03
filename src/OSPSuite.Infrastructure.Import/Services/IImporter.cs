using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics;
using OSPSuite.Assets;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Utility.Collections;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName, IReadOnlyList<MetaDataCategory> metaDataCategories);
      void AddFromFile(IDataFormat format, Cache<string, DataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IDataSource alreadyExisting);
      IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories);
      IEnumerable<string> NamesFromConvention
      (
         string namingConvention,
         string fileName,
         Cache<string, IDataSet> dataSets,
         IEnumerable<MetaDataMappingConverter> mappings
      );
      int GetImageIndex(DataFormatParameter parameter);
      MappingProblem CheckWhetherAllDataColumnsAreMapped(IReadOnlyList<ColumnInfo> dataColumns, IEnumerable<DataFormatParameter> mappings);

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

      public IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         return _container.ResolveAll<IDataFormat>()
            .Select(x => (x, x.SetParameters(data, columnInfos, metaDataCategories)))
            .Where(p => p.Item2 > 0)
            .OrderByDescending(p => p.Item2)
            .Select(p => p.x);
      }

      public void AddFromFile(IDataFormat format, Cache<string, DataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IDataSource alreadyExisting)
      {
         var dataSets = new Cache<string, IDataSet>();

         foreach (var s in dataSheets.KeyValues)
         {
            dataSets.Add(s.Key, new DataSet()
            {
               Data = format.Parse(s.Value.RawData, columnInfos)
            });
         }

         foreach (var key in dataSets.Keys)
         {
            IDataSet current;
            if (alreadyExisting.DataSets.Contains(key))
               current = alreadyExisting.DataSets[key];
            else
            {
               current = new DataSet();
               alreadyExisting.DataSets.Add(key, current);
            }
            current.Data = current.Data.Union(dataSets[key].Data);
         }
      }

      public IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         //var filename = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA, fileName);
         //in the presenter : if string == "" (Cancel clicked), then do not try to parse


         var dataSource = _parser.For(fileName);
         dataSource.AvailableFormats = AvailableFormats(dataSource.DataSheets.ElementAt(0).RawData, columnInfos, metaDataCategories).ToList();
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
            var key = s.NameFromConvention(mappings, namingConvention, fileName, ds.Key);
            int counter;
            if (counters.ContainsKey(key))
            {
               counter = counters[key];
            }
            else
            {
               counters.Add(key, 0);
               counter = 0;
            }
            counters[key]++;
            // Only add a number (for making it unique) to the name if the key already existed in the counters
            return key + (counter > 0 ? $"_{counter}" : "");
         })).ToList();
      }

      public int GetImageIndex(DataFormatParameter parameter)
      {
         switch (parameter)
         {
            case MetaDataFormatParameter mp:
               return ApplicationIcons.IconIndex(ApplicationIcons.MetaData);
            case MappingDataFormatParameter mp:
               return ApplicationIcons.IconIndex(ApplicationIcons.UnitInformation);
            case GroupByDataFormatParameter gp:
               return ApplicationIcons.IconIndex(ApplicationIcons.GroupBy);
            default:
               throw new Exception($"{parameter.GetType()} is not currently been handled");
         }
      }

      public MappingProblem CheckWhetherAllDataColumnsAreMapped(IReadOnlyList<ColumnInfo> dataColumns, IEnumerable<DataFormatParameter> mappings)
      {
         var subset = mappings.OfType<MappingDataFormatParameter>().ToList();

         return new MappingProblem()
         {
            MissingMapping = dataColumns
               .Where(col => col.IsMandatory && subset.All(cm =>
                  cm.MappedColumn.Name != col.Name)).Select(col => col.Name)
               .ToList(),
            MissingUnit = subset.Where(cm => cm.MappedColumn.Unit.SelectedUnit == UnitDescription.InvalidUnit && cm.MappedColumn.ErrorStdDev.Equals("geometric")).Select(cm => cm.MappedColumn.Name)
               .ToList()
         };
      }
   }

   public class MappingProblem
   {
      public IReadOnlyList<string> MissingMapping { get; set; }
      public IReadOnlyList<string> MissingUnit { get; set; }
   }
}