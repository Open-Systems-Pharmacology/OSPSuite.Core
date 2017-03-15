using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
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
         _oldName = _dataRepository.Name;

         //rename repository 
         _dataRepository.Name = _newName;

         foreach (var column in _dataRepository.All())
         {
            var quantityPath = column.QuantityInfo.Path.ToList();
            replaceDataRepositoryNameIn(quantityPath);
            column.QuantityInfo.Path = quantityPath;
         }
         context.PublishEvent(new RenamedEvent(_dataRepository));
      }

      private void replaceDataRepositoryNameIn(List<string> quantityPath)
      {
         for (int i = 0; i < quantityPath.Count; i++)
         {
            if (string.Equals(quantityPath[i], _oldName))
               quantityPath[i] = _newName;
         }
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
