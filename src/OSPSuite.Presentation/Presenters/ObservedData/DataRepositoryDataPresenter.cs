using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.Utility.Events;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IDataRepositoryDataPresenter : IBaseDataRepositoryDataPresenter<IDataRepositoryDataView>, IDataRepositoryItemPresenter,
      IListener<ObservedDataValueChangedEvent>,
      IListener<ObservedDataTableChangedEvent>
   { 
      int NumberOfObservations { get; }
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
         return _dataRepositoryTask.ToDataTable(_observedData, new DataColumnExportOptions { ForceColumnTypeAsObject = true }).First();
      }

      public int NumberOfObservations => _observedData.BaseGrid.Count;

      public IEnumerable<Unit> AvailableUnitsFor(int columnIndex)
      {
         var col = columnFromColumnIndex(columnIndex);
         return col != null ? col.Dimension.Units : Enumerable.Empty<Unit>();
      }

      private DataColumn columnFromColumnIndex(int columnIndex)
      {
         var id = GetColumnIdFromColumnIndex(columnIndex);
         return _observedData.Contains(id) ? _observedData[id] : null;
      }
   }
}