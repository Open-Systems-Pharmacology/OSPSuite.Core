using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(string path);
      IDataSource ImportFromFile(IDataSourceFile dataSourceFile);
      IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data);
   }

   public class Importer : IImporter
   {
      private readonly IoC _container;
      private readonly IDataSourceFileParser _parser;

      public Importer(IoC container, IDataSourceFileParser parser)
      {
         _container = container;
         _parser = parser;
      }

      public IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data)
      {
         return _container.ResolveAll<IDataFormat>()
            .Where(x => x.SetParameters(data));
      }

      public IDataSource ImportFromFile(IDataSourceFile dataSourceFile)
      {
         return new DataSource() {DataSets = (IList<IDataSet>) dataSourceFile.DataSheets.Select(s => new DataSet() {Data = dataSourceFile.Format.Parse(s.Value.RawData)}).ToList()};
      }

      public IDataSourceFile LoadFile(string path)
      {
         var dataSource = _parser.For(path);
         dataSource.AvailableFormats = AvailableFormats(dataSource.DataSheets.ElementAt(0).Value.RawData).ToList();
         dataSource.Format = dataSource.AvailableFormats.FirstOrDefault();
         //TODO: check that all sheets are supporting the formats...
         
         return dataSource;
      }
   }
}