using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class AddObservedDataRowCommand : ObservedDataCommandBase
   {
      private readonly DataRowData _dataRowAdded;

      public AddObservedDataRowCommand(DataRepository observedData, DataRowData dataRowAdded)
         : base(observedData)
      {
         _dataRowAdded = dataRowAdded;
         CommandType = Command.CommandTypeAdd;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         _observedData.InsertValues(_dataRowAdded.BaseGridValue, _dataRowAdded.Data);

         Description = Command.AddObservedDataValueDescription(
            GetDisplayFor(_observedData.BaseGrid.Id, _dataRowAdded.BaseGridValue),
            _dataRowAdded.Data.KeyValues.Select(x => GetDisplayFor(x.Key, x.Value)));

         SetBuildingBlockParameters(context);
         context.PublishEvent(new ObservedDataTableChangedEvent(_observedData));
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new RemoveObservedDataRowCommand(_observedData, _observedData.BaseGrid.RightIndexOf(_dataRowAdded.BaseGridValue)).AsInverseFor(this);
      }

   }

}