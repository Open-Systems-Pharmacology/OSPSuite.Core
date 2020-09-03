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
      IDataSource ImportFromFile(IDataFormat format, IEnumerable<IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos);
      IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);
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

      public IDataSource ImportFromFile(IDataFormat format, IEnumerable<IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos)
      {
         IList<IDataSet> dataSets =
            dataSheets
               .Select
               (
                  s => new DataSet() 
                  { 
                     Data = format.Parse(s.RawData, columnInfos) 
                  } as IDataSet
               ).ToList();
         //TODO Resharper
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
   }
}