using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class RemoveObservedDataFromProjectCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private DataRepository _observedData;
      private byte[] _serializationStream;

      public RemoveObservedDataFromProjectCommand(DataRepository observedData)
      {
         _observedData = observedData;
         CommandType = Command.CommandTypeDelete;
         ObjectType = ObjectTypes.ObservedData;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var project = context.Project;
         project.RemoveObservedData(_observedData);
         Description = Command.RemoveObservedDataFromProjectDescription(_observedData.Name, project.Name);
         _serializationStream = context.Serialize(_observedData);
         context.Unregister(_observedData);
         context.PublishEvent(new ObservedDataRemovedEvent(_observedData, project));
      }

      protected override void ClearReferences()
      {
         _observedData = null;
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new AddObservedDataToProjectCommand(_observedData).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _observedData = context.Deserialize<DataRepository>(_serializationStream);
      }
   }
}