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
   public class AddObservedDataToProjectCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private readonly List<string> _dataRepositoryId;
      private IReadOnlyList<DataRepository> _observedData;

      public AddObservedDataToProjectCommand(IReadOnlyList<DataRepository> observedData)
      {
         _observedData = observedData;
         _dataRepositoryId = _observedData.Select(x => x.Id).ToList();
         CommandType = Command.CommandTypeAdd;
         ObjectType = ObjectTypes.ObservedData;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var project = context.Project;
         _observedData.Each(project.AddObservedData);
         _observedData.Each(x => project.GetOrCreateClassifiableFor<ClassifiableObservedData, DataRepository>(x));
         
         Description = _observedData.Count == 1 ? 
            Command.AddObservedDataToProjectDescription(_observedData[0].Name, project.Name) : 
            Command.AddManyObservedDataToProjectDescription(_observedData.AllNames(), project.Name);

         _observedData.Each(context.Register);
         context.PublishEvent(new ObservedDataAddedEvent(_observedData, project));
      }

      protected override void ClearReferences()
      {
         _observedData = null;
      }

      protected override ICommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new RemoveObservedDataFromProjectCommand(_observedData).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _observedData = _dataRepositoryId.Select(x => context.Project.ObservedDataBy(x)).ToList();
      }
   }
}