using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class SetObservedDataValueCommand : ObservedDataCommandBase
   {
      private readonly CellValueChanged _cellValueChanged;

      public SetObservedDataValueCommand(DataRepository observedData, CellValueChanged cellValueChanged)
         : base(observedData)
      {
         _observedData = observedData;
         _observedDataId = _observedData.Id;
         _cellValueChanged = cellValueChanged;
         CommandType = Command.CommandTypeEdit;
         ObjectType = ObjectTypes.ObservedData;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var column = _observedData[_cellValueChanged.ColumnId];
         BuildingBlockName = _observedData.Name;
         BuildingBlockType = context.TypeFor(_observedData);

         if (!column.IsBaseGrid())
            setValueInCell(column, context);
         else
         {
            var baseGrid = column.DowncastTo<BaseGrid>();
            var newIndex = baseGrid.RightIndexOf(_cellValueChanged.NewValue);
            //same index, nothing to change
            if (newIndex == _cellValueChanged.RowIndex)
               setValueInCell(baseGrid, context);
            else
            {
               //new index. need to remove swap out the old one and for the new one
               _observedData.SwapValues(_cellValueChanged.OldValue, _cellValueChanged.NewValue);
               context.PublishEvent(new ObservedDataTableChangedEvent(_observedData));
            }
         }

         var baseGridNameValueUnits = GetDisplayFor(_observedData.BaseGrid.Id, _observedData.BaseGrid.Values[_cellValueChanged.RowIndex]);
         var oldNameValueUnits = GetDisplayFor(column.Id, _cellValueChanged.OldValue);
         var newNameValueUnits = GetDisplayFor(column.Id, _cellValueChanged.NewValue);
         Description = Command.SetObservedDataValueDescription(baseGridNameValueUnits, oldNameValueUnits, newNameValueUnits);
      }

      private void setValueInCell(DataColumn column, IOSPSuiteExecutionContext context)
      {
         column[_cellValueChanged.RowIndex] = _cellValueChanged.NewValue;
         context.PublishEvent(new ObservedDataValueChangedEvent(_observedData));
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         var inverseCellValueChanged = _cellValueChanged.Clone();
         inverseCellValueChanged.NewValue = _cellValueChanged.OldValue;
         inverseCellValueChanged.OldValue = _cellValueChanged.NewValue;
         return new SetObservedDataValueCommand(_observedData, inverseCellValueChanged).AsInverseFor(this);
      }
   }

}