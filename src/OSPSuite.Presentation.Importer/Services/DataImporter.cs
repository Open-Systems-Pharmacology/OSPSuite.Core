using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Presenter;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Importer.Services
{
   public class DataImporter : IDataImporter
   {
      private readonly IContainer _container;

      public DataImporter(IContainer container)
      {
         _container = container;
      }

      public IEnumerable<DataRepository> ImportDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         return importDataSets(metaDataCategories, columnInfos, dataImporterSettings, Mode.MultipleRepositories);
      }

      public DataRepository ImportDataSet(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         return importDataSets(metaDataCategories, columnInfos, dataImporterSettings, Mode.SingleRepository).FirstOrDefault();
      }

      private IEnumerable<DataRepository> importDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings, Mode mode)
      {
         using (var importerPresenter = _container.Resolve<IImporterPresenter>())
         {
            return importerPresenter.ImportDataSets(metaDataCategories, columnInfos, dataImporterSettings, mode);
         }
      }
   }
}