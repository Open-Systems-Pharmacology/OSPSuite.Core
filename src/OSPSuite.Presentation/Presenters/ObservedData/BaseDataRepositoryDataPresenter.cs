using System.Data;
using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.Utility;
using OSPSuite.Utility.Format;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IBaseDataRepositoryDataPresenter<TView> : IPresenter<TView>, ILatchable where TView : IView
   {
      Color BackgroundColorForRow(int sourceRow);
      bool AnyObservationInThisRowIsBelowLLOQ(int sourceRow);
      string ToolTipTextForRow(int observedDataRowIndex);
   }

   public abstract class BaseDataRepositoryDataPresenter<TView, TPresenter> : AbstractSubPresenter<TView, TPresenter>
      where TView : IBaseDataRepositoryDataView<TPresenter>
      where TPresenter : IBaseDataRepositoryDataPresenter<TView>
   {
      protected DataRepository _observedData;
      protected DataTable _datatable;
      private readonly NumericFormatter<double> _numericFormatter = new NumericFormatter<double>(NumericFormatterOptions.Instance);

      protected BaseDataRepositoryDataPresenter(TView view) : base(view)
      {
      }

      public bool IsLatched { get; set; }

      protected abstract DataTable MapDataTableFromColumns();

      public Color BackgroundColorForRow(int sourceRow)
      {
         if (AnyObservationInThisRowIsBelowLLOQ(sourceRow))
            return Colors.BelowLLOQ;

         return Colors.DefaultRowColor;
      }

      public bool AnyObservationInThisRowIsBelowLLOQ(int sourceRow)
      {
         return _observedData.HasObservationBelowLLOQ(sourceRow);
      }

      public void EditObservedData(DataRepository observedData)
      {
         _observedData = observedData;
         Rebind();
      }

      protected void Rebind()
      {
         _datatable = MapDataTableFromColumns();
         _view.BindTo(_datatable);
      }

      public string ToolTipTextForRow(int observedDataRowIndex)
      {
         var column = _observedData.ColumnWithValueBelowLLOQ(observedDataRowIndex);
         if (column == null)
            return string.Empty;

         var baseValue = convertCellToFloat(observedDataRowIndex, column);
         var displayValue = column.ConvertToDisplayUnit(baseValue);
         var valueAsString = _numericFormatter.Format(displayValue);

         return ToolTips.LLOQTooltip(column.Name, valueAsString, column.DisplayUnit.ToString(), column.ConvertToDisplayUnit(column.DataInfo.LLOQ));
      }

      private float convertCellToFloat(int sourceRow, DataColumn observationColumn)
      {
         return observationColumn.Values[sourceRow];
      }

      public void Handle(ObservedDataValueChangedEvent eventToHandle)
      {
         if (IsLatched) return;

         handle(eventToHandle);
      }

      private void handle(ObservedDataEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         Rebind();
      }

      private bool canHandle(ObservedDataEvent eventToHandle)
      {
         return Equals(eventToHandle.ObservedData, _observedData);
      }

      public void Handle(ObservedDataTableChangedEvent eventToHandle)
      {
         handle(eventToHandle);
      }

      protected string GetColumnIdFromColumnIndex(int columnIndex)
      {
         var column = _datatable.Columns[columnIndex];
         return ColumnIdFromColumn(column);
      }

      protected static string ColumnIdFromColumn(System.Data.DataColumn column)
      {
         return column.ExtendedProperties.Contains(Constants.DATA_REPOSITORY_COLUMN_ID) ? column.ExtendedProperties[Constants.DATA_REPOSITORY_COLUMN_ID].ToString() : string.Empty;
      }
   }
}