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
      IDataSource ImportFromFile(IEnumerable<IDataSheet> sheets);
      IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data);
   }

   public class Importer : IImporter
   {
      private readonly IoC _container;
      private readonly IDataSourceFileParser _parser;

      public Importer(IoC container)
      {
         this._container = container;
         this._parser = container.Resolve<IDataSourceFileParser>();
      }

      public IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data)
      {
         return _container.ResolveAll<IDataFormat>()
            .Where(x => x.CheckFile(data));
      }

      public IDataSource ImportFromFile(IEnumerable<IDataSheet> sheets)
      {
         return new DataSource() {DataSets = (IList<IDataSet>) sheets.Select(s => new DataSet() {Data = s.Format.Parse(s.RawData)}).ToList()};
      }

      public IDataSourceFile LoadFile(string path)
      {
         var dataSource = _parser.For(path);
         foreach (var sheet in dataSource.DataSheets)
         {
            sheet.Value.AvailableFormats = AvailableFormats(sheet.Value.RawData).ToList();
            sheet.Value.Format = sheet.Value.AvailableFormats.FirstOrDefault();
         }

         return dataSource;
      }
   }
}