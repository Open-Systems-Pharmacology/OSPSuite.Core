using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class UpdateObservedDataMolWeightCommand : ObservedDataCommandBase
   {
      private readonly IDimension _molWeightDimension;
      private readonly double _oldValue;
      private readonly double _newValue;

      public UpdateObservedDataMolWeightCommand(DataRepository observedData, IDimension molWeightDimension, double oldValue, double newValue) : base(observedData)
      {
         _molWeightDimension = molWeightDimension;
         _oldValue = oldValue;
         _newValue = newValue;
         CommandType = Command.CommandTypeEdit;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         _observedData.AllButBaseGrid().Each(x => x.DataInfo.MolWeight = _newValue);
         var oldValueDisplay = molWeightDisplayValue(_oldValue);
         var newValueDisplay = molWeightDisplayValue(_newValue);
         Description = Command.SetObservedDataParameterCommandDescription(oldValueDisplay, newValueDisplay, _observedData.Name, Constants.Parameters.MOL_WEIGHT);
         SetBuildingBlockParameters(context);
         context.PublishEvent(new ObservedDataMetaDataChangedEvent(_observedData));

      }

      private string molWeightDisplayValue(double value)
      {
         var displayUnit = _molWeightDimension.DefaultUnit;
         var unitFormatter = new UnitFormatter(displayUnit);
         return unitFormatter.Format(_molWeightDimension.BaseUnitValueToUnitValue(displayUnit, value));
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new UpdateObservedDataMolWeightCommand(_observedData, _molWeightDimension, _newValue, _oldValue).AsInverseFor(this);
      }
   }
}