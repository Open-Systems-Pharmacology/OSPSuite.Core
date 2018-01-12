using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class UpdateValueOriginCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private readonly ValueOrigin _valueOrigin;
      private readonly ValueOriginSource _oldSource;
      private readonly string _oldDescription;
      private readonly ValueOriginSource _newSource;
      private readonly string _newDescription;

      public UpdateValueOriginCommand(ValueOriginSource newSource, string newDescription, IWithValueOrigin withValueOrigin, IOSPSuiteExecutionContext context)
         : this(newSource, newDescription, withValueOrigin.ValueOrigin, context.TypeFor(withValueOrigin))
      {
      }

      public UpdateValueOriginCommand(ValueOriginSource newSource, string newDescription, ValueOrigin valueOrigin, string objectTyppe)
      {
         _valueOrigin = valueOrigin;
         _newSource = newSource;
         _oldSource = valueOrigin.Source;
         _oldDescription = valueOrigin.Description;
         _newDescription = newDescription;
         CommandType = Command.CommandTypeEdit;
         ObjectType = objectTyppe;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var oldValueOriginDisplay = _valueOrigin.ToString();
         _valueOrigin.Source = _newSource;
         _valueOrigin.Description = _newDescription;
         Description = Command.UpdateValueOriginFrom(oldValueOriginDisplay, _valueOrigin.ToString());
      }

      protected override void ClearReferences()
      {
         //we keep the reference alive to allow for undo
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new UpdateValueOriginCommand(_oldSource, _oldDescription, _valueOrigin, ObjectType).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         //nothing to do here
      }
   }
}