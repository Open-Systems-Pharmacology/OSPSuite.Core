using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class RemoveObservedDataRowCommand : ObservedDataCommandBase
   {
      private readonly DataRowData _dataRowData = new DataRowData();
      private readonly int _dataRowIndex;

      public RemoveObservedDataRowCommand(DataRepository dataRepository, int dataRowIndex)
         : base(dataRepository)
      {
         _dataRowIndex = dataRowIndex;
         CommandType = Command.CommandTypeDelete;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         _dataRowData.FillFromRepository(_dataRowIndex, _observedData);
         _observedData.RemoveValuesAt(_observedData.BaseGrid.RightIndexOf(_dataRowData.BaseGridValue));
         SetBuildingBlockParameters(context);
         var baseGridNameValueUnit = GetDisplayFor(_observedData.BaseGrid.Id, _dataRowData.BaseGridValue);
         var removedNameValueUnits = _dataRowData.Data.KeyValues.Select(x => GetDisplayFor(x.Key, x.Value));

         Description = Command.RemoveObservedDataValueDescription(baseGridNameValueUnit, removedNameValueUnits);
         context.PublishEvent(new ObservedDataTableChangedEvent(_observedData));
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new AddObservedDataRowCommand(_observedData, _dataRowData).AsInverseFor(this);
      }
   }
}