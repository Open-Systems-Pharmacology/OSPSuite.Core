using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class RenameObservedDataCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private readonly string _dataRepositoryId;
      private readonly string _newName;
      private DataRepository _dataRepository;
      private string _oldName;

      public RenameObservedDataCommand(DataRepository dataRepository, string newName)
      {
         _dataRepository = dataRepository;
         
         _newName = newName;
         _dataRepositoryId = dataRepository.Id;
         CommandType = Command.CommandTypeRename;
         Description = Command.RenameObservedData(dataRepository.Name, _newName);
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var dataRepositoryNamer = context.Resolve<IDataRepositoryNamer>();
         _oldName = _dataRepository.Name;

         //rename repository 
         dataRepositoryNamer.Rename(_dataRepository, _newName);

         context.PublishEvent(new RenamedEvent(_dataRepository));
      }

      protected override void ClearReferences()
      {
         _dataRepository = null;
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new RenameObservedDataCommand(_dataRepository, _oldName).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _dataRepository = context.Project.ObservedDataBy(_dataRepositoryId);
      }
   }
}
