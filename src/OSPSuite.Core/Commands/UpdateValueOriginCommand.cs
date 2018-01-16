using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class UpdateValueOriginCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private readonly ValueOrigin _valueOrigin;
      private ValueOrigin _oldValueOrigin;
      private ValueOrigin _newValueOrigin;

      public UpdateValueOriginCommand(ValueOrigin newValueOrigin, IWithValueOrigin withValueOrigin, IOSPSuiteExecutionContext context)
         : this(newValueOrigin, withValueOrigin.ValueOrigin, context.TypeFor(withValueOrigin))
      {
      }

      public UpdateValueOriginCommand(ValueOrigin newValueOrigin, ValueOrigin valueOrigin, string objectTyppe)
      {
         _valueOrigin = valueOrigin;
         _newValueOrigin = newValueOrigin;
         CommandType = Command.CommandTypeEdit;
         ObjectType = objectTyppe;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var oldValueOriginDisplay = _valueOrigin.ToString();
         _oldValueOrigin = _valueOrigin.Clone();
         _valueOrigin.UpdateFrom(_newValueOrigin);
         Description = Command.UpdateValueOriginFrom(oldValueOriginDisplay, _valueOrigin.ToString());
      }

      protected override void ClearReferences()
      {
         _newValueOrigin = null;
         //we keep the reference alive to allow for undo
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new UpdateValueOriginCommand(_oldValueOrigin, _valueOrigin, ObjectType).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         //nothing to do here
      }
   }
}