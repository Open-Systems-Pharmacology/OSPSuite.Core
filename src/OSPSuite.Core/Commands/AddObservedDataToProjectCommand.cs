using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class AddObservedDataToProjectCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private readonly string _dataRepositoryId;
      private DataRepository _observedData;

      public AddObservedDataToProjectCommand(DataRepository observedData)
      {
         _observedData = observedData;
         _dataRepositoryId = _observedData.Id;
         CommandType = Command.CommandTypeAdd;
         ObjectType = ObjectTypes.ObservedData;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var project = context.Project;
         project.AddObservedData(_observedData);
         project.GetOrCreateClassifiableFor<ClassifiableObservedData, DataRepository>(_observedData);
         Description = Command.AddObservedDataToProjectDescription(_observedData.Name, project.Name);
         context.Register(_observedData);
         context.PublishEvent(new ObservedDataAddedEvent(_observedData, project));
      }

      protected override void ClearReferences()
      {
         _observedData = null;
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new RemoveObservedDataFromProjectCommand(_observedData).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _observedData = context.Project.ObservedDataBy(_dataRepositoryId);
      }
   }
}