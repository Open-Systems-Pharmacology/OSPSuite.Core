using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Presentation.Views.Importer;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IModalImporterPresenter : IDisposablePresenter
   {
      IEnumerable<DataRepository> ImportDataSets(IImporterPresenter presenter, IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings);
   }

   public class ModalImporterPresenter : AbstractDisposablePresenter<IModalImporterView, IModalImporterPresenter>, IModalImporterPresenter
   {
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;

      public ModalImporterPresenter(IModalImporterView view, IDataSetToDataRepositoryMapper dataRepositoryMapper) : base(view)
      {
         _dataRepositoryMapper = dataRepositoryMapper;
      }

      public IEnumerable<DataRepository> ImportDataSets(IImporterPresenter presenter, IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         List<DataRepository> result = new List<DataRepository>();
         _view.FillImporterPanel(presenter.BaseView);
         presenter.OnTriggerImport += (s, d) =>
         {
            var i = 0;
            foreach (var pair in d.DataSource.DataSets.KeyValues)
            {
               foreach (var data in pair.Value.Data)
                  result.Add(_dataRepositoryMapper.ConvertImportDataSet(d.DataSource, i++, pair.Key));
            }
         };
         _view.AttachImporterPresenter(presenter);
         _view.Display();
         return result;
      }
   }
}
