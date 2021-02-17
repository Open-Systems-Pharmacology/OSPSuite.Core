using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Presentation.Views.Importer;
using System;
using System.Collections.Generic;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IModalImporterPresenter : IDisposablePresenter
   {
      (IEnumerable<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(IImporterPresenter presenter, IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings);
   }

   public class ModalImporterPresenter : AbstractDisposablePresenter<IModalImporterView, IModalImporterPresenter>, IModalImporterPresenter
   {
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;

      public ModalImporterPresenter(IModalImporterView view, IDataSetToDataRepositoryMapper dataRepositoryMapper) : base(view)
      {
         _dataRepositoryMapper = dataRepositoryMapper;
      }

      public (IEnumerable<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(IImporterPresenter presenter, IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         List<DataRepository> result = new List<DataRepository>();
         _view.FillImporterPanel(presenter.BaseView);
         var id = Guid.NewGuid().ToString();
         presenter.OnTriggerImport += (s, d) =>
         {
            var i = 0;
            foreach (var pair in d.DataSource.DataSets.KeyValues)
            {
               foreach (var data in pair.Value.Data)
               {
                  var dataRepo = _dataRepositoryMapper.ConvertImportDataSet(d.DataSource, i++, pair.Key);
                  dataRepo.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Configuration", Value = id });
                  result.Add(dataRepo);
               }
                  
            }
         };
         var configuration = presenter.getConfiguration();
         configuration.UniqueId = id;
         _view.AttachImporterPresenter(presenter);
         _view.Display();
         return (result, configuration);
      }
   }
}
