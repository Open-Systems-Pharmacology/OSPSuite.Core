using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IDataRepositoryDataPresenter : IBaseDataRepositoryDataPresenter<IDataRepositoryDataView>, IDataRepositoryItemPresenter,
      IUnitsInColumnPresenter<int>,
      IListener<ObservedDataValueChangedEvent>,
      IListener<ObservedDataTableChangedEvent>
   { 
      int NumberOfObservations { get; }
   }

   public class DataRepositoryDataPresenter : BaseDataRepositoryDataPresenter<IDataRepositoryDataView, IDataRepositoryDataPresenter>,
      IDataRepositoryDataPresenter
   {
      private readonly IDataRepositoryExportTask _dataRepositoryTask;
      private readonly IEditObservedDataTask _editObservedDataTask;

      public DataRepositoryDataPresenter(IDataRepositoryDataView view, IDataRepositoryExportTask dataRepositoryTask,
         IEditObservedDataTask editObservedDataTask)
         : base(view)
      {
         _dataRepositoryTask = dataRepositoryTask;
         _editObservedDataTask = editObservedDataTask;
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

      public void ChangeUnit(int columnIndex, Unit newUnit)
      {
         var columnId = GetColumnIdFromColumnIndex(columnIndex);
         AddCommand(_editObservedDataTask.SetUnit(_observedData, columnId, newUnit));
      }

      public Unit DisplayUnitFor(int columnIndex)
      {
         var col = columnFromColumnIndex(columnIndex);
         return col?.DisplayUnit;
      }

      private DataColumn columnFromColumnIndex(int columnIndex)
      {
         var id = GetColumnIdFromColumnIndex(columnIndex);
         return _observedData.Contains(id) ? _observedData[id] : null;
      }
   }
}