using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Extensions;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class RemoveObservedDataFromProjectCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private IReadOnlyList<DataRepository> _observedData;
      private List<byte[]> _serializationStream;

      public RemoveObservedDataFromProjectCommand(IReadOnlyList<DataRepository> observedData)
      {
         _observedData = observedData;
         CommandType = Command.CommandTypeDelete;
         ObjectType = ObjectTypes.ObservedData;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var project = context.Project;
         _observedData.Each(project.RemoveObservedData);
         Description = _observedData.Count == 1 ? 
            Command.RemoveObservedDataFromProjectDescription(_observedData[0].Name, project.Name) : 
            Command.RemoveManyObservedDataToProjectDescription(_observedData.AllNames(), project.Name);
         
         _serializationStream = _observedData.Select(context.Serialize).ToList();
         _observedData.Each(context.Unregister);
         context.PublishEvent(new ObservedDataRemovedEvent(_observedData, project));
      }

      protected override void ClearReferences()
      {
         _observedData = null;
      }

      protected override ICommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new AddObservedDataToProjectCommand(_observedData).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _observedData = _serializationStream.Select(context.Deserialize<DataRepository>).ToList();
      }
   }
}