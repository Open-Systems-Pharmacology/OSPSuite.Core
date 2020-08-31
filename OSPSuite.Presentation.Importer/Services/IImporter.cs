using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName = null);
      IDataSource ImportFromFile(IDataSourceFile dataSourceFile);
      IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos);
   }

   public class Importer : IImporter
   {
      private readonly IoC _container;
      private readonly IDataSourceFileParser _parser;
      private readonly IDialogCreator _dialogCreator;

      public Importer(IDialogCreator dialogCreator, IoC container, IDataSourceFileParser parser)
      {
         _dialogCreator = dialogCreator;
         _container = container;
         _parser = parser;
      }

      public IEnumerable<IDataFormat> AvailableFormats(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         return _container.ResolveAll<IDataFormat>()
            .Where(x => x.SetParameters(data, columnInfos));
      }

      public IDataSource ImportFromFile(IDataSourceFile dataSourceFile)
      {
         //TODO Resharper
         return new DataSource() 
         {
            DataSets = 
               (IList<IDataSet>) dataSourceFile.DataSheets
                  .Select(s => new DataSet() 
                  {
                     Data = dataSourceFile.Format.Parse
                     (
                        s.Value.RawData, 
                        new List<ColumnInfo> 
                        { 
                           new ColumnInfo() { DisplayName = "Time" },
                           new ColumnInfo() { DisplayName = "Concentration" },
                           new ColumnInfo() { DisplayName = "Error" }
                        }
                     ) 
                  }).ToList()
         };
      }

      public IDataSourceFile LoadFile(IReadOnlyList<ColumnInfo> columnInfos, string fileName = null) //TODO add optional parameter to be able to use the "..." button
      {
         var filename = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA, fileName);
         var dataSource = _parser.For(filename);
         dataSource.AvailableFormats = AvailableFormats(dataSource.DataSheets.ElementAt(0).Value.RawData, columnInfos).ToList();
         dataSource.Format = dataSource.AvailableFormats.FirstOrDefault();
         //TODO: check that all sheets are supporting the formats...
         
         return dataSource;
      }
   }
}