using OSPSuite.Presentation.Importer.Core;
using System.Collections.Generic;
using System.Linq;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(string path);
      IDataSource ImportFromFile(IEnumerable<IDataSheet> sheets);
      IList<IDataFormat> AvailableFormats(IUnformattedData data);
   }

   public class Importer : IImporter
   {
      private readonly IoC container;
      private readonly IDataSourceFileParser parser;

      public Importer(IoC container)
      {
         this.container = container;
         this.parser = container.Resolve<IDataSourceFileParser>();
      }

      public IList<IDataFormat> AvailableFormats(IUnformattedData data)
      {
         return GetType().Assembly.GetTypes()
            .Where(x => typeof(IDataFormat).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(x => container.Resolve<IDataFormat>(x.FullName))
            .Where(x => x.CheckFile(data))
            .ToList();
      }

      public IDataSource ImportFromFile(IEnumerable<IDataSheet> sheets)
      {
         return new DataSource() { DataSets = (IList<IDataSet>) sheets.Select(s => new DataSet() { Data = s.Format.Parse(s.RawData) }).ToList() };
      }

      public IDataSourceFile LoadFile(string path)
      {
         return parser.For(path);
      }
   }
}
