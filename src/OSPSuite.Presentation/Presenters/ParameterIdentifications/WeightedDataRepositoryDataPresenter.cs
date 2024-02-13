using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IWeightedDataRepositoryDataPresenter : IBaseDataRepositoryDataPresenter<IWeightedDataRepositoryDataView>
   {
      void EditObservedData(WeightedObservedData weightedObservedData);
      void ChangeWeight(int weightIndex, float newWeight);
      bool ColumnIsInDataRepository(DataColumn column);
      void DisableRepositoryColumns();
      void SelectRow(int rowIndex);
      IEnumerable<string> GetValidationMessagesForWeight(string weightValue);
      void Clear();
   }

   public class WeightedDataRepositoryDataPresenter : BaseDataRepositoryDataPresenter<IWeightedDataRepositoryDataView, IWeightedDataRepositoryDataPresenter>, IWeightedDataRepositoryDataPresenter
   {
      private readonly IWeightedDataRepositoryToDataTableMapper _weightedDataRepositoryToDataTableMapper;
      private WeightedObservedData _weightedObservedData;

      public WeightedDataRepositoryDataPresenter(IWeightedDataRepositoryDataView view, IWeightedDataRepositoryToDataTableMapper weightedDataRepositoryToDataTableMapper) : base(view)
      {
         _weightedDataRepositoryToDataTableMapper = weightedDataRepositoryToDataTableMapper;
      }

      protected override DataTable MapDataTableFromColumns() => _weightedDataRepositoryToDataTableMapper.MapFrom(_weightedObservedData);

      public void EditObservedData(WeightedObservedData weightedObservedData)
      {
         _weightedObservedData = weightedObservedData;
         EditObservedData(weightedObservedData.ObservedData);
      }

      public void Clear()
      {
         _weightedObservedData = null;
         _view.Clear();
      }

      public void ChangeWeight(int weightIndex, float newWeight) => _weightedObservedData.Weights[weightIndex] = newWeight;

      public bool ColumnIsInDataRepository(DataColumn column)
      {
         var columnId = GetColumnIdFromColumnIndex(_dataTable.Columns.IndexOf(column));

         return _weightedObservedData.ObservedData.Columns.ExistsById(columnId);
      }

      public void DisableRepositoryColumns()
      {
         if (_dataTable == null) return;
         foreach (DataColumn column in _dataTable.Columns)
         {
            if (ColumnIsInDataRepository(column))
               _view.DisplayColumnReadOnly(column);
         }
      }

      public void SelectRow(int rowIndex) => _view.SelectRow(rowIndex);

      public IEnumerable<string> GetValidationMessagesForWeight(string weightValue)
      {
         if (!float.TryParse(weightValue, out var proposedValue))
            return new[] { Error.ValueIsRequired };

         return isValidWeight(proposedValue) ? Enumerable.Empty<string>() : new[] { Error.WeightValueCannotBeNegative };
      }

      private bool isValidWeight(float value) => value >= 0;
   }
}