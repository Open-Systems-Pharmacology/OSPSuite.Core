using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class SetObservedDataColumnUnitCommand : ObservedDataCommandBase
   {
      private readonly string _columnId;
      private readonly Unit _newUnit;
      private Unit _oldUnit;

      public SetObservedDataColumnUnitCommand(DataRepository observedData, string columnId, Unit newUnit)
         : base(observedData)
      {
         _columnId = columnId;
         _newUnit = newUnit;
         CommandType = Command.CommandTypeEdit;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var column = _observedData[_columnId];
         _oldUnit = column.DisplayUnit;

         var allValuesInOldDisplayUnits = column.ConvertToDisplayValues(column.Values);
         column.DisplayUnit = _newUnit;
         column.Values = column.ConvertToBaseValues(allValuesInOldDisplayUnits);

         Description = Command.SetObservedDataColumnUnitCommandDescription(column.Name, _oldUnit.Name, _newUnit.Name);
         SetBuildingBlockParameters(context);
         context.PublishEvent(new ObservedDataTableChangedEvent(_observedData));
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new SetObservedDataColumnUnitCommand(_observedData, _columnId, _oldUnit).AsInverseFor(this);
      }
   }
}