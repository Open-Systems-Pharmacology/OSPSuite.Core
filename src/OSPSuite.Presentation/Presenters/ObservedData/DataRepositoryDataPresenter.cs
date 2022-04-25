using System.Data;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IDataRepositoryDataPresenter : IBaseDataRepositoryDataPresenter<IDataRepositoryDataView>, IDataRepositoryItemPresenter,
      IListener<ObservedDataValueChangedEvent>,
      IListener<ObservedDataTableChangedEvent>
   {
   }

   public class DataRepositoryDataPresenter : BaseDataRepositoryDataPresenter<IDataRepositoryDataView, IDataRepositoryDataPresenter>,
      IDataRepositoryDataPresenter
   {
      private readonly IDataRepositoryExportTask _dataRepositoryTask;

      public DataRepositoryDataPresenter(IDataRepositoryDataView view, IDataRepositoryExportTask dataRepositoryTask)
         : base(view)
      {
         _dataRepositoryTask = dataRepositoryTask;
      }

      protected override DataTable MapDataTableFromColumns()
      {
         return _dataRepositoryTask.ToDataTable(_observedData, new DataColumnExportOptions {ForceColumnTypeAsObject = true}).First();
      }
   }
}